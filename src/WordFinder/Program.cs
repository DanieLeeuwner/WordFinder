using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace WordFinder
{
  internal class Program
  {
    private static string word_file = "words.txt";
    private static string dictionary_file = "dictionary.txt";

    private static List<string> haystack;

    private static string selected_word;

    private static void Main(string[] args)
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
        else if (input.Key == ConsoleKey.Enter)
        {
          Console.Clear();
          Console.Write("Selected Word: ");
          PrintColor(ConsoleColor.Green, selected_word + '\n');
          Console.WriteLine("-----------------------------------");

          // find the meaning of the word
          if (File.Exists(dictionary_file))
          {
            StreamReader sr = new StreamReader(dictionary_file);
            string line = "";
            bool found = false;
            while (sr.EndOfStream == false)
            {
              line = sr.ReadLine();
              if (line.ToLower().StartsWith(selected_word.ToLower() + " "))
              {
                found = true;
                if (!String.IsNullOrEmpty(line))
                {
                  var parts = line.Split('.');
                  foreach (var p in parts)
                  {
                    Console.WriteLine(p);
                  }
                }
                break;
              }
            }
            if (found == false)
            {
              PrintColor(ConsoleColor.Red, "--- Missing Dictionary Entry ---");
            }
          }
          else
          {
            PrintColor(ConsoleColor.Red, "--- Missing Dictionary File ---");
          }

          continue;
        }
        else
        {
          needle += input.KeyChar;
        }
        
        var regex = new Regex(needle, RegexOptions.IgnoreCase);

        var filtered_words = haystack.Where(r => regex.IsMatch(r)).Take(25);
        selected_word = filtered_words.OrderBy(x => x.Length).FirstOrDefault();
        Console.Clear();

        Console.WriteLine($"Search: {needle}");
        Console.WriteLine("-----------------------------------");

        if (filtered_words.Count() == 0)
        {
          PrintColor(ConsoleColor.Red, "--- No Words Found ---");
        }

        foreach (string fw in filtered_words)
        {
          int start_index = fw.IndexOf(needle);

          string before = fw.Substring(0, start_index);

          string after = fw.Substring(start_index + needle.Length);

          Console.Write(before);

          PrintColor(ConsoleColor.Green, needle);

          Console.Write(after + "\n");
        }
      }
    }

    private static void PrintColor(ConsoleColor color, string v)
    {
      var prevColor = Console.ForegroundColor;

      Console.ForegroundColor = color;

      Console.Write(v);

      Console.ForegroundColor = prevColor;
    }
  }
}