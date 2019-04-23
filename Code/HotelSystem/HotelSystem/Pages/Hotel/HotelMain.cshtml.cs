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

namespace HotelSystem.Pages.Hotel
{
    public class HotelMainModel : PageModel
    {

        private IConfiguration _configuration;

        public HotelMainModel(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public Models.Hotel Hotel;

        public void OnGet()
        {
            int hotelId = HttpContext.Session.GetInt32(Constants.CurrentHotel).Value;
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("Default")))
            {
                Hotel = connection.QuerySingleOrDefault<Models.Hotel>("SELECT * FROM Hotel where HotelID = @HotelId;", new { HotelId = hotelId });
            }
        }
    }
}