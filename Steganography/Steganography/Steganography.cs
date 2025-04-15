namespace Steganography;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

public sealed class Steganography(string path)
{
    private string PathImage = path;
    private string? PathImageWithHiddenText = null;
    private static readonly List<string> acceptableImageFormats = ["bmp", "png", "jpg", "jpeg", "qoi", "tga", "tiff"];
    public static readonly string AdditionToFileName = "HiddenText";

    public string ReadFromImage()
    {
        return SteganographyHelper.ExtractText(PathImage);
    }

    public string WriteToImage(string text)
    {
        CheckingAvailableImageFormats(PathImage);
        PreparationImage(PathImage);

        SteganographyHelper.HideText(
            imagePath: PathImage,
            text: text,
            outputPath: GetPathImageWithHiddenText()
        );

        return GetPathImageWithHiddenText();
    }

    private static void CheckingAvailableImageFormats(string path)
    {
        string fileFormat = path[(path.LastIndexOf('.') + 1)..].ToLower();

        if (!acceptableImageFormats.Contains(fileFormat))
        {
            throw new ArgumentException("Недопустимый формат картинки");
        }
    }

    private void PreparationImage(string path)
    {
        var fileInfo = new FileInfo(path);
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
        string extension = fileInfo.Extension.ToLower();

        if (extension is ".jpg" or ".jpeg")
        {
            string newPath = $"{fileNameWithoutExtension}.png";
            ConvertJpgToPng(path, newPath);
            
            PathImage = newPath;
            PathImageWithHiddenText = path.Replace(
                fileInfo.Name, 
                $"{fileNameWithoutExtension}{AdditionToFileName}.png"
            );
        }
        else
        {
            PathImageWithHiddenText = path.Replace(
                fileNameWithoutExtension, 
                $"{fileNameWithoutExtension}{AdditionToFileName}"
            );
        }
    }

    private static void ConvertJpgToPng(string inputPath, string outputPath)
    {
        using Image image = Image.Load(inputPath);
        image.Save(outputPath, new PngEncoder());
    }

    public string GetPathImageWithHiddenText()
    {
        if (PathImageWithHiddenText == null)
        {
            throw new NullReferenceException("Осутствует путь к файлу со скрытым текстом");
        }

        return PathImageWithHiddenText;
    }
}
