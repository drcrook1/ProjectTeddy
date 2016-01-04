using Microsoft.HBase.Client;
using org.apache.hadoop.hbase.rest.protobuf.generated;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Hadoop.Hive;
using ProjectTeddy.FSCore;
using ProjectTeddy.HadoopHandler.Entities;

namespace ProjectTeddy.HadoopHandler
{
    public static class HadoopContext
    {
        private static string _annotatedTweetTableName;
        private static string _tweetTableName;
        private static string _annotatedConversationTableName;
        private static string _conversationTableName;
        private static string _wordRelationTableName;
        private static string _hbaseWordRelationTableName;
        private static HBaseClient _hbaseClient;
        private static HiveConnection _hiveConnection;
        private static HiveTable<WordRelationshipHiveRow> _wordRelationsTable;
        public static HiveTable<WordRelationshipHiveRow> WordRelationsTable
        {
            get
            {
                if (_wordRelationsTable == null)
                {
                    _wordRelationsTable = HadoopContext.HiveConnection.GetTable<WordRelationshipHiveRow>(HadoopContext.WordRelationTableName);
                }
                return _wordRelationsTable;
            }
        }
        public static string WordRelationTableName
        {
            get
            {
                if (String.IsNullOrEmpty(_wordRelationTableName) || String.IsNullOrWhiteSpace(_wordRelationTableName))
                {
                    _wordRelationTableName = ConfigurationManager.AppSettings["WordRelationTableName"];
                }
                return _wordRelationTableName;
            }
        }
        public static string HBaseWordRelationTableName
        {
            get
            {
                if (String.IsNullOrEmpty(_hbaseWordRelationTableName) || String.IsNullOrWhiteSpace(_hbaseWordRelationTableName))
                {
                    _hbaseWordRelationTableName = ConfigurationManager.AppSettings["HBaseWordRelationTableName"];
                }
                return _hbaseWordRelationTableName;
            }
        }
        public static string AnnotatedTweetTableName
        {
            get
            {
                if (String.IsNullOrEmpty(_annotatedTweetTableName) || String.IsNullOrWhiteSpace(_annotatedTweetTableName))
                {
                    _annotatedTweetTableName = ConfigurationManager.AppSettings["AnnotatedTweetTableName"];
                    //checkCreateTable(_annotatedTweetTableName);
                }
                return _annotatedTweetTableName;
            }
        }
        public static string TweetTableName
        {
            get
            {
                if (String.IsNullOrEmpty(_tweetTableName) || String.IsNullOrWhiteSpace(_tweetTableName))
                {
                    _tweetTableName = ConfigurationManager.AppSettings["TweetTableName"];
                    //checkCreateTable(_tweetTableName);
                }
                return _tweetTableName;
            }
        }
        public static string ConversationTableName
        {
            get
            {
                if (String.IsNullOrEmpty(_conversationTableName) || String.IsNullOrWhiteSpace(_conversationTableName))
                {
                    _conversationTableName = ConfigurationManager.AppSettings["ConversationTableName"];
                    //checkCreateTable(_conversationTableName);
                }
                return _conversationTableName;
            }
        }
        public static string AnnotatedConversationTableName
        {
            get
            {
                if (String.IsNullOrEmpty(_annotatedConversationTableName) || String.IsNullOrWhiteSpace(_annotatedConversationTableName))
                {
                    _annotatedConversationTableName = ConfigurationManager.AppSettings["AnnotatedConversationTableName"];
                    //checkCreateTable(_annotatedConversationTableName);
                }
                return _annotatedConversationTableName;
            }
        }
        public static HBaseClient HBaseClient
        {
            get
            {
                if(_hbaseClient == null)
                {
                    //Get the Hadoop Cluster info and create connection
                    string clusterName = ConfigurationManager.AppSettings["ClusterName"];
                    string userName = ConfigurationManager.AppSettings["HadoopUserName"];
                    string password = ConfigurationManager.AppSettings["HadoopUserPassword"];
                    SecureString pw = new SecureString();
                    for (int i = 0; i < password.Length; i++)
                    {
                        pw.InsertAt(i, password[i]);
                    }
                    Uri clusterUri = new Uri(clusterName);
                    ClusterCredentials creds = new ClusterCredentials(clusterUri, userName, pw);
                    _hbaseClient = new HBaseClient(creds);
                    checkCreateTable(HadoopContext.TweetTableName);
                    checkCreateTable(HadoopContext.AnnotatedTweetTableName);
                    checkCreateTable(HadoopContext.ConversationTableName);
                    checkCreateTable(HadoopContext.AnnotatedTweetTableName);
                    checkCreateTable(HadoopContext.WordRelationTableName);
                }
                return _hbaseClient;
            }
        }
        public static HiveConnection HiveConnection
        {
            get
            {
                if(_hiveConnection == null)
                {
                    string clusterName = ConfigurationManager.AppSettings["ClusterName"];
                    string userName = ConfigurationManager.AppSettings["HadoopUserName"];
                    string password = ConfigurationManager.AppSettings["HadoopUserPassword"];
                    string storageName = ConfigurationManager.AppSettings["HiveStorageAccount"];
                    string storageKey = ConfigurationManager.AppSettings["HiveStorageKey"];
                    _hiveConnection = new HiveConnection(new Uri(clusterName), userName, password, storageName, storageKey);
                }
                return _hiveConnection;
            }
        }
        private static void checkCreateTable(string tableName)
        {
            try
            {
                List<string> tables = HadoopContext.HBaseClient.ListTables().name;
                if (!tables.Contains(tableName))
                {
                    var tableSchema = new TableSchema();
                    tableSchema.name = tableName;
                    tableSchema.columns.Add(new ColumnSchema { name = "d" });
                    HadoopContext.HBaseClient.CreateTable(tableSchema);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
