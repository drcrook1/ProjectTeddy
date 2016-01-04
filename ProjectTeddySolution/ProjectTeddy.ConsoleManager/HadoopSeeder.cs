using ProjectTeddy.FSCore;
using ProjectTeddy.HadoopHandler;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;

namespace ProjectTeddy.ConsoleManager
{
    public class HadoopSeeder
    {
        public static void SeedHadoop(string fileLocation)
        {
            PTHBaseWriter writer = new PTHBaseWriter();
            var conversations = PopulateConversationsFromFile(fileLocation);
            foreach(FSConversation c in conversations)
            {
                writer.WriteConversation(c);
                foreach(FSTweet t in c.Tweets)
                {
                    writer.WriteTweet(t);
                }
                Console.WriteLine("Wrote Conversation: " + c.Id);
            }
        }
        private static IEnumerable<FSConversation> PopulateConversationsFromFile(string fileLocation)
        {
            List<FSConversation> conversations = new List<FSConversation>();
            var data = File.ReadAllLines(fileLocation);
            TwitterCredentials.SetCredentials(
                ConfigurationManager.AppSettings["AccessToken"],
                ConfigurationManager.AppSettings["AccessSecret"],
                ConfigurationManager.AppSettings["ConsumerKey"],
                ConfigurationManager.AppSettings["ConsumerSecret"]);
            #region getdata
            foreach (string s in data)
            {
                var tweetTriplet = s.Split("\t".ToCharArray());
                List<FSTweet> tweets = new List<FSTweet>();
                foreach(string tweetId in tweetTriplet)
                {
                    long id = long.Parse(tweetId);
                    var t = Tweetinvi.Tweet.GetTweet(id);
                    if (t != null)
                    {
                        var ptTweet = InviTweetToPTTweet(t);
                        tweets.Add(ptTweet);
                    }
                }
                if(tweets.Count > 0)
                {
                    FSConversation c = new FSConversation();
                    Guid g = Guid.NewGuid();
                    c.Id = g.ToString();
                    c.Tweets = tweets;
                    conversations.Add(c);
                    PTHBaseWriter writer = new PTHBaseWriter();
                    writer.WriteConversation(c);
                }
            }
            #endregion getdata
            return conversations;
        }

        private static FSTweet InviTweetToPTTweet(ITweet t)
        {
            FSTweet ptTweet = new FSTweet();
            if (t.Coordinates != null)
            {
                ptTweet.Coordinates = t.Coordinates.Longitude.ToString() + ","
                    + t.Coordinates.Latitude.ToString();
            }
            ptTweet.CreatedOn = t.CreatedAt;
            ptTweet.Id = t.Id.ToString();
            ptTweet.ReplyToId = t.InReplyToUserIdStr;
            ptTweet.Text = t.Text;
            return ptTweet;
        }

    }
}
