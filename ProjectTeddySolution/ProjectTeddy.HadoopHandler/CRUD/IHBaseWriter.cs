using ProjectTeddy.FSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTeddy.HadoopHandler
{
    public interface IHBaseWriter
    {
        void WriteWordRelation(FSWordRelationship relation);
        void WriteWordRelations(IEnumerable<FSWordRelationship> relations);
        void WriteTweet(FSTweet tweet);
        void WriteTweets(IEnumerable<FSTweet> tweets);
        void WriteAnnotatedTweet(FSAnnotatedTweet tweet);
        void WriteAnnotatedTweets(IEnumerable<FSAnnotatedTweet> tweets);
        void WriteConversation(FSConversation conv);
        void WriteConversations(IEnumerable<FSConversation> convos);
        void WriteAnnotatedConversation(FSAnnotatedConversation conv);
        void WriteAnnotatedConversations(IEnumerable<FSAnnotatedConversation> convos);
        void WriteWord(FSWord ptWord);
        void WriteWords(IEnumerable<FSWord> ptWords);
        void WriteAnnotatedWord(FSAnnotatedWord ptWord);
        void WriteAnnotatedWords(IEnumerable<FSAnnotatedWord> ptWords);
    }
}
