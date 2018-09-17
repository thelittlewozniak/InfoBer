using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using InfoBer.Messaging;
using InfoBer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace InfoBer.Controllers
{
    public class ProblemController : Controller
    {
        private Context _context;
        private static readonly HttpClient client = new HttpClient();
        public ProblemController(Context context)
        {
            _context = context;
        }
        public IActionResult Index(string error)
        {
            ViewBag.error = error;
            if (HttpContext.Session.GetString("username") != null)
            {
                var u = (from a in _context.Users where a.Username == HttpContext.Session.GetString("username") select a).Include(e => e.School).Include(e=>e.Departement).FirstOrDefault();
                ViewBag.problems = (from a in _context.Problems where a.Taker == null && a.Writer.School==u.School && a.Writer.Departement==u.Departement select a).Include(e => e.Writer).ToList();
            }
            else
            {
                ViewBag.problems = (from a in _context.Problems where a.Taker == null select a).Include(e => e.Writer).ToList();
            }
            return View();
        }
        public IActionResult Publish()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Publish(string problemTitle)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var prob = new Problem();
                prob.ProblemTitle = problemTitle;
                prob.status = false;
                prob.Writer = (from a in _context.Users where a.Username == HttpContext.Session.GetString("username") select a).FirstOrDefault();
                _context.Problems.Add(prob);
                _context.SaveChanges();
                return RedirectToAction("Messages", "User");
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }
        [HttpGet]
        public async Task<IActionResult> TakeIt(string id)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                string error;
                int i;
                try
                {
                    i = Convert.ToInt32(id);
                }
                catch (Exception)
                {
                    return RedirectToAction("Index", "Problem");
                }
                if ((from e in _context.Problems where e.Id == i && e.status == false select e).FirstOrDefault() != null)
                {
                    if ((from e in _context.Problems where e.Id == i && e.status == false select e).Include(e => e.Writer).FirstOrDefault().Writer.Username != HttpContext.Session.GetString("username"))
                    {
                        var user = (from e in _context.Users where e.Username == HttpContext.Session.GetString("username") select e).FirstOrDefault();
                        _context.Problems.SingleOrDefault(e => e.Id == i).Taker = user;
                        _context.SaveChanges();
                        var problem = (from e in _context.Problems where e.Id == i select e).FirstOrDefault();
                        List<string> users = new List<string>
                        {
                            Convert.ToString(problem.Writer.Id),
                            Convert.ToString(problem.Taker.Id)
                        };
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                        string json = JsonConvert.SerializeObject(new { name = Convert.ToString(problem.Id), user_ids = users });
                        var response1 = await client.PostAsync("https://us1.pusherplatform.io/services/chatkit/v1/479278d3-1b27-4b2c-9bfa-eca3af70b5f1/rooms", new StringContent(json, Encoding.UTF8, "application/json"));
                        var responseString1 = await response1.Content.ReadAsStringAsync();
                        var r = JsonConvert.DeserializeObject<ResponseProblem>(responseString1);
                        _context.Problems.SingleOrDefault(e => e.Id == problem.Id).IdChat = r.id;
                        _context.SaveChanges();
                        return RedirectToAction("Messages", "User");
                    }
                    else
                    {
                        error = "You cannot take your problem.";
                        return RedirectToAction("Index", "Problem",new { error });
                    }
                }
                else
                {
                    error = "Problem is already taked! Sorry!";
                    return RedirectToAction("Index", "Problem", new { error });
                }
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }
        [HttpGet]
        public IActionResult DeleteIt(string id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIt(string id, string result, string commentary)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var prob = (from e in _context.Problems where e.IdChat == id select e).Include(e=>e.Taker).FirstOrDefault();
                var user = (from e in _context.Users where e == prob.Taker select e).FirstOrDefault();
                Rating rate = new Rating();
                rate.Result = Convert.ToDouble(result);
                rate.Commentary = commentary;
                rate.User = user;
                _context.Ratings.Add(rate);
                _context.Problems.SingleOrDefault(e => e.Id == prob.Id).status = true;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                var response1 = await client.DeleteAsync("https://us1.pusherplatform.io/services/chatkit/v1/479278d3-1b27-4b2c-9bfa-eca3af70b5f1/rooms/" + prob.IdChat);
                var responseString1 = await response1.Content.ReadAsStringAsync();
                _context.SaveChanges();
                return RedirectToAction("Profile", "User");
            }
            else
                return RedirectToAction("Index", "User");
        }
    }
}