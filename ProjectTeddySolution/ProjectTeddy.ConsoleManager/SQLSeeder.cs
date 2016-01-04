using ProjectTeddy.Core.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Streaminvi;

namespace ProjectTeddy.ConsoleManager
{
    public static class SQLSeeder
    {
        public static void SeedSQL(string fileLocation)
        {
            using (var model = new AnnotatorModel())
            {
                var conversations = PopulateConversationsFromFile(fileLocation);
                foreach (Conversation c in conversations)
                {
                    model.Conversations.Add(c);
                    foreach (Core.EntityFramework.Tweet t in c.Tweets)
                    {
                        model.Tweets.Add(t);
                    }
                }
                model.SaveChanges();
            }
        }

        public static void MineTwitter(string term)
        {
            TwitterCredentials.SetCredentials(
                ConfigurationManager.AppSettings["AccessToken"],
                ConfigurationManager.AppSettings["AccessSecret"],
                ConfigurationManager.AppSettings["ConsumerKey"],
                ConfigurationManager.AppSettings["ConsumerSecret"]);
            IFilteredStream stream = Tweetinvi.Stream.CreateFilteredStream();
            stream.MatchingTweetReceived += Stream_TweetReceived;
            stream.AddTrack(term);
            stream.StartStreamMatchingAllConditions();
        }

        private static void Stream_TweetReceived(object sender, MatchedTweetReceivedEventArgs e)
        {
            if ( e.Tweet.InReplyToStatusId == null)
            {
                return;
            }
            ITweet r1 = Tweetinvi.Tweet.GetTweet((long)e.Tweet.InReplyToStatusId);
            if (r1 == null)
                return;
            Conversation c = new Conversation();
            c.Tweets.Add(InviTweetToPTTweet(e.Tweet));
            c.Tweets.Add(InviTweetToPTTweet(r1));
            using (var model = new AnnotatorModel())
            {
                Console.WriteLine(r1.Text);
                Console.WriteLine(e.Tweet.Text);
                model.Tweets.Add(InviTweetToPTTweet(r1));
                model.Tweets.Add(InviTweetToPTTweet(e.Tweet));
                model.Conversations.Add(c);
                model.SaveChanges();
            }
        }

        private static IEnumerable<Conversation> PopulateConversationsFromFile(string fileLocation)
        {
            List<Conversation> conversations = new List<Conversation>();
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
                List<ProjectTeddy.Core.EntityFramework.Tweet> tweets = new List<ProjectTeddy.Core.EntityFramework.Tweet>();
                foreach (string tweetId in tweetTriplet)
                {
                    long id = long.Parse(tweetId);
                    var t = Tweetinvi.Tweet.GetTweet(id);
                    if (t != null)
                    {
                        var ptTweet = InviTweetToPTTweet(t);
                        tweets.Add(ptTweet);
                        Console.WriteLine(ptTweet.Text);
                    }
                }
                if (tweets.Count > 0)
                {
                    Conversation c = new Conversation();
                    c.Tweets = tweets;
                    conversations.Add(c);
                }
            }
            #endregion getdata
            return conversations;
        }

        private static ProjectTeddy.Core.EntityFramework.Tweet InviTweetToPTTweet(ITweet t)
        {
            ProjectTeddy.Core.EntityFramework.Tweet ptTweet = new ProjectTeddy.Core.EntityFramework.Tweet();
            if (t.Coordinates != null)
            {
                ptTweet.Coordinates = t.Coordinates.Longitude.ToString() + ","
                    + t.Coordinates.Latitude.ToString();
            }
            ptTweet.CreatedOn = t.CreatedAt;
            ptTweet.ReplyToId = t.InReplyToUserIdStr;
            ptTweet.Text = t.Text;
            return ptTweet;
        }
    }
}
