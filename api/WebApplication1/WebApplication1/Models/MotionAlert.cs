using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Prediction
    {
        public string Label { get; set; }
        public double Confidence { get; set; }
        public int MinX { get; set; }
        public int MinY { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
    }

    public class Root
    {
        public string camera { get; set; }
        public int AlertId { get; set; }
        public string TimeStamp { get; set; }
        public List<string> foundTypes { get; set; }
        public List<Prediction> predictions { get; set; }
        public string message { get; set; }
        public string imageUrl { get; set; }
    }


}
