using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelSystem.Models
{
    public class Reservation
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Cost { get; set; }
        public int User_UserID{get;set;}
        public int Room_RoomID { get; set; }
        public bool CheckedIn { get; set; }
        public bool CheckedOut { get; set; }
    }
}
