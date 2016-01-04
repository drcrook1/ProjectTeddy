using org.apache.hadoop.hbase.rest.protobuf.generated;
using ProjectTeddy.FSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectTeddy.HadoopHandler
{
    public class PTHBaseWriter : IHBaseWriter
    {
        public void WriteTweet(FSTweet tweet)
        {
            CellSet.Row row = TableDomainMappers.TweetToRow(tweet);
            CellSet set = new CellSet();
            set.rows.Add(row);
            HadoopContext.HBaseClient.StoreCells(HadoopContext.TweetTableName, set);
        }

        public void WriteWordRelation(FSWordRelationship relation)
        {
            CellSet.Row row = TableDomainMappers.WordRelationToRow(relation);
            CellSet set = new CellSet();
            set.rows.Add(row);
            HadoopContext.HBaseClient.StoreCells(HadoopContext.WordRelationTableName, set);
        }

        public void WriteWordRelations(IEnumerable<FSWordRelationship> relations)
        {
            CellSet set = new CellSet();
            foreach(FSWordRelationship relation in relations)
            {
                set.rows.Add(TableDomainMappers.WordRelationToRow(relation));
            }
            HadoopContext.HBaseClient.StoreCells(HadoopContext.WordRelationTableName, set);
        }

        public void WriteTweets(IEnumerable<FSTweet> tweets)
        {
            CellSet set = new CellSet();
            foreach(FSTweet t in tweets)
            {
                set.rows.Add(TableDomainMappers.TweetToRow(t));
            }
            HadoopContext.HBaseClient.StoreCells(HadoopContext.TweetTableName, set);
        }

        public void WriteAnnotatedTweet(FSAnnotatedTweet tweet)
        {
            throw new NotImplementedException();
        }

        public void WriteAnnotatedTweets(IEnumerable<FSAnnotatedTweet> tweets)
        {
            throw new NotImplementedException();
        }

        public void WriteConversation(FSConversation convo)
        {
            CellSet.Row row = TableDomainMappers.ConversationToRow(convo);
            CellSet set = new CellSet();
            set.rows.Add(row);
            HadoopContext.HBaseClient.StoreCells(HadoopContext.ConversationTableName, set);
            Thread.Sleep(100);
        }

        public void WriteConversations(IEnumerable<FSConversation> convos)
        {
            throw new NotImplementedException();
        }

        public void WriteAnnotatedConversation(FSAnnotatedConversation conv)
        {
            throw new NotImplementedException();
        }

        public void WriteAnnotatedConversations(IEnumerable<FSAnnotatedConversation> convos)
        {
            throw new NotImplementedException();
        }

        public void WriteWord(FSWord ptWord)
        {
            throw new NotImplementedException();
        }

        public void WriteWords(IEnumerable<FSWord> ptWords)
        {
            throw new NotImplementedException();
        }

        public void WriteAnnotatedWord(FSAnnotatedWord ptWord)
        {
            throw new NotImplementedException();
        }

        public void WriteAnnotatedWords(IEnumerable<FSAnnotatedWord> ptWords)
        {
            throw new NotImplementedException();
        }
    }
}
