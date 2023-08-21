/*

Created on Fr July 14 19:53:09 2023

by Havryliuk Danylo, student of Charles University, educational program - Computer Science (Informatika)

SUMMER PROJECT

LANGUAGE RECOGNITION PROGRAM

*/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LanguageRecognition
{
    class Program
    {
        // Constants used to define special score values
        const int definitelyCorrect = -1;
        const int definitelyNotPossible = -2;
        
        static void Main(string[] args)
        {
            bool goodInput;
            
            Console.WriteLine("Welcome to Language Recognition Program!");
            
            while (true) //continuous input
           {    
                Console.WriteLine("Enter the text you want to recognize (or type 'exit' to quit):");
                
                string inputText = Console.ReadLine();
                goodInput = true;
                
                foreach (var character in inputText)
                {
                // Check for symbols or numbers
                    if (!char.IsLetter(character) && !char.IsWhiteSpace(character) && character != '.' && character != ',' && character != '?' && character != '¿' && character != '!')
                    {
                        Console.WriteLine("Invalid input. Please enter some text.");
                        goodInput = false;
                        break;
                    }
                }
                if (goodInput)
                {
                    if (inputText.ToLower() == "exit")
                    {
                        break; // Exit the loop if the user types 'exit'
                    }
                
                    // Check if the input is not empty or whitespace
                    if (!string.IsNullOrWhiteSpace(inputText))
                    {          
                        string detectedLanguage = RecognizeLanguage(inputText); // Recognize the language of the input text
                        Console.WriteLine($"Detected Language: {detectedLanguage}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter some text.");
                    }
                }     
           }
            
            Console.WriteLine("Goodbye!");
        }


        static string RecognizeLanguage(string inputText)
        {
            // Define language-specific letters
            Dictionary<string, string> languageLetters = new Dictionary<string, string>
            {
                { "English", "abcdefghijklmnopqrstuvwxyz.,?!" },
                
                { "French", "abcdefghijklmnopqrstuvwxyzàâæçéèêëîïôœùûüÿ.,?!" },
                
                { "German", "abcdefghijklmnopqrstuvwxyzäöüß.,?!" },
                
                { "Spanish", "abcdefghijklmnopqrstuvwxyzáéíóúñ.,?¿!" },
                
                { "Ukrainian", "абвгдеєжзиіїйклмнопрстуфхцчшщьюя.,?!" },
                
                { "Russian", "абвгдеёжзийклмнопрстуфхцчшщъыьэюя.,?!" }
            };

            // Define language-specific digraphs and trigraphs
            Dictionary<string, string[]> languageLetterCombinations = new Dictionary<string, string[]>
            {
                { "English", new string[] { "th", "he", "an", "in", "er", "on", "ll", "sh", "ch", "wh" } },
    
                { "French", new string[] {"le", "la", "et", "en", "au", "ou", "ui", "est", "un" } },
    
                { "German", new string[] { "sch", "ie", "ei", "en", "ch", "st", "sp", "ie" } },
    
                { "Spanish", new string[] {"la", "el", "que", "en", "del", "ch", "ll" } },
    
                { "Ukrainian", new string[] { "жд", "пр", "ко", "те", "бе", "мо", "по" } },
    
                { "Russian", new string[] { "ст", "но",  "ен", "ов", "ых", "зд", "чь" } }
            };

            //Define less popular language-specific digraphs and trigraphs
            Dictionary<string, string[]> MoreRarelylanguageLetterCombinations = new Dictionary<string, string[]>
            {
                { "English", new string[] { "st", "re", "nt", "en", "tion", "ing" } },
    
                { "French", new string[] { "re", "te", "es", "on", "oi", "ai", "il", "ue" } },
    
                { "German", new string[] { "un", "an", "in", "au", "be", "ne", "sc", "ma" } },
    
                { "Spanish", new string[] { "es", "de", "te", "re", "ra", "er", "an" } },
    
                { "Ukrainian", new string[] { "ал", "ол", "на", "ні", "ро", "рі", "ла", "лі", "ло", "ля"} },
    
                { "Russian", new string[] { "то",  "ор", "ил", "ин", "ос", "та", "ул" } }
            };        
            
            
            // Define language-specific characters
            Dictionary<string, char[]> languageCharacters = new Dictionary<string, char[]>
            {
                { "French", new char[] { 'à', 'â', 'æ', 'ç', 'é', 'è', 'ê', 'ë', 'î', 'ô', 'œ', 'ù', 'û', 'ü', 'ÿ' } },
                
                { "Spanish", new char[] { 'á', 'é', 'í', 'ó', 'ú', 'ñ' } },
                
                { "Ukrainian", new char[] { 'є', 'ґ', 'і' } },
                
                { "Russian", new char[] { 'ё', 'э', 'ы', 'ъ' } },
                
                { "German", new char[] { 'ä', 'ö', 'ü', 'ß' } }
            };

            // Define language-specific dictionaries of popular words
            Dictionary<string, string[]> languageDictionaries = new Dictionary<string, string[]>
            {
                { "English", new string[] { "the", "and", "is", "of", "to", "you", "he", "she", "it", "we", "they", "are", "in", "i", "am"} },
                
                { "French", new string[] { "le", "la", "et", "est", "en", "que", "je", "tu", "il", "elle", "nous", "vous", "ils", "elles" } },
                
                { "German", new string[] { "die", "und", "ist", "zu", "ich", "du", "er", "sie", "wir", "ihr", "sie" } },
                
                { "Spanish", new string[] { "el", "y", "es", "en", "que", "yo", "tú", "él", "ella", "usted", "nosotros", "vosotros", "ellos", "ellas", "ustedes", "es" } },
                
                { "Ukrainian", new string[] { "і", "не", "це", "за", "до", "ти", "він", "вона", "воно", "ми", "ви", "вони" } },
                
                { "Russian", new string[] { "и", "в", "не", "что", "с", "ты", "он", "она", "оно", "мы", "вы", "они" } }
            };

            // Count  letter combinations for each language
            Dictionary<string, int> languageScores = new Dictionary<string, int>();

            foreach (var language in languageLetters.Keys)
            {
                int score = CalculateScore(inputText, language, languageLetters, languageLetterCombinations, MoreRarelylanguageLetterCombinations, languageCharacters, languageDictionaries);
                languageScores[language] = score;
            }

            // Determine the detected language
            string detectedLanguage = AnalyzeScores(languageScores);

            return detectedLanguage;
        }

        // Function to calculate the each language score
        static int CalculateScore(string inputText, string language, Dictionary<string, string> languageLetters, Dictionary<string, string[]> languageLetterCombinations, Dictionary<string, string[]> MoreRarelylanguageLetterCombinations, Dictionary<string, char[]> languageCharacters, Dictionary<string, string[]> languageDictionaries)
        {
            int score = 0;
            
            
            foreach (var character in inputText)
            {
                // Check for symbols or numbers
                if (!char.IsLetter(character) && !char.IsWhiteSpace(character) && character != '.' && character != ',' && character != '?' && character != '¿' && character != '!')
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

            // Check for popular words in the input text
            foreach (var dictWord in languageDictionaries.GetValueOrDefault(language, new string[] { }))
            {
                if (score != definitelyCorrect && score != definitelyNotPossible)
                {
                    // Create a pattern for the dictionary word and check if it's present in the input text
                    string pattern = @"\b" + Regex.Escape(dictWord.ToLower()) + @"\b";
                    
                    if (Regex.IsMatch(inputText.ToLower(), pattern))
                    {
                        score = definitelyCorrect; // Mark as definitely correct if a popular word is found
                        return score;
                    }
                }
            }
            

            
            // Check for language-specific letter combinations (digraphs and trigraphs)
            foreach (var combination in languageLetterCombinations[language])
            {
                if (score != definitelyCorrect && score != definitelyNotPossible)
                {
                    //Create a pattern for the combination and count its occurrences in the input text
                    string pattern = Regex.Escape(combination.ToLower());
                    
                    int combinationCount = Regex.Matches(inputText.ToLower(), pattern).Count;

                    if (combinationCount > 0)
                    {
                        score = combinationCount; // Set the score to the count of occurrences
                    }
                } 
            }        
            
            foreach (var combination in MoreRarelylanguageLetterCombinations[language])
            {
                if (score != definitelyCorrect && score != definitelyNotPossible)
                {
                    //Create a pattern for the combination and count its occurrences in the input text
                    string pattern = Regex.Escape(combination.ToLower());
                    
                    int combinationCount = Regex.Matches(inputText.ToLower(), pattern).Count;

                    if (combinationCount > 0)
                    {
                        score += combinationCount/2 + 1; // Set the score to the count of occurrences
                    }
                    
                } 
            }

            return score;
        }

        // Function to analyze language scores and determine the detected language
        static string AnalyzeScores(Dictionary<string, int> languageScores)
        {
            string detectedLanguage = "Unknown";
            int maxScore = 0;
            
            bool multipleLanguages = false;
            bool languageDetected = false;
            
            List<string> possibleLanguages = new List<string>();
            List<string> definitelyCorrectLanguages = new List<string>();
            

            foreach (var language in languageScores.Keys)
            {   
                if (languageScores[language] != definitelyCorrect && languageScores[language] != definitelyNotPossible)
                {
                possibleLanguages.Add(language); // Add the language to the list of possible languages
                }
                
                if (languageScores[language] == definitelyCorrect && (languageDetected == false))
                {
                    detectedLanguage = language;
                    languageDetected = true;
                    definitelyCorrectLanguages.Add(language);
                }
                
                else if (languageScores[language] == definitelyCorrect)
                {
                    multipleLanguages = true;
                    definitelyCorrectLanguages.Add(language);
                }
                
                if (languageScores[language] > maxScore && languageScores[language] != definitelyNotPossible && !(languageDetected))
                {
                    maxScore = languageScores[language];
                    detectedLanguage = language;
                }
            }
            
            if (definitelyCorrectLanguages.Count == 0) // Case if only 1 possible language remaning
            {
                if (possibleLanguages.Count == 1){
                    
                    languageDetected = true;
                    detectedLanguage = possibleLanguages[0];
                    return detectedLanguage;
                }
            }
            
            //Case if multiple languages are definitelyCorrect
            if (multipleLanguages)
            {   
                detectedLanguage = "Text could be written in multiple languages: ";
                detectedLanguage += string.Join(", ", definitelyCorrectLanguages);
                return detectedLanguage;
            }

            if (maxScore == 0 && languageDetected == false)
            {   if (possibleLanguages.Count != 0)
                {
                    detectedLanguage = "The program did not receive enough data to uniquely identify the language, possible languages: ";
                    detectedLanguage += string.Join(", ", possibleLanguages);
                }        
                else
                {
                    detectedLanguage = "Sorry, but this language is not supported by the program, yet:)";
                }
            }

            return detectedLanguage;
        }

        // Function to check if a letter is specific (that letter exists only in this language) for a given language  
        static bool IsLetterValidForLanguage(char letter, string language, Dictionary<string, char[]> languageCharacters)
        {
            char[] validCharacters = languageCharacters.GetValueOrDefault(language, new char[0]);
    
            foreach (char validChar in validCharacters)
            {
                if (char.ToLower(validChar) == char.ToLower(letter))
                {   
                    return true;
                }
            }
    
            return false;
        }
    }
}
