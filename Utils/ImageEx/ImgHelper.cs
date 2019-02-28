using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class ImgHelper
    {
        /// <summary>
        /// 裁剪指定大小的图片
        /// </summary>
        /// <param name="orginalImgPath"></param>
        /// <param name="targetImgPath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static bool CutImg(string orginalImgPath, string targetImgPath, int width, int height)
        {
            if (string.IsNullOrEmpty(orginalImgPath))
            {
                return false;
            }

            if (string.IsNullOrEmpty(targetImgPath))
            {
                return false;
            }
            bool isfixed = false;
            Bitmap orginalImg, targetImg;
            try
            {
                orginalImg = new Bitmap(orginalImgPath);
                int orginalWidth = Convert.ToInt32(orginalImg.Width);
                int orginalHeight = Convert.ToInt32(orginalImg.Height);
                int targetWidth = 0, targetHeight = 0;
                if (width == 0 && height == 0)
                {
                    targetWidth = orginalWidth;
                    targetHeight = orginalHeight;
                    isfixed = true;
                }
                else if (width == 0 && height > 0)
                {
                    targetWidth = Convert.ToInt32(((double)height / orginalHeight) * orginalWidth);
                    targetHeight = height;
                }
                else if (height == 0 && width > 0)
                {
                    targetWidth = width;
                    targetHeight = Convert.ToInt32(((double)width / orginalWidth) * orginalHeight);
                }
                else
                {
                    targetWidth = width;
                    targetHeight = height;
                    isfixed = true;
                }

                if (!isfixed)
                {
                    if (orginalWidth <= targetWidth && orginalHeight <= targetHeight)
                    {
                        orginalImg.Dispose();
                        if (orginalImgPath != targetImgPath)
                        {
                            var fullDir = Path.GetDirectoryName(targetImgPath);
                            Directory.CreateDirectory(fullDir);//创建存储图片的目录;
                            File.Copy(orginalImgPath, targetImgPath, true);
                        }
                        return true;
                    }
                    else
                    {
                        targetImg = new System.Drawing.Bitmap(orginalImg, targetWidth, targetHeight);
                    }
                }
                else
                {
                    targetImg = new Bitmap(orginalImg, targetWidth, targetHeight);
                }

                orginalImg.Dispose();
                var imgExtensionName = Path.GetExtension(orginalImgPath);
                switch (imgExtensionName)
                {
                    case ".jpg":
                        ImageCodecInfo ici = GetEncoderInfo("image/jpeg");
                        if (ici != null)
                        {
                            EncoderParameters ep = new EncoderParameters(1);
                            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);

                            targetImg.Save(targetImgPath, ici, ep);
                            ep.Dispose();
                            ep = null;
                            ici = null;
                        }
                        else
                        {
                            targetImg.Save(targetImgPath, ImageFormat.Jpeg);
                        }
                        break;
                    case ".bmp":
                        targetImg.Save(targetImgPath, ImageFormat.Bmp);
                        break;
                    case ".gif":
                        targetImg.Save(targetImgPath, ImageFormat.Gif);
                        break;
                    case ".png":
                        targetImg.Save(targetImgPath, ImageFormat.Png);
                        break;
                    default:
                        targetImg.Save(targetImgPath, ImageFormat.Jpeg);
                        break;
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                orginalImg = null;
                targetImg = null;
            }
        }



        public static Stream CutImgToStream(string orginalImgPath, int width, int height)
        {
            if (string.IsNullOrEmpty(orginalImgPath))
            {
                return null;
            }

            var isfixed = false;  
            try
            {
                using (Bitmap orginalImg = new Bitmap(orginalImgPath))
                {
                    int orginalWidth = Convert.ToInt32(orginalImg.Width);
                    int orginalHeight = Convert.ToInt32(orginalImg.Height);
                    int targetWidth = 0, targetHeight = 0;
                    var imgExtensionName = Path.GetExtension(orginalImgPath);
                    if (width == 0 && height == 0)
                    {
                        targetWidth = orginalWidth;
                        targetHeight = orginalHeight;
                        isfixed = true;
                    }
                    else if (width == 0 && height > 0)
                    {
                        targetWidth = Convert.ToInt32(((double)height / orginalHeight) * orginalWidth);
                        targetHeight = height;
                    }
                    else if (height == 0 && width > 0)
                    {
                        targetWidth = width;
                        targetHeight = Convert.ToInt32(((double)width / orginalWidth) * orginalHeight);
                    }
                    else
                    {
                        targetWidth = width;
                        targetHeight = height;
                        isfixed = true;
                    }




                    if (!isfixed)
                    {
                        if (orginalWidth <= targetWidth && orginalHeight <= targetHeight)
                        {

                            targetHeight = orginalWidth;
                            targetWidth = orginalWidth;
                        }
                    }

                    ImageFormat fmt = null;
                    switch (imgExtensionName)
                    {
                        case ".png":
                            fmt = ImageFormat.Png;
                            break;
                        case ".jpg":
                        case ".jpeg":
                            fmt = ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            fmt = ImageFormat.Bmp;
                            break;
                    }

                    var watermarkedStream = new MemoryStream();
                    orginalImg.Save(watermarkedStream, fmt);
                    return watermarkedStream;
                }
            }
            catch (Exception)
            {
                return null;
            }



        }

        public static Image CutImg(string orginalImgPath, int width, int height)
        {
            if (string.IsNullOrEmpty(orginalImgPath))
            {
                return null;
            }


            return Image.FromStream(CutImgToStream(orginalImgPath, width, height));
        }

        //获取图像编码器
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            if (string.IsNullOrEmpty(mimeType))
            {
                return null;
            }

            int j;
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
}
