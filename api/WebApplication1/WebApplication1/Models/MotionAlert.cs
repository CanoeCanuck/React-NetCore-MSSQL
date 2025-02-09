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

        public string  Message { get; set; }
        public string ImageURL { get; set; }
    }
}
