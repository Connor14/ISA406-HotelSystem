using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HotelSystem.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelSystem.Pages
{
    public class AllHotelsModel : PageModel
    {
        public List<Models.Hotel> Hotels = new List<Models.Hotel>()
        {
            new Models.Hotel() { HotelID = 1, Name = "Demo Name 1", State = "Ohio", City = "Oxford", Street = "5262 Brown Road", ZipCode = 45056 },
            new Models.Hotel() { HotelID = 2, Name = "Demo Name 2", State = "Ohio", City = "Oxford", Street = "5262 Brown Road", ZipCode = 45056 },
            new Models.Hotel() { HotelID = 3, Name = "Demo Name 3", State = "Ohio", City = "Oxford", Street = "5262 Brown Road", ZipCode = 45056 },
            new Models.Hotel() { HotelID = 4, Name = "Demo Name 4", State = "Ohio", City = "Oxford", Street = "5262 Brown Road", ZipCode = 45056 },
            new Models.Hotel() { HotelID = 5, Name = "Demo Name 5", State = "Ohio", City = "Oxford", Street = "5262 Brown Road", ZipCode = 45056 },
            new Models.Hotel() { HotelID = 6, Name = "Demo Name 6", State = "Ohio", City = "Oxford", Street = "5262 Brown Road", ZipCode = 45056 },
            new Models.Hotel() { HotelID = 7, Name = "Demo Name 7", State = "Ohio", City = "Oxford", Street = "5262 Brown Road", ZipCode = 45056 },
            new Models.Hotel() { HotelID = 8, Name = "Demo Name 8", State = "Ohio", City = "Oxford", Street = "5262 Brown Road", ZipCode = 45056 },
            new Models.Hotel() { HotelID = 9, Name = "Demo Name 9", State = "Ohio", City = "Oxford", Street = "5262 Brown Road", ZipCode = 45056 },
            new Models.Hotel() { HotelID = 10, Name = "Demo Name 10", State = "Ohio", City = "Oxford", Street = "5262 Brown Road", ZipCode = 45056 }
        };

        public void OnGet()
        {
            // select * from hotels
        }

        public IActionResult OnPostSelectHotel()
        {
            HttpContext.Session.SetInt32(Constants.CurrentHotel, int.Parse(Request.Form["hotelId"]));

            Debug.WriteLine(HttpContext.Session.GetInt32(Constants.CurrentHotel));

            return Redirect("/Hotel/HotelMain");
        }
    }
}