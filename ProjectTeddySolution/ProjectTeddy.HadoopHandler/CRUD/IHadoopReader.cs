using ProjectTeddy.FSCore;
using ProjectTeddy.SentimentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Hadoop.Hive;

namespace ProjectTeddy.HadoopHandler
{
    public interface IHadoopReader
    {
        IEnumerable<FSWordRelationship> FindRelationsWithWord(string word);
        IEnumerable<FSWordRelationship> ReadWordRelationshipRange(int startId, int endId);
        IEnumerable<FSTweet> ReadAllTweets();
        IEnumerable<FSTweet> ReadTweetsRange(DateTime startTime, int numYouWant);
        IEnumerable<FSAnnotatedTweet> ReadAllAnnotatedTweets();
        IEnumerable<FSAnnotatedTweet> ReadAnnotatedTweetsRange(DateTime startTime, int numYouWant);
        IEnumerable<FSConversation> ReadAllConversations();
        IEnumerable<FSConversation> ReadConversationsRange(DateTime startTime, int numYouWant);
        IEnumerable<FSAnnotatedConversation> ReadAllAnnotatedConversations();
        IEnumerable<FSAnnotatedConversation> ReadAnnotatedConversationsRange(DateTime startTime, int numYouWant);
    }
}
