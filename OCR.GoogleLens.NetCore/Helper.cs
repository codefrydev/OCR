using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO.Compression;

internal class Helper
{
    public static async Task<byte[]> ResizeImageAsync(byte[] buffer, int maxWidth, int maxHeight)
    {
        using var image = Image.Load<Rgba32>(buffer);
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = new Size(maxWidth, maxHeight)
        }));
        using var outputStream = new MemoryStream();
        await image.SaveAsJpegAsync(outputStream);
        return outputStream.ToArray();
    }
    public static (int Width, int Height) ImageDimensionsFromData(byte[] data)
    {
        using var image = Image.Load(data);
        return (image.Width, image.Height);
    }
    public static byte[] DecompressDeflate(byte[] deflateData)
    {
        using (MemoryStream inputStream = new MemoryStream(deflateData))
        using (DeflateStream deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress))
        using (MemoryStream outputStream = new MemoryStream())
        {
            deflateStream.CopyTo(outputStream);
            return outputStream.ToArray();
        }
    }
    public static byte[] DecompressGzip(byte[] gzipData)
    {
        using (MemoryStream inputStream = new MemoryStream(gzipData))
        using (GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
        using (MemoryStream outputStream = new MemoryStream())
        {
            gzipStream.CopyTo(outputStream);
            return outputStream.ToArray();
        }
    }
    public static byte[] DecompressBrotli(byte[] brotliData)
    {
        using (MemoryStream inputStream = new MemoryStream(brotliData))
        using (BrotliStream brotliStream = new BrotliStream(inputStream, CompressionMode.Decompress))
        using (MemoryStream outputStream = new MemoryStream())
        {
            brotliStream.CopyTo(outputStream);
            return outputStream.ToArray();
        }
    }
}