using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTeddy
{
    class Program
    {
        static void Main(string[] args)
        {

            LoadMainMenu();

            //MicrosoftProjectOxfordExample.SpeechRecognitionServiceExample.Listen(args);

        }

        static void LoadMainMenu()
        {
            Console.WriteLine(" ");
            Console.WriteLine("--------------------------");
            Console.WriteLine("Hello! I am Project Teddy!");
            Console.WriteLine("--------------------------");
            Console.WriteLine("Available Options:");
            Console.WriteLine("[1] Speech to Text Engine");
            Console.WriteLine("[2] LUIS Speech Recog with Intent");
            Console.WriteLine("[3] LUIS Speech Recog WITHOUT Intent");
            Console.Write("Please enter an option:> ");

            int x = int.Parse(Console.ReadLine());

            if (x == 1) SayHello();
            if (x == 2) LoadLUIS(true);
            if (x == 3) LoadLUIS(false);
        }

        static void SayHello()
        {

            Console.WriteLine(" ");
            Console.WriteLine("--------------------------");
            Console.WriteLine(" Bing Speech to Text ");
            Console.WriteLine("--------------------------");

            Console.Write("Please tell me your name:> ");

            string userName = Console.ReadLine();

            string toSay = ("Hello," + userName + ", Nice to meet you!");

            TTSSample.Program.SpeakThis(toSay);

            ConvertTextToSpeech();

        }

        static void ConvertTextToSpeech()
        {
            Console.WriteLine(" ");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Please text for me to speak: ");
            Console.WriteLine("-----------------------------");
            Console.Write(":> ");
            string longTextToSay = Console.ReadLine();

            //check to see if we want to exit to main menu
            if (longTextToSay == "menu")
            {
                LoadMainMenu();
            }

            Console.WriteLine("-----------------------------");
            TTSSample.Program.SpeakThis(longTextToSay);

            ConvertTextToSpeech();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        static void LoadLUIS(bool Intent)
        {
            MicrosoftProjectOxfordExample.SpeechRecognitionServiceExample.Listen(Intent);

            LoadLUIS(Intent);
        }
        static void HelloWorld()
        {
            Console.WriteLine("HOLA!");


        }

    }
}
