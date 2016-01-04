using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events.EventArguments;

namespace TwitterStreamer
{
    public class Program
    {
        public static HBaseWriter hbaseWriter;
        static void Main(string[] args)
        {
            hbaseWriter = new HBaseWriter();
            string consumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
            string consumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];
            string accessToken = ConfigurationManager.AppSettings["AccessToken"];
            string accessSecret = ConfigurationManager.AppSettings["AccessSecret"];
            TwitterCredentials.SetCredentials(accessToken, accessSecret, consumerKey, consumerSecret);
            Task.Run(() =>
            {
                StreamTwitter();
            });
            while (true) ;
        }
        private static void StreamTwitter()
        {
            var sampleStream = Stream.CreateSampleStream();
            sampleStream.AddTweetLanguageFilter(Language.English);
            sampleStream.TweetReceived += ReceiveTweet;
            sampleStream.StartStream();
        }
        private static void ReceiveTweet(object sender, TweetReceivedEventArgs args)
        {            
            if(hbaseWriter != null)
            {
                TweetSentimentData data = new TweetSentimentData(args.Tweet);
                hbaseWriter.WriteTweet(data);
                Console.WriteLine(args.Tweet.Text);
            }
        }

        //TODO: Write Code //Joe
    }
}
