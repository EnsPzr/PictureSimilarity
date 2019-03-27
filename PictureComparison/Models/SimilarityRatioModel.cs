using System;
using System.Collections.Generic;
using System.Text;

namespace PictureComparison.Models
{
    public class SimilarityRatioModel
    {
        public String Img1 { get; set; }

        public String Img2 { get; set; }

        public double SimilarityRatio { get; set; }
    }
}
