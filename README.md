# OCR Solutions Documentation

This documentation provides an overview of three OCR (Optical Character Recognition) solutions: Google Lens OCR, Windows Media OCR, and Tesseract OCR. These solutions cater to different environments and requirements, offering a range of functionalities for extracting text from images.

---

## Clone repository

```sh
git clone https://github.com/codefrydev/OCR.git
```

### Usage

1. Download the project.
2. Select You Choice of Project -> Program.cs File
3. Provide Image path you want to work on
4. Run the Application

## 1. Google Lens OCR

```cs
var path = "imagetwo.jpg";  
var extract = new Core();
var result = await extract.ScanByFileAsync(path);
foreach (var item in result)
{
    Console.Out.WriteLine(item);
}
```

### Overview

Google Lens OCR is derived from the [Chrome Lens OCR project, which is a JavaScript-based library for text recognition](https://github.com/dimdenGD/chrome-lens-ocr). This project has been converted to .NET Core and .NET Framework, offering flexibility for different development environments.

### Features

- **Supported Formats:** Only JPG format.
- **Accuracy:** Provides almost accurate results.
- **Compatibility:** Works with both .NET Core and .NET Framework.

---

## 2. Windows Media OCR

```cs
internal class Program
{
    static void Main()
    {
        Console.WriteLine(Extract.ExtractText("imagetwo.jpg"));
    }
}
```

### Overview

Windows Media OCR Code is extracted from the [PowerToys repository](https://github.com/microsoft/PowerToys), a popular set of utilities for Windows 10 and above. This OCR solution leverages the capabilities of the Windows operating system to perform text recognition.

### Features

- **Supported Formats:** Various image formats supported by Windows.
- **Compatibility:** Works on Windows 10 and above.
- **Integration:** Can be integrated into Windows applications.

### Requirement 

- Make Sure in Projct Setting It is Window 10 and above
have Imastalled following package from Nuget

```xml
<ItemGroup>
		<PackageReference Include="Microsoft.Windows.CsWinRT" />
		<PackageReference Include="System.ComponentModel.Composition" />
		<PackageReference Include="System.Drawing.Common" Version="8.0.6" />
		<PackageReference Include="System.Windows.Extensions" Version="8.0.0" />
		<PackageReference Include="WPF-UI" />
</ItemGroup>
```
---

## 3. Tesseract OCR

```cs
static readonly string imagPath = "imagetwo.jpg";
static string extractedTextImageSharp = string.Empty;

using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.LstmOnly))
{

    using var img = Pix.LoadFromFile(path);
    using var page = engine.Process(img);
    extractedTextImageSharp = page.GetText();
} 
Console.WriteLine(extractedTextImageSharp);
```

### Overview

[Tesseract OCR is one of the most popular open-source OCR libraries](https://github.com/tesseract-ocr/tesseract). It supports a wide range of languages and can recognize text from various image formats.

### Features

- **Supported Formats:** Multiple image formats (JPG, PNG, BMP, etc.).
- **Languages:** Supports multiple languages.
- **Extensibility:** Highly customizable and extensible.

## Conclusion

Each OCR solution offers unique features and compatibility options. Google Lens OCR is suitable for .NET environments, Windows Media OCR is ideal for Windows 10 and above, and Tesseract OCR is a versatile open-source option supporting multiple languages and formats. Choose the one that best fits your project's requirements.
