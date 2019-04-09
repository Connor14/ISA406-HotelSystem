using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using HotelSystem.Models;

namespace HotelSystem.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            if (HttpContext.Session.GetString(Constants.UserEmail) == null)
            {
                Response.Redirect("/LoginRegister/Login");
            }
        }
    }
}
