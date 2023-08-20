using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LanguageRecognition
{
    class Program
    {
        const int definitelyCorrect = -1;
        const int definitelyNotPossible = -2;

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

        static string RecognizeLanguage(string inputText)
        {
            // Define language-specific letters
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
                { "English", new string[] { "th", "he", "an", "in", "er", "on", "ll"} },
                
                { "French", new string[] { "de", "le", "la", "et", "en", "au" } },
                
                { "German", new string[] { "sch", "ie", "ei", "en", "ch", "st" } },
                
                { "Spanish", new string[] { "de", "la", "el", "que", "en", "del" } },
                
                { "Ukrainian", new string[] { "ін", "не", "пр", "то", "на", "ни" } },
                
                { "Russian", new string[] { "ст", "но", "то", "на", "ен", "ов" } }
            };

            // Define language-specific characters
            Dictionary<string, char[]> languageCharacters = new Dictionary<string, char[]>
            {
                { "French", new char[] { 'à', 'â', 'æ', 'ç', 'é', 'è', 'ê', 'ë', 'î', 'ô', 'œ', 'ù', 'û', 'ü', 'ÿ' } },
                
                { "Spanish", new char[] { 'á', 'é', 'í', 'ó', 'ú', 'ñ' } },
                
                { "Ukrainian", new char[] { 'є', 'ї', 'ґ', 'і' } },
                
                { "Russian", new char[] { 'ё', 'э', 'ы', 'ъ' } },
                
                { "German", new char[] { 'ä', 'ö', 'ü', 'ß' } }
            };

            // Define language-specific dictionaries of popular words
            Dictionary<string, string[]> languageDictionaries = new Dictionary<string, string[]>
            {
                { "English", new string[] { "the", "and", "is", "of", "in", "to", "i", "you", "he", "she", "it", "we", "they", "hello" } },
                
                { "French", new string[] { "le", "la", "et", "est", "en", "que", "je", "tu", "il", "elle", "nous", "vous", "ils", "elles" } },
                
                { "German", new string[] { "die", "und", "ist", "in", "zu", "es", "ich", "du", "er", "sie", "es", "wir", "ihr", "sie" } },
                
                { "Spanish", new string[] { "el", "la", "y", "es", "en", "que", "yo", "tú", "él", "ella", "usted", "nosotros", "vosotros", "ellos", "ellas", "ustedes" } },
                
                { "Ukrainian", new string[] { "і", "не", "це", "на", "за", "до", "я", "ти", "він", "вона", "воно", "ми", "ви", "вони" } },
                
                { "Russian", new string[] { "и", "в", "не", "на", "что", "с", "я", "ты", "он", "она", "оно", "мы", "вы", "они" } }
            };

            // Count letters, letter combinations, characters, and dictionary words for each language
            Dictionary<string, int> languageScores = new Dictionary<string, int>();

            foreach (var language in languageLetters.Keys)
            {
                int score = CalculateScore(inputText, language, languageLetters, languageLetterCombinations, languageCharacters, languageDictionaries);
                languageScores[language] = score;
                Console.WriteLine(score);
            }

            // Determine the detected language
            string detectedLanguage = AnalyzeScores(languageScores);

            return detectedLanguage;
        }

        static int CalculateScore(string inputText, string language, Dictionary<string, string> languageLetters, Dictionary<string, string[]> languageLetterCombinations, Dictionary<string, char[]> languageCharacters, Dictionary<string, string[]> languageDictionaries)
        {
            int score = 0;
            
            
            foreach (var character in inputText)
            {
                // Check for symbols or numbers
                if (!char.IsLetter(character) && !char.IsWhiteSpace(character))
                {
                    return definitelyNotPossible;
                }

                // Check language-specific letters
                if (score != definitelyCorrect)
                {
                    if (IsLetterValidForLanguage(character, language, languageCharacters))
                    {
                        score = definitelyCorrect;
                    }
                    else if (!(languageLetters[language].Contains(char.ToLower(character).ToString())) && !char.IsWhiteSpace(character))
                    {   
                        score = definitelyNotPossible;
                    }
                }
            }

            //popular words check 
            foreach (var dictWord in languageDictionaries.GetValueOrDefault(language, new string[] { }))
            {
                if (score != definitelyCorrect && score != definitelyNotPossible)
                {
                    string pattern = @"\b" + Regex.Escape(dictWord.ToLower()) + @"\b";
                    if (Regex.IsMatch(inputText.ToLower(), pattern))
                    {
                        Console.WriteLine (language);
                        score = definitelyCorrect;
                        return score;
                    }
                }
            }

            
            // Two or three letter combinations check
            foreach (var combination in languageLetterCombinations[language])
            {
                if (score != definitelyCorrect && score != definitelyNotPossible)
                {
                    string pattern = @"\b" + Regex.Escape(combination.ToLower()) + @"\b";
                    
                    Console.WriteLine(pattern);
                    
                    int combinationCount = Regex.Matches(inputText.ToLower(), pattern).Count;

                    if (combinationCount > 0)
                    {
                        score = combinationCount;
                    }
                } 
            }              

            return score;
        }

        static string AnalyzeScores(Dictionary<string, int> languageScores)
        {
            string detectedLanguage = "Unknown";
            int maxScore = 0;
            bool multipleLanguages = false;
            bool languageDetected = false;

            foreach (var language in languageScores.Keys)
            {   
                if (languageScores[language] == definitelyCorrect && (languageDetected == false))
                {
                    detectedLanguage = language;
                    languageDetected = true;
                }
                else if (languageScores[language] == definitelyCorrect){
                    multipleLanguages = true;
                }
                
                if (languageScores[language] > maxScore && languageScores[language] != definitelyNotPossible && !(languageDetected))
                {
                    maxScore = languageScores[language];
                    detectedLanguage = language;
                }
            }

            if (multipleLanguages)
            {
                detectedLanguage = "Text is not written in one language";
            }

            if (maxScore == 0 && languageDetected == false)
            {
                detectedLanguage = "Language couldn't be identified";
            }

            return detectedLanguage;
        }

        static bool IsLetterValidForLanguage(char letter, string language, Dictionary<string, char[]> languageCharacters)
        {
            char[] validCharacters = languageCharacters.GetValueOrDefault(language, new char[0]);
    
            foreach (char validChar in validCharacters)
            {
                if (char.ToLower(validChar) == char.ToLower(letter))
                {   Console.WriteLine (language);
                    return true;
                }
            }
    
            return false;
        }
    }
}
