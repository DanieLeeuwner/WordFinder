using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordFinder
{
    class Program
    {
        static string word_file = "words.txt";


        static List<string> haystack;        

        static void Main(string[] args)
        {
            if (!File.Exists(word_file))
            {
                Console.Write("Word list not found");
                return;
            }

            Console.Write("Search: ");

            string needle = "";

            haystack = File.ReadAllLines(word_file).ToList();

            while (true)
            {
                var input = Console.ReadKey();

                if (input.Key == ConsoleKey.Backspace)
                {
                    if (needle.Length > 0)
                    {
                        needle = needle.Substring(0, needle.Length - 1);
                    }
                }
                else
                {
                    needle += input.KeyChar;
                }

                var filtered_words = haystack.Where(hw => hw.Contains(needle)).Take(25);

                Console.Clear();

                Console.WriteLine($"Search: {needle}");
                Console.WriteLine("-----------------------------------");

                if (filtered_words.Count() == 0)
                {
                    var prevColor = Console.ForegroundColor;

                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine("--- No Words Found ---");

                    Console.ForegroundColor = prevColor;
                }

                foreach (string fw in filtered_words)
                {
                    int start_index = fw.IndexOf(needle);

                    string before = fw.Substring(0, start_index);

                    string after = fw.Substring(start_index + needle.Length);

                    Console.Write(before);

                    var prevColor = Console.ForegroundColor;

                    Console.ForegroundColor = ConsoleColor.Green;

                    Console.Write(needle);

                    Console.ForegroundColor = prevColor;
                    Console.Write(after + "\n");

                }

            }

        }
    }
}
