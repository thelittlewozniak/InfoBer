using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using InfoBer.Messaging;
using InfoBer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace InfoBer.Controllers
{
    public class AdminController : Controller
    {
        private Context _context;
        private static readonly HttpClient client = new HttpClient();
        public AdminController(Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var u = (from e in _context.Users where e.Username == HttpContext.Session.GetString("username") select e).FirstOrDefault();
                if (u.Admin)
                    return View();
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }
        public IActionResult SeeSchools()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var u = (from e in _context.Users where e.Username == HttpContext.Session.GetString("username") select e).FirstOrDefault();
                if (u.Admin)
                {
                    ViewBag.schools = (from e in _context.Schools select e).ToList();
                    return View();
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }
        public IActionResult School(string id)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var u = (from e in _context.Users where e.Username == HttpContext.Session.GetString("username") select e).FirstOrDefault();
                if (u.Admin)
                {
                    ViewBag.school = (from e in _context.Schools where e.Id == Convert.ToInt32(id) select e).Include(e=>e.Departements).FirstOrDefault();
                    return View();
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }
        public IActionResult AddSchool()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddSchool(string Name, string Adress, string Director, string Email)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var u = (from e in _context.Users where e.Username == HttpContext.Session.GetString("username") select e).FirstOrDefault();
                if (u.Admin)
                {
                    School newSchool = new School();
                    newSchool.Name = Name;
                    newSchool.Adress = Adress;
                    newSchool.Director = Director;
                    newSchool.Email = Email;
                    _context.Schools.Add(newSchool);
                    _context.SaveChanges();
                    return RedirectToAction("SeeSchools", "Admin");
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }
        public IActionResult AddDepartement(string school)
        {
            ViewBag.school = school;
            return View();
        }
        [HttpPost]
        public IActionResult AddDepartement(string Name,string school)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var u = (from e in _context.Users where e.Username == HttpContext.Session.GetString("username") select e).FirstOrDefault();
                if (u.Admin)
                {
                    var s = (from e in _context.Schools where e.Id==Convert.ToInt32(school) select e).FirstOrDefault();
                    Departement newDepartement = new Departement();
                    newDepartement.Name = Name;
                    newDepartement.School = s;
                    _context.Departements.Add(newDepartement);
                    _context.SaveChanges();
                    return RedirectToAction("School", "Admin",new { id = s.Id });
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "User");
            }

        }
        public IActionResult SeeUsers()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var u = (from e in _context.Users where e.Username == HttpContext.Session.GetString("username") select e).FirstOrDefault();
                if (u.Admin)
                {
                    ViewBag.users = (from e in _context.Users select e).Include(e=>e.School).Include(e=>e.Departement).ToList();
                    return View();
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }
        public IActionResult SeeUser(string id)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var u = (from e in _context.Users where e.Username == HttpContext.Session.GetString("username") select e).FirstOrDefault();
                if (u.Admin)
                {
                    ViewBag.user = (from e in _context.Users where e.Id==Convert.ToInt32(id) select e).Include(e => e.School).Include(e => e.Departement).Include(e=>e.Ratings).FirstOrDefault();
                    ViewBag.problems = (from e in _context.Problems where e.Taker.Id == Convert.ToInt32(id) || e.Writer.Id == Convert.ToInt32(id) select e).Include(e => e.Taker).Include(e => e.Writer).ToList();
                    return View();
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }
        public IActionResult UpgradeAdmin(string id)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var u = (from e in _context.Users where e.Username == HttpContext.Session.GetString("username") select e).FirstOrDefault();
                if (u.Admin)
                {
                    _context.Users.Where(e => e.Id == Convert.ToInt32(id)).FirstOrDefault().Admin = true;
                    _context.SaveChanges();
                    return RedirectToAction("SeeUser", "Admin", new { id = id });
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }
        public async Task<IActionResult> DeleteProblem(string id)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var u = (from e in _context.Users where e.Username == HttpContext.Session.GetString("username") select e).FirstOrDefault();
                if (u.Admin)
                {
                    var prob = (from e in _context.Problems where e.Id == Convert.ToInt32(id) select e).Include(e => e.Taker).FirstOrDefault();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", u.Token);
                    var response1 = await client.DeleteAsync("https://us1.pusherplatform.io/services/chatkit/v1/479278d3-1b27-4b2c-9bfa-eca3af70b5f1/rooms/" + prob.IdChat);
                    var responseString1 = await response1.Content.ReadAsStringAsync();
                    _context.Problems.Remove(prob);
                    _context.SaveChanges();
                    return RedirectToAction("SeeUsers","Admin");
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }
        public IActionResult SeeProblems()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var u = (from e in _context.Users where e.Username == HttpContext.Session.GetString("username") select e).FirstOrDefault();
                if (u.Admin)
                {
                    ViewBag.problems = (from e in _context.Problems select e).Include(e => e.Writer).Include(e=>e.Writer.School).Include(e=>e.Writer.Departement).ToList();
                    return View();
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }
        public async Task<IActionResult> SeeProblem(string id)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var u = (from e in _context.Users where e.Username == HttpContext.Session.GetString("username") select e).FirstOrDefault();
                if (u.Admin)
                {
                    var prob = (from e in _context.Problems where e.Id == Convert.ToInt32(id) select e).Include(e => e.Writer).Include(e => e.Writer.School).Include(e => e.Writer.Departement).Include(e => e.Taker).FirstOrDefault();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", u.Token);
                    var response1 = await client.GetAsync("https://us1.pusherplatform.io/services/chatkit/v1/479278d3-1b27-4b2c-9bfa-eca3af70b5f1/rooms/" + prob.IdChat + "/messages?initial_id=1&direction=newer");
                    var responseString1 = await response1.Content.ReadAsStringAsync();
                    var r = JsonConvert.DeserializeObject<List<MessageFromChat>>(responseString1);
                    var messages = new List<Message>();
                    foreach (var i in r)
                    {
                        var user = (from e in _context.Users where e.Id == Convert.ToInt32(i.user_id) select e).FirstOrDefault();
                        messages.Add(new Message(user, i.text, i.created_at));
                    }
                    ViewBag.messages = messages;
                    ViewBag.problem = prob;
                    return View();
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }

    }
}