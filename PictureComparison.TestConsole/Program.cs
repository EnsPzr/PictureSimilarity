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
                @"C:/resim4.png",
                @"C:/resim5.png",
                @"C:/resim6.png",
                @"C:/resim7.png"
            };
            //1. Parameter => way of main picture
            //2. Parameter => ways of other pictures
            //3. Parameter=> Minimum Similarity Difference
            //backwards Returns the path of those that provide only the similarity rate of type <string>
            var result = PictureSimilarity.ComparePictures(mainPicture, otherPictures, 90);

            //1. Parameter => way of main picture
            //2. Parameter => ways of other pictures
            //Returns the paths and similarity ratios of the Dictionary <string, double>
            var result2 = PictureSimilarity.ComparePictures(mainPicture, otherPictures);

            //Parameter => Ways of all pictures
            //The image based on the backward img1 parameter returns the percentage of the similarity
            //rate in the picture and SimilarityRatio parameter compared to img1 in the img2 parameter.
            var result3 = PictureSimilarity.ComparePictures(otherPictures);

            var result4 = PictureSimilarity.ComparePictures(otherPictures, 90);
        }
    }
}
