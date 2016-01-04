CREATE EXTERNAL TABLE WordRelationTable(
    rowkey STRING, WordOne STRING, WordOneId STRING, WordTwo STRING, WordTwoId STRING, RScore STRING)
STORED BY 'org.apache.hadoop.hive.hbase.HBaseStorageHandler'
WITH SERDEPROPERTIES ('hbase.columns.mapping' = ':key,d:WordOne,d:WordOneId,d:WordTwo,d:WordTwoId,d:RScore')
TBLPROPERTIES ('hbase.table.name' = 'wordrelationstrengths');
