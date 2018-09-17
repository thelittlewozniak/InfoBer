using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class MessageController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private Context _context;
        public MessageController(Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string id)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var u = (from e in _context.Users where e.Username == HttpContext.Session.GetString("username") select e).FirstOrDefault();
                var prob = (from e in _context.Problems where e.IdChat == id select e).Include(e=>e.Taker).Include(e=>e.Writer).FirstOrDefault();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", u.Token);
                var response1 = await client.GetAsync("https://us1.pusherplatform.io/services/chatkit/v1/479278d3-1b27-4b2c-9bfa-eca3af70b5f1/rooms/" + id + "/messages?initial_id=1&direction=newer");
                var responseString1 = await response1.Content.ReadAsStringAsync();
                var r = JsonConvert.DeserializeObject<List<MessageFromChat>>(responseString1);
                List<Message> messages = new List<Message>();
                foreach (var i in r)
                {
                    var user = (from e in _context.Users where e.Id == Convert.ToInt32(i.user_id) select e).FirstOrDefault();
                    messages.Add(new Message(user, i.text, i.created_at));
                }
                ViewBag.messages = messages;
                ViewBag.id = id;
                ViewBag.username = HttpContext.Session.GetString("username");
                ViewBag.problem = prob;
                return View();
            }
            else
                return View();
        }
        [HttpPost]
        public async Task<IActionResult> Send(string text, string id)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var u = (from e in _context.Users where e.Username == HttpContext.Session.GetString("username") select e).FirstOrDefault();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", u.Token);
                string json = JsonConvert.SerializeObject(new { text = text });
                var response1 = await client.PostAsync("https://us1.pusherplatform.io/services/chatkit/v1/479278d3-1b27-4b2c-9bfa-eca3af70b5f1/rooms/" + id + "/messages", new StringContent(json, Encoding.UTF8, "application/json"));
                return RedirectToAction("Index", "Message", new { id });
            }
            else
                return RedirectToAction("Index", "User");
        }
    }
}