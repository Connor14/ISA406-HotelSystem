using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using HotelSystem.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
namespace HotelSystem.Pages.Loyalty
{
    public class LoyaltyModel : PageModel
    {
        private IConfiguration _configuration;
        public int points;
        public LoyaltyModel(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public void OnGet()
        {
            int userId = HttpContext.Session.GetInt32(Constants.UserId).Value;
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("Default")))
            {
                points = connection.Query<int>("SELECT SUM(COST) FROM Reservation WHERE User_UserID = @userId;", new { userId }).First();
            }
        }
    }
}