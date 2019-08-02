using System;
using System.IO;

namespace HorseApi.Helpers
{
    public static class Logging
    {
        public static void LogAction(string message)
        {
            string path = getLogFolderPath();
            string fileName = "/ACTION-" + DateTime.Today.ToString("dd.MM.yyyy") + ".txt";

            using (StreamWriter writer = new StreamWriter(path + fileName, true))
            {
                writer.WriteLine("******Client Error******");
                writer.WriteLine(DateTime.Now);
                writer.WriteLine(message);
                writer.WriteLine("******Client Error******");
                writer.WriteLine("");
                writer.Close();
            }
        }

        public static void LogClientError(string message)
        {
            string path = getLogFolderPath();
            string fileName = "/CLIENT-" + DateTime.Today.ToString("dd.MM.yyyy") + ".txt";

            using (StreamWriter writer = new StreamWriter(path + fileName, true))
            {
                writer.WriteLine("******Client Error******");
                writer.WriteLine(DateTime.Now);
                writer.WriteLine(message);
                writer.WriteLine("******Client Error******");
                writer.WriteLine("");
                writer.Close();
            }
        }

        public static void LogServerError(Exception ex)
        {
            string path = getLogFolderPath();
            string fileName = "/SERVER-" + DateTime.Today.ToString("dd.MM.yyyy") + ".txt";

            using (StreamWriter writer = new StreamWriter(path + fileName, true))
            {
                writer.WriteLine("******Server Error******");
                writer.WriteLine(DateTime.Now);
                writer.WriteLine(ex.StackTrace);
                writer.WriteLine(ex.Message);
                writer.WriteLine("******Server Error******");
                writer.WriteLine("");
                writer.Close();
            }
        }

        private static string getLogFolderPath()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Log");
            bool exists = System.IO.Directory.Exists(path);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(path);
            }

            return path;
        }
    }
}
