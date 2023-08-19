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
/*           // Define language letters
            Dictionary<string, string> languageLetters = new Dictionary<string, string>
            {
                { "English", "abcdefghijklmnopqrstuvwxyz" },
                { "French", "abcdefghijklmnopqrstuvwxyzàâæçéèêëîïôœùûüÿ" },
                { "German", "abcdefghijklmnopqrstuvwxyzäöüß" },
                { "Spanish", "abcdefghijklmnopqrstuvwxyzáéíóúñ" },
                { "Ukrainian", "абвгдеєжзиіїйклмнопрстуфхцчшщьюя" },
                { "Russian", "абвгдеёжзийклмнопрстуфхцчшщъыьэюя" }
            };

            // Define language-specific digraphs and trigraphs
            Dictionary<string, string[]> languageLetterCombinations = new Dictionary<string, string[]>
            {
                { "English", new string[] { "th", "he", "an", "in", "er", "on" } },
                { "French", new string[] { "de", "le", "la", "et", "en", "au" } },
                { "German", new string[] { "sch", "ie", "ei", "en", "ch", "st" } },
                { "Spanish", new string[] { "de", "la", "el", "que", "en", "del" } },
                { "Ukrainian", new string[] { "ін", "не", "пр", "то", "на", "ни" } },
                { "Russian", new string[] { "ст", "но", "то", "на", "ен", "ов" } }
            };

            // Define language-specific characters
            Dictionary<string, char[]> languageCharacters = new Dictionary<string, char[]>
            {
                { "French", new char[] { 'à', 'â', 'æ', 'ç', 'é', 'è', 'ê', 'ë', 'î', 'ï', 'ô', 'œ', 'ù', 'û', 'ü', 'ÿ' } },
                { "Spanish", new char[] { 'á', 'é', 'í', 'ó', 'ú', 'ñ' } },
                { "Ukrainian", new char[] { 'є', 'ї', 'ґ', 'і', 'ў' } },
                { "Russian", new char[] { 'ё', 'э', 'ы', 'ъ', 'ь' } }
            };
