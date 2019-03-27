using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using PictureComparison.Models;

namespace PictureComparison
{
    public static class PictureSimilarity
    {

        //ana resim, diğer resimler ve benzerlik oranı alıp geriye benzerlik oranının üstündeki resimlerin yollarını döndüren metot
        public static List<string> ComparePictures(string mainPicturePath,
            IEnumerable<string> otherPicturePaths, double minSimilarityDifference)
        {
            try
            {
                var picturePaths = otherPicturePaths as string[] ?? otherPicturePaths.ToArray();
                if (!string.IsNullOrEmpty(mainPicturePath) || picturePaths.Length != 0)
                {
                    if (minSimilarityDifference > 0 && minSimilarityDifference <= 100)
                    {
                        var result = new List<string>();
                        var twoColorMainPicture = PictureConverted(mainPicturePath, null, null);
                        foreach (var otherPicturePath in picturePaths)
                        {
                            var twoColorOtherPicture = PictureConverted(otherPicturePath, twoColorMainPicture.Width, twoColorMainPicture.Height);
                            var equalBit = CompareBits(twoColorMainPicture, twoColorOtherPicture);
                            var sumBit = twoColorMainPicture.Height * twoColorMainPicture.Width;
                            if (((double)equalBit / (double)sumBit) * 100 >= minSimilarityDifference)
                            {
                                result.Add(otherPicturePath);
                            }
                        }

                        return result;
                    }

                    throw new IndexOutOfRangeException("Minimum Similarity Difference must be between 0 and 100!");
                }

                throw new ArgumentNullException("Parameters cannot be empty!");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //ana resim ve diğer resimleri alıp benzerlik oranlarını yüzde olarak dictionaryde döndüren metot
        public static Dictionary<string, double> ComparePictures(string mainPicturePath,
            IEnumerable<string> otherPicturePaths)
        {
            try
            {
                var picturePaths = otherPicturePaths as string[] ?? otherPicturePaths.ToArray();
                if (!string.IsNullOrEmpty(mainPicturePath) || picturePaths.Length != 0)
                {
                    var result = new Dictionary<string, double>();
                    var twoColorMainPicture = PictureConverted(mainPicturePath, null, null);
                    foreach (var otherPicturePath in picturePaths)
                    {
                        var twoColorOtherPicture = PictureConverted(otherPicturePath, twoColorMainPicture.Width, twoColorMainPicture.Height);
                        var equalBit = CompareBits(twoColorMainPicture, twoColorOtherPicture);
                        var sumBit = twoColorMainPicture.Height * twoColorMainPicture.Width;
                        result.Add(otherPicturePath, ((double)equalBit / (double)sumBit) * 100);
                    }
                    return result;
                }

                throw new ArgumentNullException("Parameters cannot be empty!");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //tüm resimlerin yollarını alıp hangi resmin hangi resme % kaç benzer olduğunu döndüren metot
        public static List<SimilarityRatioModel> ComparePictures(IEnumerable<string> picturesPaths)
        {
            try
            {
                var otherPictures = picturesPaths as string[] ?? picturesPaths.ToArray();
                if (otherPictures.Length != 0 && otherPictures.Length > 1)
                {
                    var result = new List<SimilarityRatioModel>();
                    foreach (var mainPic in otherPictures)
                    {
                        var twoColorMainPicture = PictureConverted(mainPic, null, null);
                        var sumBit = twoColorMainPicture.Width * twoColorMainPicture.Height;
                        foreach (var otherPic in otherPictures)
                        {
                            if (!mainPic.Equals(otherPic))
                            {
                                if (result.FirstOrDefault(p => p.Img1.Equals(otherPic) && p.Img2.Equals(mainPic)) == null)
                                {
                                    var twoColorOtherPicture = PictureConverted(otherPic, twoColorMainPicture.Width, twoColorMainPicture.Height);
                                    var equalBit = CompareBits(twoColorMainPicture, twoColorOtherPicture);
                                    result.Add(new SimilarityRatioModel()
                                    {
                                        Img1 = mainPic,
                                        Img2 = otherPic,
                                        SimilarityRatio = ((double)equalBit / (double)sumBit) * 100
                                    });
                                    twoColorOtherPicture.Dispose();
                                }
                            }
                        }
                        twoColorMainPicture.Dispose();
                    }

                    return result;
                }
                throw new Exception("There should be at least two images in the picture paths.");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //resim listesi ve min benzerlik oranı alarak benzer olanları string dizi içerisinde döndüren metot.
        public static List<string[]> ComparePictures(IEnumerable<string> picturesPaths, double minSimilarityDifference)
        {
            try
            {
                var result = new List<string[]>();
                foreach (var mainpic in picturesPaths)
                {
                    var eklenmismi = false;
                    foreach (var benz in result)
                    {
                        if (benz.Contains(mainpic))
                        {
                            eklenmismi = true;
                            break;
                        }
                    }
                    if (eklenmismi)
                        continue;

                    var digerleri = picturesPaths.Where(p => p != mainpic).ToArray();
                    var benzerler = ComparePictures(mainpic, digerleri, minSimilarityDifference).ToList();
                    if (!benzerler.Any())
                        continue;

                    benzerler.Insert(0, mainpic);

                    result.Add(benzerler.ToArray());

                }

                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }

        private static Bitmap ConvertTwoColorPicture(Bitmap translated)
        {
            var twoColorPicture = new Bitmap(translated.Width, translated.Height);
            for (int i = 0; i < translated.Width; i++)
            {
                for (int j = 0; j < translated.Height; j++)
                {
                    var px = translated.GetPixel(i, j);
                    var grayScale = (int)((px.R * 0.3) + (px.G * 0.59) + (px.B * 0.11));
                    var nc = Color.FromArgb(px.A, grayScale, grayScale, grayScale);
                    if (nc.A < 125 && nc.R < 125 && nc.G < 125 && nc.B < 125)
                    {
                        twoColorPicture.SetPixel(i, j, Color.FromArgb(0, 0, 0, 0));
                    }
                    else
                    {
                        twoColorPicture.SetPixel(i, j, Color.FromArgb(255, 255, 255, 255));
                    }
                }
            }

            return twoColorPicture;
        }

        private static int CompareBits(Bitmap img1, Bitmap img2)
        {
            var equalBit = 0;
            for (var i = 0; i < img1.Width; i++)
            {
                for (var j = 0; j < img1.Height; j++)
                {
                    if (img1.GetPixel(i, j).Equals(img2.GetPixel(i, j)))
                    {
                        equalBit++;
                    }
                }
            }

            return equalBit;
        }

        private static Bitmap PictureConverted(string path, int? width, int? height)
        {
            var picture = new Bitmap(path);
            Bitmap pictureResized;
            if (width == null && height == null)
            {
                pictureResized = ResizeImage(picture, picture.Width, picture.Height);
            }
            else
            {
                pictureResized = ResizeImage(picture, width.Value, height.Value);
            }
            var twoColorPicture = ConvertTwoColorPicture(pictureResized);
            return twoColorPicture;
        }
    }
}
