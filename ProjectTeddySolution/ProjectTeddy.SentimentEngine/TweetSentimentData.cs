using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;

namespace ProjectTeddy.SentimentEngine
{
    public class TweetSentimentData
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string ReplyToId { get; set; }
        public string Coordinates { get; set; }
        public float Sentiment { get; set; }
        public DateTime CreatedOn { get; set; }
        public TweetSentimentData() { }
        public TweetSentimentData(ITweet tweet)
        {
            this.Id = tweet.IdStr;
            this.Text = tweet.Text;
            if (tweet.InReplyToStatusIdStr != null)
            {
                this.ReplyToId = tweet.InReplyToStatusIdStr;
            }
            else
            {
                this.ReplyToId = "";
            }
            this.CreatedOn = tweet.CreatedAt;
            if (tweet.Coordinates != null)
            {
                this.Coordinates = tweet.Coordinates.Longitude.ToString() + ","
                    + tweet.Coordinates.Latitude.ToString();
            }
            this.Sentiment = SentimentEngine.CalculateSentiment(this.Text);
        }
    }
}
