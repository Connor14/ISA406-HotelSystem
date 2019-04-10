using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelSystem.Pages.LoginRegister
{
    public class RegisterModel : PageModel
    {
        public void OnGet()
        {

        }

        public void OnPost()
        {
            string firstName = Request.Form["firstName"];
            string lastName = Request.Form["lastName"];
            string email = Request.Form["email"];
            string phone = Request.Form["phone"];
            string birthday = Request.Form["birthday"];

            string password = Request.Form["password"];
            string passwordConfirm = Request.Form["passwordConfirm"];

            if (string.IsNullOrWhiteSpace(firstName)
                || string.IsNullOrWhiteSpace(lastName)
                || string.IsNullOrWhiteSpace(email) 
                || string.IsNullOrWhiteSpace(phone) 
                || string.IsNullOrWhiteSpace(birthday) 
                || string.IsNullOrWhiteSpace(password) 
                || string.IsNullOrWhiteSpace(passwordConfirm))
            {
                //throw new Exception("Stuff is missing");
                return;
            }

            if(password != passwordConfirm)
            {
                //throw new Exception("Passwords don't match");
                return;
            }

            // todo insert stuff into database.

            Response.Redirect("/LoginRegister/Login");
        }
    }
}