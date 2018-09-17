using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace InfoBer.Controllers
{
    public class UserController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private Context _context;
        public UserController(Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("username") == null)
                return View();
            else
                return RedirectToAction("Profile", "User");
        }
        [HttpPost]
        public IActionResult Index(string username,string password)
        {
            if(HttpContext.Session.GetString("username")==null)
            {
                if (username == (from a in _context.Users where a.Username == username select a).FirstOrDefault()?.Username)
                {
                    byte[] pass = System.Text.Encoding.ASCII.GetBytes(password);
                    pass = new System.Security.Cryptography.SHA256Managed().ComputeHash(pass);
                    password = System.Text.Encoding.ASCII.GetString(pass);
                    if (password == (from a in _context.Users where a.Username == username select a).FirstOrDefault()?.Password)
                    {
                        var u = (from a in _context.Users where a.Username == username select a).FirstOrDefault();
                        HttpContext.Session.SetString("username", username);
                        HttpContext.Session.SetString("Admin",Convert.ToString(u.Admin));
                        string key = "igKLnN2Rd6ygBLKNzI5VjPBVFUBdgVDw687QQFZS+1k=";
                        var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                        var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                        var header = new JwtHeader(credentials);
                        var payload = new JwtPayload
                       {
                           { "instance", "479278d3-1b27-4b2c-9bfa-eca3af70b5f1"},
                           { "iss", "api_keys/d5b79396-8f8d-4292-a3fa-5d0a5dfeade1"},
                           { "iat", (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds},
                           { "exp", (Int32)(DateTime.UtcNow.Subtract(new DateTime(1969, 12, 31,23,30,00))).TotalSeconds},
                           { "sub", Convert.ToString((from e in _context.Users where e.Username==username select e).FirstOrDefault().Id)},
                           { "su", true},
                       };
                        var secToken = new JwtSecurityToken(header, payload);
                        var handler = new JwtSecurityTokenHandler();
                        var tokenString = handler.WriteToken(secToken);
                        _context.Users.SingleOrDefault(e => e.Username == username).Token = tokenString;
                        _context.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                }
                return View();
            }
            else
            {
                return RedirectToAction("Profile", "User");
            }
        }
        public IActionResult Create()
        {
            ViewBag.schools = (from e in _context.Schools select e).ToList();
            ViewBag.departements = (from e in _context.Departements select e).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string username,string password,string name,string surname,string Departement,string School)
        {
            if (username == (from a in _context.Users where a.Username == username select a).FirstOrDefault()?.Username)
            {
                ViewBag.error = "an user has already this username.";
                ViewBag.schools = (from e in _context.Schools select e).ToList();
                ViewBag.departements = (from e in _context.Departements select e).ToList();
                return View();
            }
            else
            {
                var u = new User();
                u.Username = username;
                byte[] pass = System.Text.Encoding.ASCII.GetBytes(password);
                pass = new System.Security.Cryptography.SHA256Managed().ComputeHash(pass);
                password = System.Text.Encoding.ASCII.GetString(pass);
                u.Password = password;
                u.Name = name;
                u.Surname = surname;
                u.School = (from a in _context.Schools where a.Name == School select a).FirstOrDefault();
                u.Departement = (from a in _context.Departements where a.Name == Departement && a.School == u.School select a).FirstOrDefault();
                u.Ratings = new List<Rating>();
                u.Admin = false;
                _context.Users.Add(u);
                _context.SaveChanges();
                HttpContext.Session.SetString("username", username);
                HttpContext.Session.SetString("Admin", "false");
                var us = (from e in _context.Users where e.Username == username select e).FirstOrDefault();
                string key = "igKLnN2Rd6ygBLKNzI5VjPBVFUBdgVDw687QQFZS+1k=";
                var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var header = new JwtHeader(credentials);
                var payload = new JwtPayload
               {
                   { "instance", "479278d3-1b27-4b2c-9bfa-eca3af70b5f1"},
                   { "iss", "api_keys/d5b79396-8f8d-4292-a3fa-5d0a5dfeade1"},
                   { "iat", (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds},
                   { "exp", (Int32)(DateTime.UtcNow.Subtract(new DateTime(1969, 12, 31,23,30,00))).TotalSeconds},
                   { "sub", "0"},
                   { "su", true},
               };
                var secToken = new JwtSecurityToken(header, payload);
                var handler = new JwtSecurityTokenHandler();
                var tokenString = handler.WriteToken(secToken);
                _context.Users.SingleOrDefault(e => e.Username == username).Token = tokenString;
                _context.SaveChanges();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                string json=JsonConvert.SerializeObject(new { id = Convert.ToString(us.Id), name = us.Username });
                var response1 = await client.PostAsync("https://us1.pusherplatform.io/services/chatkit/v1/479278d3-1b27-4b2c-9bfa-eca3af70b5f1/users", new StringContent(json, Encoding.UTF8, "application/json"));
                var responseString1 = await response1.Content.ReadAsStringAsync();
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString("username")!=null)
            {
                ViewBag.user = (from a in _context.Users where a.Username == HttpContext.Session.GetString("username") select a).Include(e=>e.Ratings).FirstOrDefault();
                ViewBag.problems = (from a in _context.Problems where a.Writer.Username == HttpContext.Session.GetString("username") select a).Include(e=>e.Taker).ToList();
                return View();
            }
            else
            {
                return View("Index");
            }
        }
        public IActionResult Messages()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.messages = (from a in _context.Problems where (a.Taker.Username == HttpContext.Session.GetString("username") || a.Writer.Username== HttpContext.Session.GetString("username")) && a.Taker!=null && a.status==false select a).Include(r => r.Writer).Include(e => e.Taker).ToList();
                return View();
            }
            else
            {
                return View("Index");
            }
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}