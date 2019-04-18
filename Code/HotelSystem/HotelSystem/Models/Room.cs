using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelSystem.Models
{
    public enum RoomType
    {
        SingleKing = 0,
        DoubleQueen
    }

    public class Room
    {
		public int RoomID { get; set; }
		public int RoomNo { get; set; }
		public string BedSize { get; set; }
		public int BedCount { get; set; }
		public int BathroomCount { get; set; }
		public string Description { get; set; }
		public string Price { get; set; }

        public RoomType RoomType { get; set; }
    }
}
