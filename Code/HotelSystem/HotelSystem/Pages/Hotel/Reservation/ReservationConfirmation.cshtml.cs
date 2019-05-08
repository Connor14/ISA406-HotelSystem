using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;


namespace HotelSystem.Pages.Hotel.Reservation
{
    public class ReservationConfirmationModel : PageModel
    {
        public void OnGet()
        {
            String startDate = HttpContext.Session.GetString("reservationStart");
            String endDate = HttpContext.Session.GetString("reservationEnd");
            int cost = HttpContext.Session.GetInt32("reservationCost").Value;
        }
    }
}