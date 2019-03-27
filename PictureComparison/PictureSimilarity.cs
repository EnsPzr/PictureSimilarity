using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

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

                        var equalBit = 0;
                        for (var i = 0; i < twoColorMainPicture.Width; i++)
                        {
                            for (var j = 0; j < twoColorMainPicture.Height; j++)
                            {
                                if (twoColorMainPicture.GetPixel(i, j).Equals(twoColorOtherPicture.GetPixel(i, j)))
                                {
                                    equalBit++;
                                }
                            }
                        }

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

                            var equalBit = 0;
                            for (var i = 0; i < twoColorMainPicture.Width; i++)
                            {
                                for (var j = 0; j < twoColorMainPicture.Height; j++)
                                {
                                    if (twoColorMainPicture.GetPixel(i, j).Equals(twoColorOtherPicture.GetPixel(i, j)))
                                    {
                                        equalBit++;
                                    }
                                }
                            }

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
    }
}
