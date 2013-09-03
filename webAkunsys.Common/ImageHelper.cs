using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace webAkunsys.Common
{
    public class ImageHelper
    {
        public static string MergeImages(HttpServerUtilityBase Server, string backImagePath, string frontImagePath, string facebookId, string type)
        {
            string pathRoute = SiteSettings.MergeImageRoute;

            Image backImage = null;

            //IF USE PROXY
            //WebRequest request = HttpWebRequest.Create(backImagePath);
            //WebProxy proxy = new System.Net.WebProxy(SiteSettings.ProxyServerIP, true);
            //proxy.Credentials = new System.Net.NetworkCredential(SiteSettings.ProxyUserName, SiteSettings.ProxyPass);
            //request.Proxy = proxy;
            //Stream backImageStream = request.GetResponse().GetResponseStream();

            using (Stream backImageStream = HttpWebRequest.Create(backImagePath).GetResponse().GetResponseStream())
            {
                try
                {
                    backImage = Image.FromStream(backImageStream);
                }
                catch (Exception ex)
                {
                    Common.Helper.CommonFunction.LogToFile(ex.InnerException != null ? ex.InnerException.Message : ex.Message, "Stream backImageStream", Server);
                }
                if (type == Constants.USERTYPE_TWITTER || backImage.Width < 100)
                {

                    double aspectRatio = (double)backImage.Width / (double)backImage.Height;
                    int newWidth = 190;
                    int newHeight = (int)Math.Round(newWidth / aspectRatio);
                    backImage = new Bitmap(backImage, newWidth, newHeight);

                }
                Bitmap frontImage = null;
                try
                {
                    frontImage = (Bitmap)Image.FromFile(Server.MapPath(frontImagePath));
                }
                catch (Exception ex)
                {
                    Common.Helper.CommonFunction.LogToFile(ex.InnerException != null ? ex.InnerException.Message : ex.Message, "get frontImage", Server);
                }

                try
                {
                    using (var bitmap = new Bitmap(backImage.Width, backImage.Height))
                    {
                        using (var canvas = Graphics.FromImage(bitmap))
                        {
                            canvas.InterpolationMode = InterpolationMode.HighQualityBilinear;
                            if (type == Constants.USERTYPE_TWITTER)
                            {
                                canvas.DrawImage(backImage, new Rectangle(0, 0, backImage.Width, backImage.Height));//, new Rectangle(0, 0, backImage.Width, backImage.Height), GraphicsUnit.Pixel);
                                canvas.DrawImage(frontImage, new Rectangle(backImage.Width - frontImage.Width, backImage.Height - frontImage.Height, frontImage.Width, frontImage.Height));
                            }
                            else
                            {
                                canvas.DrawImage(backImage, new Rectangle(0, 0, backImage.Width, backImage.Height));//, new Rectangle(0, 0, backImage.Width, backImage.Height), GraphicsUnit.Pixel);
                                canvas.DrawImage(frontImage, new Rectangle(backImage.Width - frontImage.Width, backImage.Height - frontImage.Height, frontImage.Width, frontImage.Height));
                            }
                            canvas.Save();
                        }

                        string fileName = facebookId + ".png";
                        bitmap.Save(Server.MapPath(pathRoute) + fileName, ImageFormat.Png);


                        double aspectRatio = (double)bitmap.Width / (double)bitmap.Height;
                        int newWidth = 24;
                        int newHeight = (int)Math.Round(newWidth / aspectRatio);
                        var miniImage = new Bitmap(bitmap, newWidth, newHeight);

                        miniImage.Save(Server.MapPath(SiteSettings.MergeImageRoute.Replace("avatar", "miniavatar")) + fileName, ImageFormat.Png);


                        backImage.Dispose();
                        frontImage.Dispose();
                        bitmap.Dispose();
                        miniImage.Dispose();

                        return pathRoute + fileName;
                    }
                }
                catch (Exception ex)
                {
                    Common.Helper.CommonFunction.LogToFile(ex.InnerException != null ? ex.InnerException.Message : ex.Message, "Merging Image And Save Image", Server);
                    return string.Empty;
                }

            }
        }



        public static string OpacityImages(HttpServerUtilityBase Server, string backImagePath, List<string> ListfrontImagePath, string facebookId)
        {
            string pathRoute = SiteSettings.MergeImageRoute;
            Image backImage = null;
            int merge = 235;
            using (Stream backImageStream = HttpWebRequest.Create(backImagePath).GetResponse().GetResponseStream())
            {
                try
                {
                    backImage = Image.FromStream(backImageStream);
                }
                catch
                {

                }
                using (var bitmap = new Bitmap(backImage.Width, backImage.Height))
                {
                    using (var canvas = Graphics.FromImage(bitmap))
                    {
                        int maxWidth = 16;


                        canvas.InterpolationMode = InterpolationMode.HighQualityBilinear;
                        canvas.DrawImage(backImage, new Rectangle(0, 0, backImage.Width, backImage.Height), new Rectangle(0, 0, backImage.Width, backImage.Height), GraphicsUnit.Pixel);
                        int y = backImage.Height - maxWidth;
                        int x = merge;
                        int index = 0;
                        Image frontImage = null;
                        string frontImagePath = string.Empty;
                        float opacity = 1;
                        while (y >= 0)
                        {
                            if (index < ListfrontImagePath.Count)
                            {
                                opacity = (float)0.2;
                                frontImagePath = ListfrontImagePath[index];
                                index++;
                            }
                            else
                            {
                                opacity = (float)0.7;
                                frontImagePath = "/Asset/uploads/avatar/Black.jpg";
                            }

                            try
                            {
                                ColorMatrix cmxPic = new ColorMatrix();
                                cmxPic.Matrix33 = opacity;

                                ImageAttributes iaPic = new ImageAttributes();
                                iaPic.SetColorMatrix(cmxPic, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);


                                if (!frontImagePath.Contains("http"))
                                {
                                    frontImage = Image.FromFile(Server.MapPath(".." + frontImagePath));
                                }
                                else
                                {
                                    Stream frontImageStream = HttpWebRequest.Create(frontImagePath).GetResponse().GetResponseStream();
                                    frontImage = Image.FromStream(frontImageStream);
                                }
                                if (frontImage.Width != maxWidth)
                                {
                                    double aspectRatio = (double)frontImage.Width / (double)frontImage.Height;
                                    int newWidth = maxWidth;
                                    int newHeight = (int)Math.Round(newWidth / aspectRatio);
                                    frontImage = new Bitmap(frontImage, newWidth, newHeight);
                                }
                                canvas.DrawImage(frontImage, new Rectangle(x, y, frontImage.Width, frontImage.Height), 0, 0, frontImage.Width, frontImage.Height, GraphicsUnit.Pixel, iaPic);

                                if (x >= backImage.Width - merge)
                                {
                                    merge = merge - maxWidth;
                                    y = y - frontImage.Height;
                                    x = merge;
                                }
                                else
                                {
                                    x = x + frontImage.Width;
                                }
                                frontImage.Dispose();
                            }
                            catch
                            {

                            }

                        }

                        //foreach (string frontImagePath in ListfrontImagePath)
                        //{                           

                        //    Image frontImage = null;                           
                        //    try
                        //    {
                        //        if (!frontImagePath.Contains("http"))
                        //        {
                        //            frontImage = Image.FromFile(Server.MapPath(frontImagePath));
                        //        }
                        //        else
                        //        {
                        //            Stream frontImageStream = HttpWebRequest.Create(frontImagePath).GetResponse().GetResponseStream();
                        //            frontImage = Image.FromStream(frontImageStream);
                        //        }
                        //        if (frontImage.Width != maxWidth)
                        //        {

                        //            double aspectRatio = (double)frontImage.Width / (double)frontImage.Height;
                        //            int newWidth = maxWidth;
                        //            int newHeight = (int)Math.Round(newWidth / aspectRatio);
                        //            frontImage = new Bitmap(frontImage, newWidth, newHeight);
                        //        }
                        //        canvas.DrawImage(frontImage, new Rectangle(x, y, frontImage.Width, frontImage.Height), 0, 0, frontImage.Width, frontImage.Height, GraphicsUnit.Pixel, iaPic);

                        //        if (x >= backImage.Width - merge)
                        //        {
                        //            y = y - frontImage.Height;
                        //            x = merge;
                        //        }
                        //        else
                        //        {

                        //            x = x + frontImage.Width;
                        //        }
                        //        frontImage.Dispose();
                        //    }
                        //    catch
                        //    {

                        //    }                      
                        //}
                        canvas.Save();
                    }
                    string fileName = facebookId + ".png";
                    bitmap.Save(Server.MapPath(pathRoute) + fileName, ImageFormat.Png);
                    backImage.Dispose();
                    bitmap.Dispose();
                    return pathRoute + fileName;
                }
            }
        }

        public static string OpacityImages2(string RootServer, string backImagePath, List<string> ListfrontImagePath, string facebookId)
        {
            string pathRoute = SiteSettings.MergeImageRoute;
            Image backImage = null;
            int merge = 235;
            using (Stream backImageStream = HttpWebRequest.Create(backImagePath).GetResponse().GetResponseStream())
            {
                try
                {
                    backImage = Image.FromStream(backImageStream);
                }
                catch
                {

                }
                using (var bitmap = new Bitmap(backImage.Width, backImage.Height))
                {
                    using (var canvas = Graphics.FromImage(bitmap))
                    {
                        int maxWidth = 16;
                        canvas.InterpolationMode = InterpolationMode.HighQualityBilinear;
                        canvas.DrawImage(backImage, new Rectangle(0, 0, backImage.Width, backImage.Height), new Rectangle(0, 0, backImage.Width, backImage.Height), GraphicsUnit.Pixel);
                        int y = backImage.Height - maxWidth;
                        int x = 0;
                        int index = 0;
                        Image frontImage = null;
                        string frontImagePath = string.Empty;
                        float opacity = 1;
                        while (y >= 0)
                        {
                            if (index < ListfrontImagePath.Count)
                            {
                                opacity = (float)0.2;
                                frontImagePath = ListfrontImagePath[index];
                                index++;
                            }
                            else
                            {
                                // opacity = (float)0.2;
                                //int _count = ListfrontImagePath.Count;
                                //int _index = new Random().Next(_count);
                                //frontImagePath = ListfrontImagePath[_index];
                                index = 0;
                                frontImagePath = ListfrontImagePath[index];
                                index++;
                                //frontImagePath = SiteSettings.FrontImageOverlay;
                            }

                            try
                            {
                                ColorMatrix cmxPic = new ColorMatrix();
                                cmxPic.Matrix33 = opacity;

                                ImageAttributes iaPic = new ImageAttributes();
                                iaPic.SetColorMatrix(cmxPic, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);


                                if (!frontImagePath.Contains("http"))
                                {
                                    frontImage = Image.FromFile(RootServer + frontImagePath);
                                }
                                else
                                {
                                    Stream frontImageStream = HttpWebRequest.Create(frontImagePath).GetResponse().GetResponseStream();
                                    frontImage = Image.FromStream(frontImageStream);
                                }
                                if (frontImage.Width != maxWidth)
                                {
                                    double aspectRatio = (double)frontImage.Width / (double)frontImage.Height;
                                    int newWidth = maxWidth;
                                    int newHeight = maxWidth;// (int)Math.Round(newWidth / aspectRatio);
                                    frontImage = new Bitmap(frontImage, newWidth, newHeight);
                                }
                                canvas.DrawImage(frontImage, new Rectangle(x, y, frontImage.Width, frontImage.Height), 0, 0, frontImage.Width, frontImage.Height, GraphicsUnit.Pixel, iaPic);

                                if (x >= backImage.Width)
                                {
                                    merge = merge - maxWidth;
                                    y = y - frontImage.Height;
                                    x = 0;
                                }
                                else
                                {
                                    x = x + frontImage.Width;
                                }
                                frontImage.Dispose();
                            }
                            catch
                            {

                            }

                        }

                        canvas.Save();
                    }
                    string fileName = facebookId + ".png";
                    bitmap.Save(RootServer + pathRoute + fileName, ImageFormat.Png);
                    backImage.Dispose();
                    bitmap.Dispose();
                    return pathRoute + fileName;
                }
            }
        }
    }
}
