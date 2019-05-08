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
using HotelSystem.Models;

namespace HotelSystem.Pages.Hotel.Reservation
{
    public class CreateReservationModel : PageModel
    {
        public Models.Hotel Hotel;
        public IEnumerable<Room> Rooms;
        private IConfiguration _configuration;

        public CreateReservationModel(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public void OnGet()
        {
            
            int hotelId = HttpContext.Session.GetInt32(Constants.CurrentHotel).Value;
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("Default")))
            {
                Hotel = connection.QuerySingleOrDefault<Models.Hotel>("SELECT * FROM Hotel where HotelID = @HotelId;", new { HotelId = hotelId });

                Rooms = connection.Query<Models.Room>("SELECT DISTINCT BedSize, BedCount, Description FROM Room where Hotel_HotelID = @HotelId;", new { HotelId = hotelId });
            }
        }

        public IActionResult OnPost()
        {
            int hotelId = HttpContext.Session.GetInt32(Constants.CurrentHotel).Value;
            int userId = HttpContext.Session.GetInt32(Constants.UserId).Value;

            //so here what we have to do is check if the room is available. So, I'm just going to 
            //grab all the available rooms on that date range. 
            string desc = Request.Form["roomType"]; 
            string startDate = Request.Form["startDate"];
            string endDate = Request.Form["endDate"];
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("Default")))
            {
                var roomsOfType = connection.Query<Models.Room>(@"
                    SELECT * 
                    FROM Room
                    WHERE Room.Hotel_HotelID = @HotelID AND Room.Description = @Description;",
                    new { HotelID = hotelId, Description = desc });

                Debug.WriteLine(roomsOfType.First());

                var Rooms = connection.Query<int, int, Tuple<int, int>>(@"
                    SELECT Reservation.ReservationID, Room.RoomID
                    FROM Reservation
                    JOIN Room ON Reservation.Room_RoomID = Room.RoomID 
                    WHERE Room.Hotel_HotelID = @HotelID AND Room.Description = @Description AND ((Reservation.StartDate <= @StartDate AND @StartDate <= Reservation.EndDate) OR (Reservation.StartDate <= @EndDate AND @EndDate <= Reservation.EndDate));",
                    Tuple.Create,
                    new { HotelID = hotelId, Description = desc, StartDate = startDate, EndDate = endDate },
                    splitOn: "*");

                if(Rooms.Count() <= roomsOfType.Count())
                {
                    // the number of reservations for this room is less than the number of roooms available
                    // YAYAYAYAYAYAYAY
                    Debug.WriteLine("YAYYAYAYAYAYAYAYYAYAYYAH");

                    // find the rooms that are actually available
                    List<Models.Room> rooms = new List<Models.Room>();
                    foreach(Models.Room room in roomsOfType)
                    {
                        bool roomAvailable = true;
                        foreach (Tuple<int, int> tuple in Rooms)
                        {
                            if(tuple.Item1 == room.RoomID)
                            {
                                roomAvailable = false;
                            }
                        }

                        if (roomAvailable)
                        {
                            rooms.Add(room);
                        }
                    }

                    // find the cost of that room and multiply by the length of stay
                    var assignedRoom = rooms.FirstOrDefault();

                    if(assignedRoom != null)
                    {
                        TimeSpan duration = DateTime.Parse(endDate) - DateTime.Parse(startDate);

                        connection.Execute("INSERT INTO Reservation (StartDate, EndDate, Cost, User_UserID, Room_RoomID) VALUES (@StartDate, @EndDate, @Cost, @UserID, @RoomID);",
                        new { StartDate = startDate, EndDate = endDate, Cost = assignedRoom.Price * Math.Ceiling(duration.TotalDays), UserID = userId , RoomID = assignedRoom.RoomID });
                        double cost = assignedRoom.Price * Math.Ceiling(duration.TotalDays);

                        HttpContext.Session.SetInt32("reservationCost", (int) cost);
                        HttpContext.Session.SetString("reservationStart", startDate);
                        HttpContext.Session.SetString("reservationEnd", endDate);
                        return Redirect("/Hotel/Reservation/ReservationConfirmation");
                    }
                    else
                    {
                    return Redirect("/Hotel/Reservation/NoneAvailable");
                        return null;
                    }
                }
                else
                {
                    // we have a conflict
                    return null;
                }
            }

            //return null;
        }
    }
}