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

namespace HotelSystem.Pages.Hotel.Staff
{
    public class FrontDeskModel : PageModel
    {
        private IConfiguration _configuration;

        public int RoleId { get; set; }
        public Models.Hotel Hotel;
        public IEnumerable<dynamic> MyReservations;

        public FrontDeskModel(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public IActionResult OnGet()
        {
            RoleId = HttpContext.Session.GetInt32(Constants.UserRoleId).Value;

            if(RoleId != 2)
            {
                return new UnauthorizedResult();
            }

            int hotelId = HttpContext.Session.GetInt32(Constants.CurrentHotel).Value;
            int userId = HttpContext.Session.GetInt32(Constants.UserId).Value;
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("Default")))
            {
                Hotel = connection.QuerySingleOrDefault<Models.Hotel>("SELECT * FROM Hotel where HotelID = @HotelId;", new { HotelId = hotelId });
                MyReservations = connection.Query(@"
                    SELECT * 
                    FROM Reservation res, Room r, User u
                    WHERE r.Hotel_HotelID = @HotelId AND res.Room_RoomID = r.RoomID AND res.User_UserID = u.UserID
                    ORDER BY res.StartDate ASC", new { HotelId = hotelId });
            }

            return null;
        }

        public IActionResult OnPost()
        {
            string action = Request.Form["actionButton"];

            string resId = Request.Form["reservationId"];
            int reservationId = int.Parse(resId);

            using (var connection = new MySqlConnection(_configuration.GetConnectionString("Default")))
            {
                // cancel button pressed
                if (action == "cancel")
                {
                    int result = connection.Execute(@"
                    DELETE 
                    FROM Reservation
                    WHERE ReservationID = @ReservationID", new { ReservationID = reservationId });
                }
                else if (action == "checkIn")
                {
                    int result = connection.Execute(@"
                    UPDATE Reservation
                    SET CheckedIn = true
                    WHERE ReservationID = @ReservationID", new { ReservationID = reservationId });
                }
                else if (action == "checkOut")
                {
                    int result = connection.Execute(@"
                    UPDATE Reservation
                    SET CheckedOut = true
                    WHERE ReservationID = @ReservationID", new { ReservationID = reservationId });
                }
            }

            return OnGet();  // run OnGet again so we have reservation and HOtel data
        }
    }
}