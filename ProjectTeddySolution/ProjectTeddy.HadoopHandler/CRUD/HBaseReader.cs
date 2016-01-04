using Microsoft.HBase.Client;
using org.apache.hadoop.hbase.rest.protobuf.generated;
using ProjectTeddy.FSCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTeddy.HadoopHandler
{
    public class HBaseReader : IHadoopReader
    {
        // For writting to HBase
        public HBaseClient client;
        //HDinsight HBase cluster and HBase table information
        public string ClusterName { get; set; }
        public string HadoopUserName { get; set; }

        public HBaseReader()
        {
            //Get the Hadoop Cluster info and create connection
            this.ClusterName = ConfigurationManager.AppSettings["ClusterName"];
            this.HadoopUserName = ConfigurationManager.AppSettings["HadoopUserName"];
            string HadoopUserPassword = ConfigurationManager.AppSettings["HadoopUserPassword"];
            SecureString pw = new SecureString();
            for (int i = 0; i < HadoopUserPassword.Length; i++)
            {
                pw.InsertAt(i, HadoopUserPassword[i]);
            }
            Uri clusterUri = new Uri(this.ClusterName);
            ClusterCredentials creds = new ClusterCredentials(clusterUri, this.HadoopUserName, pw);
            this.client = new HBaseClient(creds);
        }

        public IEnumerable<FSWordRelationship> ReadAllWordRelationships()
        {
            Scanner s = new Scanner()
            {
                batch = 10
            };
            ScannerInformation si = client.CreateScanner(HadoopContext.WordRelationTableName, s);
            CellSet next = null;
            CellSet readRows = new CellSet();
            while ((next = client.ScannerGetNext(si)) != null)
            {
                foreach (CellSet.Row row in next.rows)
                {
                    //convert row into desired domain type....
                    readRows.rows.Add(row);
                }
            }

            return null;
        }
        public IEnumerable<FSWordRelationship> ReadWordRelationshipRange(int startId, int endId)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<FSTweet> ReadAllTweets()
        {
            Scanner s = new Scanner()
            {
                batch = 10
            };
            ScannerInformation si = client.CreateScanner(HadoopContext.TweetTableName, s);
            CellSet next = null;
            while((next = client.ScannerGetNext(si)) != null)
            {
                foreach(CellSet.Row row in next.rows)
                {
                    
                }
            }
            return null;
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

        public IEnumerable<FSWordRelationship> FindRelationsWithWord(string word)
        {
            Scanner s = new Scanner()
            {
                batch = 1000
            };
            ScannerInformation si = client.CreateScanner(HadoopContext.HBaseWordRelationTableName, s);
            CellSet next = null;
            CellSet readRows = new CellSet();
            List<FSWordRelationship> relations = new List<FSWordRelationship>();
            while ((next = client.ScannerGetNext(si)) != null)
            {
                foreach (CellSet.Row row in next.rows)
                {
                    //convert row into desired domain type....
                    var w1 = row.values.Find(w => Encoding.UTF8.GetString(w.column) == "d:WordOne");
                    string wordOne = Encoding.UTF8.GetString(w1.data);
                    var w1Id = row.values.Find(w => Encoding.UTF8.GetString(w.column) == "d:WordOneId");
                    int wordOneId = Convert.ToInt32(Encoding.UTF8.GetString(w1Id.data));
                    var w2 = row.values.Find(w => Encoding.UTF8.GetString(w.column) == "d:WordTwo");
                    string wordTwo = Encoding.UTF8.GetString(w2.data);
                    var w2Id = row.values.Find(w => Encoding.UTF8.GetString(w.column) == "d:WordTwoId");
                    int wordTwoId = Convert.ToInt32(Encoding.UTF8.GetString(w2Id.data));
                    var rS = row.values.Find(w => Encoding.UTF8.GetString(w.column) == "d:RScore");
                    var rScore = Convert.ToDouble(Encoding.UTF8.GetString(rS.data));
                    var id = Encoding.UTF8.GetString(row.key);
                    if (word.CompareTo(wordOne) == 0 || word.CompareTo(wordTwo) == 0
                        && !(String.IsNullOrWhiteSpace(wordOne) || String.IsNullOrWhiteSpace(wordTwo)))
                    {
                        FSWordRelationship rel = new FSWordRelationship(id, wordOne, wordOneId, wordTwo, wordTwoId, rScore);
                        relations.Add(rel);
                    }
                }
            }
            return relations;
        }
    }
}
