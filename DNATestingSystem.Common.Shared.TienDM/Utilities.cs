using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DNATestingSystem.Common.Shared.TienDM
{
    public class Utilities
    {
        private static string loggerFilePath = Directory.GetCurrentDirectory() + @"\DataLog.txt";

        public static string ConvertObjectToJsonString<T>(T entity)
        {
            /*
            JSON Serialize: Convert specified generic type (specified object) into a JSON string
            */
            string jsonString = JsonSerializer.Serialize(entity, new JsonSerializerOptions { WriteIndented = false });

            return jsonString;
        }

        public static void WriteLoggerFile(string content)
        {
            try
            {
                var path = Directory.GetCurrentDirectory();

                using (var file = File.Open(loggerFilePath, FileMode.Append, FileAccess.Write))
                using (var writer = new StreamWriter(file))
                {
                    writer.WriteLineAsync(content);
                    writer.FlushAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
