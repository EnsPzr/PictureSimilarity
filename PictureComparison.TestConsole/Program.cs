using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureComparison.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var mainPicture = @"C:/resim1.png";
            var otherPictures = new List<string>
            {
                @"C:/resim2.png",
                @"C:/resim3.png",
                @"C:/resim4.png"
            };
            //1. Parameter => main picture
            //2. Parameter => other picture list
            //3. Parameter=> Minimum Similarity Difference
            var result = PictureSimilarity.ComparePictures(mainPicture, otherPictures, 90);
        }
    }
}
