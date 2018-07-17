using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utils.NetEx
{
   public class IpHelper
    {
       public static long IpToInt64(IPAddress ipAddress)
       {
           string[] items = ipAddress.MapToIPv4().ToString().Split('.');
           return long.Parse(items[0]) << 24
                   | long.Parse(items[1]) << 16
                   | long.Parse(items[2]) << 8
                   | long.Parse(items[3]);
       }
       public static long IpToInt64(string ipAddress)
       {
           try
           {
               string[] items = ipAddress.Split('.');
               return long.Parse(items[0]) << 24
                       | long.Parse(items[1]) << 16
                       | long.Parse(items[2]) << 8
                       | long.Parse(items[3]);
           }
           catch
           {
               throw new ArgumentException(string.Format("{0} is not a valid IP Address.", ipAddress), "ipAddress");
           }
       }

       public static string Int64ToIp(long ipInt)
       {
           StringBuilder sb = new StringBuilder();
           sb.Append((ipInt >> 24) & 0xFF).Append(".");
           sb.Append((ipInt >> 16) & 0xFF).Append(".");
           sb.Append((ipInt >> 8) & 0xFF).Append(".");
           sb.Append(ipInt & 0xFF);
           return sb.ToString();
       }

       public static bool IsIp(string ipAddress)
       {
           //匹配正确的IP地址
           Regex reg = new Regex(@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$");

           return reg.IsMatch(ipAddress);
       
       }

    }
}
