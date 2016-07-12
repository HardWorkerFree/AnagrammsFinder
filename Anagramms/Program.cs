using System;

namespace Anagramms
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFileName;
            string outputFileName;
            Anagramms anagramms = new Anagramms();

            Console.WriteLine("Введите имя входного файла или нажмите Enter:");
            inputFileName = Console.ReadLine();

            bool noFileName = (inputFileName == "");

            if (noFileName)
            {
                try
                { 
                    anagramms.CreateFileOfAnagrammsFromFile();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Введите имя выходного файла:");
                outputFileName = Console.ReadLine();

                try
                {
                    anagramms.CreateFileOfAnagrammsFromFile(inputFileName, outputFileName);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message + "(если вы перетащили файл с помощью мыши - уберите кавычки)");
                }
            }

            Console.ReadKey();
        }
    }
}
