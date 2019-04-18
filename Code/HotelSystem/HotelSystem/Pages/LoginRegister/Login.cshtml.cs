using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using HotelSystem.Models;
using HotelSystem.Application;

namespace HotelSystem.Pages.LoginRegister
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {

        }

        // for some reason I needed IActionResult
        public IActionResult OnPost()
        {
            string email = Request.Form["email"];
            string password = Request.Form["password"];

            if(email == "connor@connor.com" && password == "asdf")
            {
                HttpContext.Session.SetString(Constants.UserEmail, email);
                return Redirect("/Index");
            }

            return null;
        }
    }
}