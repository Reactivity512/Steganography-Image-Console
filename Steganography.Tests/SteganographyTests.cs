namespace Steganography.Tests;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class DirectoryFixture : IDisposable
{
    public DirectoryFixture()
    {
        Directory.CreateDirectory(SteganographyTests.temporaryFolder);
    }

    public void Dispose()
    {
        Directory.Delete(path: SteganographyTests.temporaryFolder, recursive: true);
    }
}

public class SteganographyTests : IClassFixture<DirectoryFixture>
{
    public static readonly string temporaryFolder = "tmp";
    private static readonly string temporaryFileName = "test";

    [Fact]
    public void Steganography_SuccessfullyHideTextInPng()
    {
        // Arrange
        var pathFile = GetTemporaryImagePathNoFileFormat() + ".png";
        var text = "Спрятанный текст";
        var image = CreateTestImage(100, 100);

        // Act
        image.SaveAsPng(pathFile);
        Steganography steganography = new (pathFile);
        var pathFileHiddenText = steganography.WriteToImage(text);

        // Assert
        Steganography steganographyForImageHiddenText = new (pathFileHiddenText);
        Assert.Equal(steganographyForImageHiddenText.ReadFromImage(), text);
    }

    [Theory]
    [InlineData(1000)]
    [InlineData(100_000)]
    [InlineData(300_000)]
    public void Steganography_SuccessfullyMaxLimitHideTextInPng(int lengthText)
    {
        // Arrange
        string text = new('X', lengthText);
        var pathFile = GetTemporaryImagePathNoFileFormat() + ".png";
        var image = CreateTestImage(1000, 1000);

        // Act
        image.SaveAsPng(pathFile);
        Steganography steganography = new (pathFile);
        var pathFileHiddenText = steganography.WriteToImage(text);

        // Assert
        Steganography steganographyForImageHiddenText = new (pathFileHiddenText);
        Assert.Equal(steganographyForImageHiddenText.ReadFromImage(), text);
    }

    [Theory]
    [InlineData(100_000)]
    [InlineData(1_000_000)]
    public void Steganography_FailedMaxLimitHideTextInPng(int lengthText)
    {
        // Arrange
        string text = new('X', lengthText);
        var pathFile = GetTemporaryImagePathNoFileFormat() + ".png";
        var image = CreateTestImage(100, 100);

        // Act
        image.SaveAsPng(pathFile);
        Steganography steganography = new (pathFile);

        // Assert
        Assert.Throws<InvalidOperationException>(() => steganography.WriteToImage(text));
    }

    [Fact]
    public void Steganography_SuccessfullyHideTextInBmp()
    {
        // Arrange
        var pathFile = GetTemporaryImagePathNoFileFormat() + ".bmp";
        var text = "Спрятанный текст";
        var image = CreateTestImage(100, 100);

        // Act
        image.SaveAsBmp(pathFile);
        Steganography steganography = new (pathFile);
        var pathFileHiddenText = steganography.WriteToImage(text);

        // Assert
        Steganography steganographyForImageHiddenText = new (pathFileHiddenText);
        Assert.Equal(steganographyForImageHiddenText.ReadFromImage(), text);
    }

    [Fact]
    public void Steganography_SuccessfullyHideTextInJpeg()
    {
        // Arrange
        var pathFile = GetTemporaryImagePathNoFileFormat() + ".jpeg";
        var text = "Спрятанный текст";
        var image = CreateTestImage(100, 100);

        // Act
        image.SaveAsJpeg(pathFile);
        Steganography steganography = new (pathFile);
        var pathFileHiddenText = steganography.WriteToImage(text);

        // Assert
        Steganography steganographyForImageHiddenText = new (pathFileHiddenText);
        Assert.Equal(steganographyForImageHiddenText.ReadFromImage(), text);
    }

    [Fact]
    public void Steganography_SuccessfullyHideTextInQoi()
    {
        // Arrange
        var pathFile = GetTemporaryImagePathNoFileFormat() + ".qoi";
        var text = "Спрятанный текст";
        var image = CreateTestImage(100, 100);

        // Act
        image.SaveAsQoi(pathFile);
        Steganography steganography = new (pathFile);
        var pathFileHiddenText = steganography.WriteToImage(text);

        // Assert
        Steganography steganographyForImageHiddenText = new (pathFileHiddenText);
        Assert.Equal(steganographyForImageHiddenText.ReadFromImage(), text);
    }

    [Fact]
    public void Steganography_SuccessfullyHideTextInTga()
    {
        // Arrange
        var pathFile = GetTemporaryImagePathNoFileFormat() + ".tga";
        var text = "Спрятанный текст";
        var image = CreateTestImage(100, 100);

        // Act
        image.SaveAsTga(pathFile);
        Steganography steganography = new (pathFile);
        var pathFileHiddenText = steganography.WriteToImage(text);

        // Assert
        Steganography steganographyForImageHiddenText = new (pathFileHiddenText);
        Assert.Equal(steganographyForImageHiddenText.ReadFromImage(), text);
    }

    [Fact]
    public void Steganography_SuccessfullyHideTextInTiff()
    {
        // Arrange
        var pathFile = GetTemporaryImagePathNoFileFormat() + ".tiff";
        var text = "Спрятанный текст";
        var image = CreateTestImage(100, 100);

        // Act
        image.SaveAsTiff(pathFile);
        Steganography steganography = new (pathFile);
        var pathFileHiddenText = steganography.WriteToImage(text);

        // Assert
        Steganography steganographyForImageHiddenText = new (pathFileHiddenText);
        Assert.Equal(steganographyForImageHiddenText.ReadFromImage(), text);
    }

    [Fact]
    public void Steganography_FailedWriteToImageInWebp()
    {
        // Arrange
        var pathFile = GetTemporaryImagePathNoFileFormat() + ".webp";
        var text = "Спрятанный текст";
        var image = CreateTestImage(100, 100);

        // Act
        image.SaveAsWebp(pathFile);
        Steganography steganography = new (pathFile);

        // Assert
        Assert.Throws<ArgumentException>(() => steganography.WriteToImage(text));
    }

    [Fact]
    public void Steganography_FailedWriteToImageInGif()
    {
        // Arrange
        var pathFile = GetTemporaryImagePathNoFileFormat() + ".gif";
        var text = "Спрятанный текст";
        var image = CreateTestImage(100, 100);

        // Act
        image.SaveAsGif(pathFile);
        Steganography steganography = new (pathFile);

        // Assert
        Assert.Throws<ArgumentException>(() => steganography.WriteToImage(text));
    }

    [Fact]
    public void Steganography_FailedWriteToImageInPbm()
    {
        // Arrange
        var pathFile = GetTemporaryImagePathNoFileFormat() + ".pbm";
        var text = "Спрятанный текст";
        var image = CreateTestImage(100, 100);

        // Act
        image.SaveAsPbm(pathFile);
        Steganography steganography = new (pathFile);

        // Assert
        Assert.Throws<ArgumentException>(() => steganography.WriteToImage(text));
    }

    private static Image<Rgba32> CreateTestImage(int width, int height)
    {
        var image = new Image<Rgba32>(width, height);
        
        // Заполняем случайными данными (для реалистичности)
        var rnd = new Random();
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                image[x, y] = new Rgba32(
                    (byte)rnd.Next(255),
                    (byte)rnd.Next(255),
                    (byte)rnd.Next(255)
                );
            }
        }
        return image;
    }

    private static string GetTemporaryImagePathNoFileFormat()
    {
        return temporaryFolder + "\\" + temporaryFileName;
    }
}
