using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

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
static string ImageTOJpg(string path)
{
    string outputFilePath = "output.jpg";
    using (Image image = Image.Load(path))
    {
        image.Save(outputFilePath, new JpegEncoder());
    }
    return outputFilePath;
}
