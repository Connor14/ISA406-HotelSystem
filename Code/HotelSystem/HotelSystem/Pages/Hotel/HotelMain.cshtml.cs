﻿using System;
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

        public int RoleId { get; set; }
        public Models.Hotel Hotel;
        public IEnumerable<dynamic> MyReservations;

        public HotelMainModel(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public void OnGet()
        {
            RoleId = HttpContext.Session.GetInt32(Constants.UserRoleId).Value;
            int hotelId = HttpContext.Session.GetInt32(Constants.CurrentHotel).Value;
            int userId = HttpContext.Session.GetInt32(Constants.UserId).Value;
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("Default")))
            {
                Hotel = connection.QuerySingleOrDefault<Models.Hotel>("SELECT * FROM Hotel where HotelID = @HotelId;", new { HotelId = hotelId });
                MyReservations = connection.Query(@"
                    SELECT * 
                    FROM Reservation res, Room r 
                    WHERE r.Hotel_HotelID = @HotelId AND res.Room_RoomID = r.RoomID AND res.User_UserID = @UserID 
                    ORDER BY res.StartDate ASC", new { HotelId = hotelId, UserId = userId });
            }
        }

        public void OnPost()
        {
            string action = Request.Form["actionButton"];

            string resId = Request.Form["reservationId"];
            int reservationId = int.Parse(resId);

            // cancel button pressed
            if (action == "cancel")
            {
                using (var connection = new MySqlConnection(_configuration.GetConnectionString("Default")))
                {
                    int result = connection.Execute(@"
                    DELETE 
                    FROM Reservation
                    WHERE ReservationID = @ReservationID", new { ReservationID = reservationId });
                }
            }

            OnGet();  // run OnGet again so we have reservation and HOtel data
        }
    }
}