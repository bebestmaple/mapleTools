
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
   public class FileHelper
    {
        public static IEnumerable<string> GetFileName(string path,string[] fileExs)
        {
            DirectoryInfo root = new DirectoryInfo(path);
            foreach (FileInfo f in root.GetFiles())
            {
                if (fileExs != null && fileExs.Length > 0)
                {
                    if (fileExs.Contains(Path.GetExtension(f.FullName).ToLower()))
                    {
                        yield return f.FullName;
                    }
                }
                else
                {
                    yield return f.FullName;
                }

            }
        }

        public static IEnumerable<string> GetDirectoryAllFiles(string path, string[] fileExs = null)
        {
            var fileList = new List<string>();
            fileList.AddRange(GetFileName(path,fileExs));
            DirectoryInfo root = new DirectoryInfo(path);
            foreach (DirectoryInfo d in root.GetDirectories())
            {
                GetDirectoryAllFiles(d.FullName,fileExs);
            }
            return fileList;
        }
    }
}
