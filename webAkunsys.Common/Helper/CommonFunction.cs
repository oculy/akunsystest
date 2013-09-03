using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace webAkunsys.Common.Helper
{
    public class CommonFunction
    {

        public static void LogToFile(string logMessage, string Type, HttpServerUtilityBase Server)
        {
            string path = string.Empty;
            path = Path.Combine(Server.MapPath("../Asset/"), "log.txt");
            // fallback to log.txt if can't find the filepath
            using (StreamWriter w = File.AppendText(path))
            {

                w.WriteLine("{0} {1} Type = {2}  Message: --- {3}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString(), Type, logMessage ?? string.Empty);
                // Update the underlying file.
                w.Flush();
                // Close the writer and underlying file.
                w.Close();
            }
        }

    }
}
