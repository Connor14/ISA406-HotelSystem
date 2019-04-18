using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HotelSystem.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelSystem.Pages.Hotel.Reservation
{
    public class CreateReservationModel : PageModel
    {
        public Models.Hotel Hotel;

        public void OnGet()
        {
            int hotelId = HttpContext.Session.GetInt32(Constants.CurrentHotel).Value;
            Hotel = new Models.Hotel() { HotelID = 1, Name = "Demo Name 1", State = "Ohio", City = "Oxford", Street = "5262 Brown Road", ZipCode = 45056 };
        }

        public IActionResult OnPost()
        {
            return Redirect("/Hotel/Reservation/ReservationConfirmation");

            return null;
        }
    }
}