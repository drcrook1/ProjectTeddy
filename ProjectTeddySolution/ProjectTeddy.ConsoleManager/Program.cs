using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectTeddy.BlobProvider;
using ProjectTeddy.CorrelationBuilder;

namespace ProjectTeddy.ConsoleManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            //CorrelationBuilder.WordsDB.CreateWordRelationDB();
            SQLSeeder.MineTwitter("football");
            while(true)
            {

            }
            //BlobProvider.BlobProvider.ViewBlobs("spark");
            //SQLSeeder.SeedSQL(
            //    @".\Data\twitter_ids.validation.txt");
            Console.WriteLine("Done.");
            //Console.ReadKey();
        }
    }
}
