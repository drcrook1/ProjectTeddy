using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterStreamer
{
    public static class SentimentHelper
    {
        private static char[] _punctuationChars =
            new[] {
                ' ', '!', '\"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/',   //ascii 23--47
                ':', ';', '<', '=', '>', '?', '@', '[', ']', '^', '_', '`', '{', '|', '}', '~' };   //ascii 58--64 + misc.
        //// Sentiment dictionary file and the punctuation characters
        private const string DICTIONARYFILENAME = @".\data\dictionary.tsv";
        // a sentiment dictionary for estimate sentiment. It is loaded from a physical file.
        private static Dictionary<string, SentimentDictionaryItem> _dictionary { get; set; }
        public static Dictionary<string, SentimentDictionaryItem> Dictionary
        {
            get
            {
                if(_dictionary == null)
                {
                    _dictionary = LoadDictionary();
                    return _dictionary;
                }
                else{
                    return _dictionary;
                }
            }
        }
        public static float CalculateSentiment(string text)
        {
            string[] words = text.ToLower().Split(_punctuationChars);
            float sentiment = 0;
            float neutralChars = 0;
            foreach(string word in words)
            {
                SentimentDictionaryItem item = null;
                try
                {
                    item = Dictionary[word];
                }
                catch(Exception e) { neutralChars += 1;}
                if(item != null)
                {
                    if(item.Polarity.ToLower().CompareTo("positive") == 0)
                    {
                        sentiment += 1;
                    }
                    else if(item.Polarity.ToLower().CompareTo("negative") == 0)
                    {
                        sentiment -= 1;
                    }
                    else
                    {
                        neutralChars += 1;
                    }
                }
            }
            //normalize
            float divisor = words.Length - neutralChars;
            if (divisor < 1)
            {
                return 0;
            }
            return sentiment / divisor;
        }

        // Load sentiment dictionary from a file
        private static Dictionary<string, SentimentDictionaryItem> LoadDictionary()
        {
            List<string> lines = File.ReadAllLines(DICTIONARYFILENAME).ToList();
            var items = lines.Select(line =>
            {
                var fields = line.Split('\t');
                var pos = 0;
                return new SentimentDictionaryItem
                {
                    Type = fields[pos++],
                    Length = Convert.ToInt32(fields[pos++]),
                    Word = fields[pos++],
                    Pos = fields[pos++],
                    Stemmed = fields[pos++],
                    Polarity = fields[pos++]
                };
            });

            var SDictionary = new Dictionary<string, SentimentDictionaryItem>();
            foreach (var item in items)
            {
                if (!SDictionary.Keys.Contains(item.Word))
                {
                    SDictionary.Add(item.Word, item);
                }
            }
            return SDictionary;
        }
    }
}
