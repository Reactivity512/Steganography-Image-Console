using Steganography;

class Program
{
    static void Main(string[] args)
    {
        if (args[0] == "-r")
        {
            Read(args[1]);
        }
        else if (args[0] == "-w")
        {
            Write(args[1], string.Join(" ", args.Skip(2)));
        }
    }

    static void Read(string path)
    {
        try
        {
            Steganography.Steganography steganography = new (path);
            string text = steganography.ReadFromImage();

            Console.WriteLine($"Извлечённый из: {path}");
            Console.WriteLine($"Извлечённый текст: {text}");
        }
        catch (Exception ex)
        {
             Console.WriteLine(ex.Message);
        }
    }

    static void Write(string path, string text)
    {
        try
        {
            Steganography.Steganography steganography = new (path);
            string PathImageWithHiddenText = steganography.WriteToImage(text);
            
            Steganography.Steganography steganographyForImageWithHiddenText = new (PathImageWithHiddenText);

            if (steganographyForImageWithHiddenText.ReadFromImage() == text)
            {
                Console.WriteLine("Успешно сохранено");
            }
            else
            {
                Console.WriteLine("Текст сохранен неудачно, чтение из картинки невозможно");
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
