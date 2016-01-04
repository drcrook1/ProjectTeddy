using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitterStreamer;
using Microsoft.HBase.Client;

namespace TwitterStreamerTest
{
    [TestClass]
    public class HBaseWriterTest
    {
        [TestMethod]
        public void TestReadConfig()
        {
            HBaseWriter writer = new HBaseWriter();
            Assert.IsTrue(writer.ClusterName.CompareTo("https://projectteddy.azurehdinsight.net") == 0);
            Assert.IsTrue(writer.HadoopUserName.CompareTo("drcrook") == 0);
            Assert.IsTrue(writer.HBaseTableName.CompareTo("tweetsbywords") == 0);

            var s = writer.client.ListTables();
        }
    }
}
