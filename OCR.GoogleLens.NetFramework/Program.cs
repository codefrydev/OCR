using System;
using System.Threading.Tasks;

namespace OCR.GoogleLens.NetFramework
{
    internal class Program
    {
        static async Task Main()
        {
            var path = "imagetwo.jpg";

            var extract = new Core();

            try
            {
                var result = await extract.ScanByFileAsync(path);
                foreach (var item in result)
                {
                    await Console.Out.WriteLineAsync(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
