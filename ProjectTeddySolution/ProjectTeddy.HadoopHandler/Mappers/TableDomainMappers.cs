using org.apache.hadoop.hbase.rest.protobuf.generated;
using ProjectTeddy.FSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTeddy.HadoopHandler
{
    public static class TableDomainMappers
    {
        public static CellSet.Row TweetToRow(FSTweet tweet)
        {
            // Create a row with a key
            var row = new CellSet.Row { key = Encoding.UTF8.GetBytes(tweet.Id) };
            // Add columns to the row
            row.values.Add(
                new Cell
                {
                    column = Encoding.UTF8.GetBytes("d:Text"),
                    data = Encoding.UTF8.GetBytes(tweet.Text)
                });
            row.values.Add(
                new Cell
                {
                    column = Encoding.UTF8.GetBytes("d:CreatedOn"),
                    data = Encoding.UTF8.GetBytes(tweet.CreatedOn.ToString())
                });
            if (tweet.ReplyToId != null)
            {
                row.values.Add(
                    new Cell
                    {
                        column = Encoding.UTF8.GetBytes("d:ReplyToId"),
                        data = Encoding.UTF8.GetBytes(tweet.ReplyToId)
                    });
            }
            if (tweet.Coordinates != null)
            {
                row.values.Add(
                    new Cell
                    {
                        column = Encoding.UTF8.GetBytes("d:Coordinates"),
                        data = Encoding.UTF8.GetBytes(tweet.Coordinates)
                    });
            }
            return row;
        }
        public static CellSet.Row AnnotatedTweetToRow(FSAnnotatedTweet tweet)
        {
            // Create a row with a key
            var row = new CellSet.Row { key = Encoding.UTF8.GetBytes(tweet.Id) };
            // Add columns to the row
            row.values.Add(
                new Cell
                {
                    column = Encoding.UTF8.GetBytes("d:TweetId"),
                    data = Encoding.UTF8.GetBytes(tweet.TweetId)
                });
            row.values.Add(
                new Cell
                {
                    column = Encoding.UTF8.GetBytes("d:AnnotatedBy"),
                    data = Encoding.UTF8.GetBytes(tweet.AnnotatedBy)
                });
            row.values.Add(
                new Cell
                {
                    column = Encoding.UTF8.GetBytes("d:AnnotatedOn"),
                    data = Encoding.UTF8.GetBytes(tweet.AnnotatedOn.ToString())
                });
            row.values.Add(
                new Cell
                {
                    column = Encoding.UTF8.GetBytes("d:SentenceType"),
                    data = Encoding.UTF8.GetBytes(tweet.SentenceType.ToString())
                });
            row.values.Add(
                new Cell
                {
                    column = Encoding.UTF8.GetBytes("d:Sentiment"),
                    data = Encoding.UTF8.GetBytes(tweet.Sentiment.ToString())
                });
            return row;
        }
        public static CellSet.Row ConversationToRow(FSConversation convo)
        {
            var row = new CellSet.Row { key = Encoding.UTF8.GetBytes(convo.Id) };
            // Add columns to the row
            for (int i = 0; i < convo.Tweets.Count(); i++ )
            {
                row.values.Add(
                    new Cell
                    {
                        column = Encoding.UTF8.GetBytes("d:TweetId_"+i.ToString()),
                        data = Encoding.UTF8.GetBytes(convo.Tweets.ElementAt(i).Id)
                    });
            }
            return row;
        }
        public static CellSet.Row WordRelationToRow(FSWordRelationship relation)
        {
            var row = new CellSet.Row { key = Encoding.UTF8.GetBytes(relation.Id) };
            row.values.Add(
                new Cell
                {
                    column = Encoding.UTF8.GetBytes("d:WordOne" ),
                    data = Encoding.UTF8.GetBytes(relation.WordOne)
                });
            row.values.Add(
                new Cell
                {
                    column = Encoding.UTF8.GetBytes("d:WordOneId"),
                    data = Encoding.UTF8.GetBytes(relation.WordOneId.ToString())
                });
            row.values.Add(
                new Cell
                {
                    column = Encoding.UTF8.GetBytes("d:WordTwo"),
                    data = Encoding.UTF8.GetBytes(relation.WordTwo)
                });
            row.values.Add(
                new Cell
                {
                    column = Encoding.UTF8.GetBytes("d:WordTwoId"),
                    data = Encoding.UTF8.GetBytes(relation.WordTwoId.ToString())
                });
            row.values.Add(
                new Cell
                {
                    column = Encoding.UTF8.GetBytes("d:RScore"),
                    data = Encoding.UTF8.GetBytes(relation.rScore.ToString())
                });
            return row;
        }
    }
}
