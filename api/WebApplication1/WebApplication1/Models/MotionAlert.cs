using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class MotionAlert
    {
        public int AlertId { get; set; }
        public string camera {get; set; }
        public decimal Confidence { get; set; }

        public string TimeStamp { get; set; }

        public string message { get; set; }
        public string imageUrl { get; set; }
    }
}
