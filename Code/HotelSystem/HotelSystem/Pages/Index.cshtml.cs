using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using HotelSystem.Models;
using HotelSystem.Application;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace HotelSystem.Pages
{
    public class IndexModel : PageModel
    {
        private IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public List<Models.Hotel> Hotels = new List<Models.Hotel>();
        public void OnGet()
        {
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("Default")))
            {
                Hotels = connection.Query<Models.Hotel>("SELECT * FROM Hotel;").ToList<Models.Hotel>();
            }
        }

        public IActionResult OnPostSelectHotel()
        {
            HttpContext.Session.SetInt32(Constants.CurrentHotel, int.Parse(Request.Form["hotelId"]));

            Debug.WriteLine(HttpContext.Session.GetInt32(Constants.CurrentHotel));

            return Redirect("/Hotel/HotelMain");
        }
    }
}
