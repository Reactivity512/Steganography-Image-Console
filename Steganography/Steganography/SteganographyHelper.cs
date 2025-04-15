namespace Steganography;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Text;

internal static class SteganographyHelper
{
    /// <summary>
    /// Скрывает текст в изображении (с использованием младших битов).
    /// </summary>
    public static void HideText(string imagePath, string text, string outputPath)
    {
        using Image<Rgba32> image = Image.Load<Rgba32>(imagePath);
        byte[] data = Encoding.UTF8.GetBytes(text + '\0');
        InjectData(image, data);
        image.Save(outputPath);
    }

    private static void InjectData(Image<Rgba32> image, byte[] data)
    {
        int maxBits = image.Width * image.Height * 3; // 3 канала (R, G, B)
        if (data.Length * 8 > maxBits)
            throw new InvalidOperationException("Изображение слишком маленькое для данного текста");

        int bitIndex = 0;
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                if (bitIndex >= data.Length * 8) return;

                Rgba32 pixel = image[x, y];
                pixel.R = SetLsb(pixel.R, data, ref bitIndex);
                pixel.G = SetLsb(pixel.G, data, ref bitIndex);
                pixel.B = SetLsb(pixel.B, data, ref bitIndex);
                image[x, y] = pixel;
            }
        }
    }

    private static byte SetLsb(byte color, byte[] data, ref int bitIndex)
    {
        if (bitIndex < data.Length * 8)
        {
            int bytePos = bitIndex / 8;
            int bitPos = bitIndex % 8;
            int bitValue = (data[bytePos] >> (7 - bitPos)) & 1;
            color = (byte)((color & 0xFE) | bitValue);
            bitIndex++;
        }
        return color;
    }

    /// <summary>
    /// Извлекает скрытый текст из изображения.
    /// </summary>
    public static string ExtractText(string imagePath)
    {
        using Image<Rgba32> image = Image.Load<Rgba32>(imagePath);
        byte[] data = ExtractData(image);
        return DecodeText(data);
    }

    private static byte[] ExtractData(Image<Rgba32> image)
    {
        byte[] result = new byte[image.Width * image.Height * 3 / 8 + 1]; // Максимально возможный размер
        int byteIndex = 0;
        int bitIndex = 0;

        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                Rgba32 pixel = image[x, y];
                byteIndex = UpdateByte(result, byteIndex, ref bitIndex, pixel.R);
                byteIndex = UpdateByte(result, byteIndex, ref bitIndex, pixel.G);
                byteIndex = UpdateByte(result, byteIndex, ref bitIndex, pixel.B);

                if (byteIndex > 0 && result[byteIndex - 1] == 0) // Найден маркер конца
                {
                    Array.Resize(ref result, byteIndex - 1);
                    return result;
                }
            }
        }

        return [];
    }

    private static string DecodeText(byte[] data)
    {
        if (data == null || data.Length == 0)
            return string.Empty;

        return Encoding.UTF8.GetString(data).TrimEnd('\0');
    }

    private static int UpdateByte(byte[] result, int byteIndex, ref int bitIndex, byte color)
    {
        if (byteIndex >= result.Length) return byteIndex;

        int bitValue = color & 1;
        result[byteIndex] = (byte)((result[byteIndex] << 1) | bitValue);
        bitIndex++;

        if (bitIndex % 8 == 0)
        {
            byteIndex++;
            bitIndex = 0;
        }

        return byteIndex;
    }
}
