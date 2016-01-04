using Microsoft.HBase.Client;
using org.apache.hadoop.hbase.rest.protobuf.generated;
using ProjectTeddy.SentimentEngine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectTeddy.HaddopHandler
{
    public class HBaseWriter
    {
        // use multithread write
        private Thread WriterThread;
        private Queue<TweetSentimentData> WriteQueue = new Queue<TweetSentimentData>();
        private bool ThreadRunning = true;
        // For writting to HBase
        public HBaseClient client;
        //HDinsight HBase cluster and HBase table information
        public string ClusterName { get; set; }
        public string HadoopUserName { get; set; }
        public string HBaseTableName { get; set; }
        public HBaseWriter(string tableName)
        {
            //Get the Hadoop Cluster info and create connection
            this.ClusterName = ConfigurationManager.AppSettings["ClusterName"];
            this.HadoopUserName = ConfigurationManager.AppSettings["HadoopUserName"];
            string HadoopUserPassword = ConfigurationManager.AppSettings["HadoopUserPassword"];
            this.HBaseTableName = tableName;
            SecureString pw = new SecureString();
            for (int i = 0; i < HadoopUserPassword.Length; i++)
            {
                pw.InsertAt(i, HadoopUserPassword[i]);
            }
            Uri clusterUri = new Uri(this.ClusterName);
            ClusterCredentials creds = new ClusterCredentials(clusterUri, this.HadoopUserName, pw);
            this.client = new HBaseClient(creds);
            //create table and enable the hbase writer
            if (!client.ListTables().name.Contains(this.HBaseTableName))
            {
                // Create the table
                var tableSchema = new TableSchema();
                tableSchema.name = this.HBaseTableName;
                tableSchema.columns.Add(new ColumnSchema { name = "d" });
                client.CreateTable(tableSchema);
                Console.WriteLine("Table \"{0}\" created.", this.HBaseTableName);
            }
            WriterThread = new Thread(new ThreadStart(WriterThreadFunction));
            WriterThread.Start();
        }
        // Enqueue the Tweets received
        public void WriteTweet(TweetSentimentData tweet)
        {
            lock (this.WriteQueue)
            {
                this.WriteQueue.Enqueue(tweet);
            }
        }
        // Popular a CellSet object to be written into HBase
        private void CreateTweetByWordsCells(CellSet set, TweetSentimentData tweet)
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
            row.values.Add(
                new Cell
                {
                    column = Encoding.UTF8.GetBytes("d:ReplyToId"),
                    data = Encoding.UTF8.GetBytes(tweet.ReplyToId)
                });
            row.values.Add(
                new Cell
                {
                    column = Encoding.UTF8.GetBytes("d:Sentiment"),
                    data = Encoding.UTF8.GetBytes(tweet.Sentiment.ToString())
                });
            if (tweet.Coordinates != null)
            {
                row.values.Add(
                    new Cell
                    {
                        column = Encoding.UTF8.GetBytes("d:Coordinates"),
                        data = Encoding.UTF8.GetBytes(tweet.Coordinates)
                    });
            }
            set.rows.Add(row);
        }
        // Write a Tweet (CellSet) to HBase
        public void WriterThreadFunction()
        {
            while (ThreadRunning)
            {
                if (WriteQueue.Count > 0)
                {
                    CellSet set = new CellSet();
                    lock (WriteQueue)
                    {
                        do
                        {
                            TweetSentimentData tweet = WriteQueue.Dequeue();
                            CreateTweetByWordsCells(set, tweet);
                        } while (WriteQueue.Count > 0);
                    }
                    // Write the Tweet by words cell set to the HBase table
                    client.StoreCells(this.HBaseTableName, set);
                    Console.WriteLine("\tRows written: {0}", set.rows.Count);
                }
            }
        }
    }
}
