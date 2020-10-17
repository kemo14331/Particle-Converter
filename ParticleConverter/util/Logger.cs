using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ParticleConverter.util
{
    public static class Logger
    {
        public static void WriteExceptionLog(Exception e)
        {
            DateTime dt = DateTime.Now;
            string result = dt.ToString("yyyy-MM-dd_HH.mm.ss");
            StreamWriter streamWriter = null;
            try
            {
                Directory.CreateDirectory(@".\exceptionLog");
                string path = $@".\exceptionLog\{result}.txt";
                streamWriter = new StreamWriter(path);
                streamWriter.WriteLine("Member:" + e.TargetSite.Name);
                streamWriter.WriteLine(e.Message);
                streamWriter.WriteLine(e.StackTrace);
            }
            finally
            {
                if (streamWriter != null) streamWriter.Close();
            }
        }
    }
}
