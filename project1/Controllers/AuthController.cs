﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace project1.Controllers
{
    public class AuthController : Controller
    //   private readonly FoodContext _db;
    //public AuthController(FoodContext db)
    //{
    //    _db = db;
    //}
    //{
    //    public IActionResult Signup()
    //    {
    //        return View();
    //    }
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public IActionResult Signup(User user)
    //    {
    //        var checkExistingUser = _db.Users.FirstOrDefault(o => o.Email == user.Email);
    //        if (checkExistingUser != null)
    //        {
    //            ViewBag.msg = "User already registered";
    //            return View();
    //        }

    //        var hasher = new PasswordHasher<string>();
    //        user.Password = hasher.HashPassword(user.Email, user.Password);
    //        _db.Users.Add(user);
    //        _db.SaveChanges();
    //        return RedirectToAction("Login");
    //    }
    { 
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string pass)
        {

            bool IsAuthenticated = false;
            bool IsAdmin = false;
            ClaimsIdentity identity = null;

            if (email == "admin@gmail.com" && pass == "123")
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name ,"Rabia"),
                    new Claim(ClaimTypes.Role ,"Admin"),
                }
               , CookieAuthenticationDefaults.AuthenticationScheme);
                IsAuthenticated = true;
                IsAdmin = true;
            }
            else if (email == "user@gmail.com" && pass == "123")
            {
                IsAuthenticated = true;
                identity = new ClaimsIdentity(new[]
               {
                    new Claim(ClaimTypes.Name ,"User1"),
                    new Claim(ClaimTypes.Role ,"User"),
                }
               , CookieAuthenticationDefaults.AuthenticationScheme);
            }
            else
            {
                IsAuthenticated = false;
                ViewBag.msg = "Invalid Credentials";

            }
            if (IsAuthenticated && IsAdmin)
            {
                var principal = new ClaimsPrincipal(identity);

                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index1", "Admin");
            }
            else if (IsAuthenticated)
            {
                var principal = new ClaimsPrincipal(identity);

                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                IsAuthenticated = false;

            }
            if (IsAuthenticated)
            {
                var principal = new ClaimsPrincipal(identity);

                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }
            else
            {

                return View();
            }


        }
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


    }

}
