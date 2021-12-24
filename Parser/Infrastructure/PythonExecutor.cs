using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Python
{
    public class PythonExecutor
    {
        string pathToInterpritator;
        string pathToScript;
        public PythonExecutor(string pathToPythonInterpritator, string pathToPythonScript)
        {
            pathToInterpritator = pathToPythonInterpritator;
            pathToScript = pathToPythonScript;
        }

        string EncodeTo64(string toEncode)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(toEncode);
            return Convert.ToBase64String(plainTextBytes);
        }
        string DecodeFrom64(string encodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(encodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public Task<string> ExecuteScript(string argument)
        {
            return Task.Run(() =>
            {
                var input = EncodeTo64(argument);

                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = pathToInterpritator;
                start.Arguments = $"{pathToScript} {input}";
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                string result;
                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        result = reader.ReadToEnd();
                    }
                }

                if (result == string.Empty)
                    return result;
                result = result.Substring(2, result.Length - 5);
                result = DecodeFrom64(result);

                return result;
            });
        }
    }
}
