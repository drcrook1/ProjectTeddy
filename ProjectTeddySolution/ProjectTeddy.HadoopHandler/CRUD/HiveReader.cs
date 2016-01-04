using ProjectTeddy.FSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Hadoop.Hive;

namespace ProjectTeddy.HadoopHandler
{
    public class HiveReader : IHadoopReader
    {
        public IEnumerable<FSWordRelationship> FindRelationsWithWord(string word)
        {
            var query = from o in HadoopContext.WordRelationsTable
                        where o.rScore > 0 && (o.WordOne.CompareTo(word) == 0 || o.WordTwo.CompareTo(word) == 0)
                        select new { o.Id, o.WordOne, o.WordOneId, o.WordTwo, o.WordTwoId, o.rScore };
            query.ExecuteQuery().Wait();
            var results = query.ToList();
            List<FSWordRelationship> relations = new List<FSWordRelationship>();
            foreach (var r in results)
            {
                FSWordRelationship relation = new FSWordRelationship(r.Id, r.WordOne, r.WordOneId, r.WordTwo, r.WordTwoId, r.rScore);
                relations.Add(relation);
            }
            return relations;
        }

        public IEnumerable<FSWordRelationship> ReadWordRelationshipRange(int startId, int endId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FSTweet> ReadAllTweets()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FSTweet> ReadTweetsRange(DateTime startTime, int numYouWant)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FSAnnotatedTweet> ReadAllAnnotatedTweets()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FSAnnotatedTweet> ReadAnnotatedTweetsRange(DateTime startTime, int numYouWant)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FSConversation> ReadAllConversations()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FSConversation> ReadConversationsRange(DateTime startTime, int numYouWant)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FSAnnotatedConversation> ReadAllAnnotatedConversations()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FSAnnotatedConversation> ReadAnnotatedConversationsRange(DateTime startTime, int numYouWant)
        {
            throw new NotImplementedException();
        }
    }
}
