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
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace HotelSystem.Pages.LoginRegister
{
    public class LoginModel : PageModel
    {
        private IConfiguration _configuration;

        public LoginModel(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public void OnGet()
        {

        }

        // for some reason I needed IActionResult
        public IActionResult OnPost()
        {
            string email = Request.Form["email"];
            string password = Request.Form["password"];

            if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)){
                return null;
            }

            User foundUser = null;
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("Default")))
            {
                foundUser = connection.QuerySingleOrDefault<User>("SELECT * FROM User WHERE EmailAddress = @EmailAddress AND Password = @Password;", new { EmailAddress = email, Password = password });
            }

            if(foundUser != null)
            {
                HttpContext.Session.SetString(Constants.UserEmail, foundUser.EmailAddress);
                HttpContext.Session.SetInt32(Constants.UserId, foundUser.UserID);
                HttpContext.Session.SetInt32(Constants.UserRoleId, foundUser.RoleID_RoleID);

                return Redirect("/Index");
            }

            return null;
        }
    }
}