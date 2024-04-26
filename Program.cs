
using Microsoft.Extensions.Configuration;
using Svg;
using System.Drawing.Imaging;
using System.Drawing;
using System.Xml;

var configBuilder = new ConfigurationBuilder().
   AddJsonFile("appsettings.json").Build();
var configSection = configBuilder.GetSection("AppSettings");

var inputFolder = Path.Combine(Environment.CurrentDirectory, configSection["InputFolder"] ?? "input");
var outputFolder = Path.Combine(Environment.CurrentDirectory, configSection["OutputFolder"] ?? "output");

Console.WriteLine($"Inputfolder: {inputFolder}");
Console.WriteLine($"Outputfolder: {outputFolder}");

foreach (string file in Directory.EnumerateFiles(inputFolder, "*.svg"))
{
    var filename = Path.GetFileName(file);
    var svgFilename = Path.Combine(inputFolder, filename);
    var outputSvgFilename = Path.Combine(outputFolder, filename);

    var svgImage = LoadSVGImage(svgFilename);

    CreateAllImages(svgImage, filename, configBuilder);

    svgImage.Dispose();
    File.Move(svgFilename, outputSvgFilename);
    Console.WriteLine($"SVG Moved: {outputSvgFilename}");
}

Console.WriteLine("Conversion Finished!");

void CreateAllImages(Bitmap svgImage, string file, IConfigurationRoot configBuilder)
{
    IConfigurationSection pngSizes = configBuilder.GetSection("AppSettings:PngFormats");
    IConfigurationSection gifSizes = configBuilder.GetSection("AppSettings:GifFormats");

    string filenameWithoutExtension = Path.GetFileNameWithoutExtension(file);

    if (pngSizes != null)
    {
        foreach (var section in pngSizes.GetChildren())
        {
            if (!int.TryParse(section.Value ?? "32", out int size))
            {
                size = 32;
            }
            CreateImage(svgImage, ImageFormat.Png, filenameWithoutExtension, outputFolder, size, false);
        }
    }

    if (gifSizes != null)
    {
        ImageFormat currentFormat = ImageFormat.Gif;
        foreach (var section in gifSizes.GetChildren())
        {
            if (!int.TryParse(section.Value ?? "32", out int size))
            {
                size = 32;
            }
            CreateImage(svgImage, ImageFormat.Gif, filenameWithoutExtension, outputFolder, size, true);
        }
    }
}

static void CreateImage(Bitmap baseImage, ImageFormat outputFormat, string filename, string outputfolder, int imageSize, bool isGif)
{
    string outputFilename = $"{filename}_{imageSize}x{imageSize}{(isGif ? ".gif" : ".png")}";
    string outputFilenameFullpath = Path.Combine(outputfolder, outputFilename);
    Bitmap outputImage = new Bitmap(imageSize, imageSize);
    Graphics graphics = Graphics.FromImage(outputImage);
    graphics.DrawImage(baseImage, 0, 0, imageSize, imageSize);

    outputImage.Save(outputFilenameFullpath, outputFormat);
    Console.WriteLine($"Created File: {outputFilenameFullpath}");
}

static Bitmap LoadSVGImage(string svgfullfilename)
{
    var xmlDoc = new XmlDocument
    {
        XmlResolver = null
    };
    xmlDoc.Load(svgfullfilename);
    var xml = xmlDoc.InnerXml;
    var svgDoc = SvgDocument.FromSvg<SvgDocument>(xml);
    return svgDoc.Draw();
}