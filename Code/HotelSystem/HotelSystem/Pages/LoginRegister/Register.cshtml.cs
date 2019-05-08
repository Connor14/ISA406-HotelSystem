using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using HotelSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace HotelSystem.Pages.LoginRegister
{
    public class RegisterModel : PageModel
    {
        private IConfiguration _configuration;

        public RegisterModel(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            string firstName = Request.Form["firstName"];
            string lastName = Request.Form["lastName"];
            string email = Request.Form["email"];
            string phone = Request.Form["phone"];
            string birthday = Request.Form["birthday"];

            string password = Request.Form["password"];
            string passwordConfirm = Request.Form["passwordConfirm"];

            string accountType = Request.Form["accountType"];

            Debug.WriteLine(accountType);

            if (string.IsNullOrWhiteSpace(firstName)
                || string.IsNullOrWhiteSpace(lastName)
                || string.IsNullOrWhiteSpace(email) 
                || string.IsNullOrWhiteSpace(phone) 
                || string.IsNullOrWhiteSpace(birthday) 
                || string.IsNullOrWhiteSpace(password) 
                || string.IsNullOrWhiteSpace(passwordConfirm))
            {
                //throw new Exception("Stuff is missing");
                return null;
            }

            if(password != passwordConfirm)
            {
                //throw new Exception("Passwords don't match");
                return null;
            }

            User newUser = new User(){
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                PhoneNumber = phone,
                DOB = DateTime.Parse(birthday),
                Password = password,
                RoleID_RoleID = int.Parse(accountType)
            };

            int registerResult = -1;
            // todo insert stuff into database.
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("Default")))
            {
                registerResult = connection.Execute("INSERT INTO User (FirstName, LastName, EmailAddress, DOB, Password, RoleID_RoleID) VALUES (@FirstName, @LastName, @EmailAddress, @DOB, @Password, @RoleID_RoleID)", newUser);
            }

            if(registerResult > 0){
                return Redirect("/LoginRegister/Login");
            }

            return null;
        }
    }
}