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
        public static Dictionary<string, double> ComparePictures(string mainPicturePath,
           IEnumerable<string> otherPicturePaths)
        {
            try
            {
                var picturePaths = otherPicturePaths as string[] ?? otherPicturePaths.ToArray();
                if (!string.IsNullOrEmpty(mainPicturePath) || picturePaths.Length != 0)
                {
                    var result = new Dictionary<string, double>();
                    var mainPicture = new Bitmap(mainPicturePath);
                    var mainPictureResized = ResizeImage(mainPicture, mainPicture.Width, mainPicture.Height);
                    var twoColorMainPicture = ConvertTwoColorPicture(mainPictureResized);
                    foreach (var otherPicturePath in picturePaths)
                    {
                        var otherPicture = new Bitmap(otherPicturePath);
                        var otherPictureResized = ResizeImage(otherPicture, twoColorMainPicture.Width, twoColorMainPicture.Height);
                        var twoColorOtherPicture = ConvertTwoColorPicture(otherPictureResized);
                        var equalBit = CompareBits(twoColorMainPicture, twoColorOtherPicture);
                        var sumBit = twoColorMainPicture.Height * twoColorMainPicture.Width;
                        result.Add(otherPicturePath, ((double)equalBit / (double)sumBit) * 100);
                        twoColorOtherPicture.Dispose();
                        otherPictureResized.Dispose();
                        otherPicture.Dispose();
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
                        var mainPicture = new Bitmap(mainPicturePath);
                        var mainPictureResized = ResizeImage(mainPicture, mainPicture.Width, mainPicture.Height);
                        var twoColorMainPicture = ConvertTwoColorPicture(mainPictureResized);
                        foreach (var otherPicturePath in picturePaths)
                        {
                            var otherPicture = new Bitmap(otherPicturePath);
                            var otherPictureResized = ResizeImage(otherPicture, twoColorMainPicture.Width, twoColorMainPicture.Height);
                            var twoColorOtherPicture = ConvertTwoColorPicture(otherPictureResized);
                            var equalBit = CompareBits(twoColorMainPicture, twoColorOtherPicture);
                            var sumBit = twoColorMainPicture.Height * twoColorMainPicture.Width;
                            if (((double)equalBit / (double)sumBit) * 100 >= minSimilarityDifference)
                            {
                                result.Add(otherPicturePath);
                            }
                            twoColorOtherPicture.Dispose();
                            otherPictureResized.Dispose();
                            otherPicture.Dispose();
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

        public static List<SimilarityRatioModel> ComparePictures(IEnumerable<string> picturesPaths)
        {
            var otherPictures = picturesPaths as string[] ?? picturesPaths.ToArray();
            if (otherPictures.Length != 0 && otherPictures.Length > 1)
            {
                var result = new List<SimilarityRatioModel>();
                foreach (var mainPic in otherPictures)
                {
                    var mainPicture = new Bitmap(mainPic);
                    var mainPictureResized = ResizeImage(mainPicture, mainPicture.Width, mainPicture.Height);
                    var twoColorMainPicture = ConvertTwoColorPicture(mainPictureResized);
                    var sumBit = twoColorMainPicture.Width * twoColorMainPicture.Height;
                    foreach (var otherPic in otherPictures)
                    {
                        if (!mainPic.Equals(otherPic))
                        {
                            if (result.FirstOrDefault(p => p.Img1.Equals(otherPic) && p.Img2.Equals(mainPic)) == null)
                            {
                                var otherPicture = new Bitmap(otherPic);
                                var otherPictureResized = ResizeImage(otherPicture, twoColorMainPicture.Width,
                                    twoColorMainPicture.Height);
                                var twoColorOtherPicture = ConvertTwoColorPicture(otherPictureResized);
                                var equalBit = CompareBits(twoColorMainPicture, twoColorOtherPicture);
                                result.Add(new SimilarityRatioModel()
                                {
                                    Img1 = mainPic,
                                    Img2 = otherPic,
                                    SimilarityRatio = ((double)equalBit / (double)sumBit) * 100
                                });
                                twoColorOtherPicture.Dispose();
                                otherPictureResized.Dispose();
                                otherPicture.Dispose();;
                            }
                        }
                    }
                    twoColorMainPicture.Dispose();
                    mainPictureResized.Dispose();
                    mainPicture.Dispose();
                }

                return result;
            }
            else
            {
                throw new Exception("There should be at least two images in the picture paths.");
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
    }
}
