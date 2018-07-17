using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace Utils.SecurityEx
{
   public class Base64Helper
    {
       /// <summary>
       /// base64 编码
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
       public static string ToBase64String(string value)
       {
           if (value == null || value == "")
           {
               return "";
           }
           byte[] bytes = Encoding.UTF8.GetBytes(value);
           return Convert.ToBase64String(bytes);
       }

       /// <summary>
       /// base64 解码
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
       public static string UnBase64String(string value)
       {
           if (value == null || value == "")
           {
               return "";
           }
           byte[] bytes = Convert.FromBase64String(value);
           return Encoding.UTF8.GetString(bytes);
       }


       /// <summary>
       /// 图片转base64
       /// </summary>
       /// <param name="img"></param>
       /// <returns></returns>
       public static string ImgToBase64(Image img)
       {
           try
           {
               using (Bitmap bmp = new Bitmap(img))
               {
                   using (MemoryStream ms = new MemoryStream())
                   {
                       bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                       byte[] arr = new byte[ms.Length];
                       ms.Position = 0;
                       ms.Read(arr, 0, (int)ms.Length);
                       ms.Close();
                       return Convert.ToBase64String(arr);
                   }
               }
           }
           finally
           {
               if (img != null)
               {
                   img.Dispose();
                   img = null;
               }
           }
       }

       /// <summary>
       /// base64转图片
       /// </summary>
       /// <param name="base64Str"></param>
       /// <returns></returns>
       public static Image Base64ToImg(string base64Str)
       {
          
               byte[] arr = Convert.FromBase64String(base64Str);
               using (MemoryStream ms = new MemoryStream(arr))
               {
                   return new System.Drawing.Bitmap(ms);
               }
         
       }


    }
}
