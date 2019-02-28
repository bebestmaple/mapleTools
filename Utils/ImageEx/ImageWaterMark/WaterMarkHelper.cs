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
   public class WaterMarkHelper
    {
        #region 文字水印

        public static MemoryStream AddWatermark(Image originImage, string imgExtensionName, string watermarkText, Color color, WatermarkPositionEnum watermarkPosition = WatermarkPositionEnum.RightButtom, int textPadding = 10, int fontSize = 20, Font font = null)
        {

            if (originImage == null)
            {
                return null;
            }
            using (var graphic = Graphics.FromImage(originImage))
            {
                if (!string.IsNullOrEmpty(watermarkText))
                {
                    var brush = new SolidBrush(color);

                    if (fontSize < 5)
                    {
                        fontSize = 5;
                    }

                    var f = font ?? new Font(FontFamily.GenericSansSerif, fontSize,
                                FontStyle.Bold, GraphicsUnit.Pixel);

                    var textSize = graphic.MeasureString(watermarkText, f);
                    int x = textPadding, y = textPadding;

                    switch (watermarkPosition)
                    {
                        case WatermarkPositionEnum.LeftTop:
                            x = textPadding; y = textPadding;
                            break;
                        case WatermarkPositionEnum.LeftCenter:
                            x = textPadding;
                            y = (int)((originImage.Height/2.0) - textSize.Height - textPadding);
                            break;
                        case WatermarkPositionEnum.LeftButtom:
                            x = textPadding;
                            y = originImage.Height - (int)textSize.Height - textPadding;
                            break;
                        case WatermarkPositionEnum.CenterTop:
                            x = (int)((originImage.Width/2 )- textSize.Width - textPadding);
                            y = textPadding;
                            break;
                        case WatermarkPositionEnum.Center:
                            x = (int)((originImage.Width / 2) - textSize.Width - textPadding);
                            y = originImage.Height - (int)textSize.Height - textPadding;
                            break;
                        case WatermarkPositionEnum.CenterButtom:
                            x = (int)((originImage.Width / 2) - textSize.Width - textPadding);
                            y = originImage.Height - (int)textSize.Height - textPadding;
                            break;
                        case WatermarkPositionEnum.RightTop:
                            x = originImage.Width - (int)textSize.Width - textPadding;
                            y = textPadding;
                            break;
                        case WatermarkPositionEnum.RightCenter:
                            x = originImage.Width - (int)textSize.Width - textPadding;
                            y = (int)((originImage.Height / 2.0) - textSize.Height - textPadding);
                            break;
                        case WatermarkPositionEnum.RightButtom:
                            x = originImage.Width - (int)textSize.Width - textPadding;
                            y = originImage.Height - (int)textSize.Height - textPadding;
                            break;
                        default:
                            x = textPadding; y = textPadding;
                            break;
                    }

                    graphic.DrawString(watermarkText, f, brush, new Point(x, y));
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
                    default:
                        fmt = ImageFormat.Jpeg;
                        break;

                }
                var watermarkedStream = new MemoryStream();
                originImage.Save(watermarkedStream, fmt);
                return watermarkedStream;
            }

        }

        public static MemoryStream AddWatermark(string originImagePath, string watermarkText, Color color, WatermarkPositionEnum watermarkPosition = WatermarkPositionEnum.RightButtom, int textPadding = 10, int fontSize = 20, Font font = null)
        {
            if (string.IsNullOrEmpty(originImagePath))
            {
                return null;
            }

            using (var img = Image.FromFile(originImagePath))
            {
                var imgExtensionName = "." + originImagePath.Split('.').Last();
                return AddWatermark(img, imgExtensionName, watermarkText, color, watermarkPosition, textPadding, fontSize, font);
            }
        }

        public static MemoryStream AddWatermark(Stream originImageStream, string imgExtensionName, string watermarkText, Color color, WatermarkPositionEnum watermarkPosition = WatermarkPositionEnum.RightButtom, int textPadding = 10, int fontSize = 20, Font font = null)
        {
            if (originImageStream == null)
            {
                return null;
            }
            using (var img = Image.FromStream(originImageStream))
            {
                return AddWatermark(img, imgExtensionName, watermarkText, color, watermarkPosition, textPadding, fontSize, font);
            }
        }
        #endregion

        #region 图片水印
        public static MemoryStream AddWatermark(Image originImage, string imgExtensionName, Image watermarkImg, WatermarkPositionEnum watermarkPosition = WatermarkPositionEnum.RightButtom, int textPadding = 10)
        {
            if (originImage == null)
            {
                return null;
            }
            using (var graphic = Graphics.FromImage(originImage))
            {
                if (watermarkImg != null)
                {
                    var width = (int)(originImage.Width * 0.3);
                    var height = (int)(originImage.Height * 0.3);
                    watermarkImg = ZoomImage(watermarkImg, width, height);
                    int x = textPadding, y = textPadding;
                    switch (watermarkPosition)
                    {
                        case WatermarkPositionEnum.LeftTop:
                            x = textPadding; y = textPadding;
                            break;
                        case WatermarkPositionEnum.LeftCenter:
                            x = textPadding;
                            y = (int)((originImage.Height / 2.0) - watermarkImg.Height - textPadding);
                            break;
                        case WatermarkPositionEnum.LeftButtom:
                            x = textPadding;
                            y = originImage.Height - (int)watermarkImg.Height - textPadding;
                            break;
                        case WatermarkPositionEnum.CenterTop:
                            x = (int)((originImage.Width / 2) - watermarkImg.Width - textPadding);
                            y = textPadding;
                            break;
                        case WatermarkPositionEnum.Center:
                            x = (int)((originImage.Width / 2) - watermarkImg.Width - textPadding);
                            y = originImage.Height - (int)watermarkImg.Height - textPadding;
                            break;
                        case WatermarkPositionEnum.CenterButtom:
                            x = (int)((originImage.Width / 2) - watermarkImg.Width - textPadding);
                            y = originImage.Height - (int)watermarkImg.Height - textPadding;
                            break;
                        case WatermarkPositionEnum.RightTop:
                            x = originImage.Width - (int)watermarkImg.Width - textPadding;
                            y = textPadding;
                            break;
                        case WatermarkPositionEnum.RightCenter:
                            x = originImage.Width - (int)watermarkImg.Width - textPadding;
                            y = (int)((originImage.Height / 2.0) - watermarkImg.Height - textPadding);
                            break;
                        case WatermarkPositionEnum.RightButtom:
                            x = originImage.Width - (int)watermarkImg.Width - textPadding;
                            y = originImage.Height - (int)watermarkImg.Height - textPadding;
                            break;
                        default:
                            x = textPadding; y = textPadding;
                            break;
                    }

                    graphic.DrawImage(watermarkImg, new Point(x, y));
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
                originImage.Save(watermarkedStream, fmt);
                return watermarkedStream;
            }

        }

        public static MemoryStream AddWatermark(string originImagePath, string watermarkImgPath, WatermarkPositionEnum watermarkPosition = WatermarkPositionEnum.RightButtom, int textPadding = 10)
        {
            if (string.IsNullOrEmpty(originImagePath))
            {
                return null;
            }
            using (var originImage = Image.FromFile(originImagePath))
            {
                var imgExtensionName = "." + originImagePath.Split('.').Last();
                if (!string.IsNullOrEmpty(watermarkImgPath))
                {
                    using (var watermarkImg = Image.FromFile(watermarkImgPath))
                    {
                        return AddWatermark(originImage, imgExtensionName, watermarkImg, watermarkPosition, textPadding);
                    }
                }
                else
                {
                    return AddWatermark(originImage, imgExtensionName, null, watermarkPosition, textPadding);
                }
            }

        }




        public static MemoryStream AddWatermark(Stream originImageStream, string imgExtensionName, Stream watermarkImgStream, WatermarkPositionEnum watermarkPosition = WatermarkPositionEnum.RightButtom, int textPadding = 10)
        {
            if (originImageStream == null)
            {
                return null;
            }
            using (var originImage = Image.FromStream(originImageStream))
            {

                if (watermarkImgStream != null)
                {
                    using (var watermarkImg = Image.FromStream(watermarkImgStream))
                    {
                        return AddWatermark(originImage, imgExtensionName, watermarkImg, watermarkPosition, textPadding);
                    }
                }
                else
                {
                    return AddWatermark(originImage, imgExtensionName, null, watermarkPosition, textPadding);
                }
            }

        }


        public static MemoryStream AddWatermark(Stream originImageStream, string imgExtensionName, string watermarkImgPath, WatermarkPositionEnum watermarkPosition = WatermarkPositionEnum.RightButtom, int textPadding = 10)
        {
            if (originImageStream == null)
            {
                return null;
            }
            using (var originImage = Image.FromStream(originImageStream))
            {

                if (!string.IsNullOrEmpty(watermarkImgPath))
                {
                    using (var watermarkImg = Image.FromFile(watermarkImgPath))
                    {
                        return AddWatermark(originImage, imgExtensionName, watermarkImg, watermarkPosition, textPadding);
                    }
                }
                else
                {
                    return AddWatermark(originImage, imgExtensionName, null, watermarkPosition, textPadding);
                }
            }

        }

        public static Image AddWatermarkImg(Stream originImageStream, string imgExtensionName, string watermarkImgPath, WatermarkPositionEnum watermarkPosition = WatermarkPositionEnum.RightButtom, int textPadding = 10)
        {
            if (originImageStream == null)
            {
                return null;
            }
            using (var originImage = Image.FromStream(originImageStream))
            {

                if (!string.IsNullOrEmpty(watermarkImgPath))
                {
                    using (var watermarkImg = Image.FromFile(watermarkImgPath))
                    {
                        return Image.FromStream(AddWatermark(originImage, imgExtensionName, watermarkImg, watermarkPosition, textPadding));
                    }
                }
                else
                {
                    return Image.FromStream(AddWatermark(originImage, imgExtensionName, null, watermarkPosition, textPadding));
                }
            }

        }

        /// <summary>
        /// 等比例缩放图片
        /// </summary>
        /// <param name="sourceImage">原图</param>
        /// <param name="targetWidth">期望宽度</param>
        /// <param name="targetHeight">期望高度</param>
        /// <returns></returns>
        public static Image ZoomImage(Image sourceImg, int targetWidth, int targetHeight)
        {
            Bitmap targetImg;
            try
            {
                if (targetWidth == 0 || targetHeight == 0)
                {
                    targetWidth = sourceImg.Width;
                    targetHeight = sourceImg.Height;
                }

                int orginalWidth = Convert.ToInt32(sourceImg.Width);
                int orginalHeight = Convert.ToInt32(sourceImg.Height);
                int newHeight = Convert.ToInt32(((double)targetWidth / orginalWidth) * orginalHeight);
                int newWidth = Convert.ToInt32(((double)targetHeight / orginalHeight) * orginalWidth);

                if (orginalWidth <= targetWidth && orginalHeight <= targetHeight)
                {
                    return sourceImg;
                }
                else
                {
                    if (newWidth > targetWidth)//即使把高度固定，宽度还是太大
                    {
                        targetImg = new Bitmap(sourceImg, targetWidth, newHeight);
                    }
                    else
                    {
                        targetImg = new Bitmap(sourceImg, newWidth, targetHeight);
                    }
                }

                sourceImg.Dispose();

                using (EncoderParameters eps = new EncoderParameters(1))
                {
                    using (var ep = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100))
                    {
                        eps.Param[0] = ep;
                        sourceImg.Dispose();
                        return targetImg;
                    }
                }
            }
            catch
            {
                return null;

            }
        }
        #endregion
    }
}
