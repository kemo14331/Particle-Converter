using System;
using System.IO;

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
                streamWriter.WriteLine(e.Data);
                streamWriter.WriteLine(e.ToString());
            }
            finally
            {
                if (streamWriter != null) streamWriter.Close();
            }
        }
    }
}
