using System;
using System.Collections.Generic;

namespace LanguageRecognition
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Language Recognition Program!");
            Console.WriteLine("Enter the text you want to recognize:");

            string inputText = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(inputText))
            {
                string detectedLanguage = RecognizeLanguage(inputText);
                Console.WriteLine($"Detected Language: {detectedLanguage}");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter some text.");
            }
        }
    }
}
