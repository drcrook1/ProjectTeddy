namespace ProjectTeddy.Analytics
//
//open ProjectTeddy.HadoopHandler
//open System
//open Hive.HiveRuntime
//open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols
//open Microsoft.FSharp.Linq.NullableOperators
//
//module FSHadoopProviders = 
//    [<Literal>]
//    let dsn = "Word Relation Table; pwd=David!2345"
//    type Conn = Hive.HiveTypeProvider<dsn, DefaultMetadataTimeout=1000>
//    type FSHiveReader () =        
//        interface IHadoopReader with
//            //IEnumerable<FSWordRelationship> 
//            member this.FindRelationsWithWord(word:string) =             
//                let context = Conn.GetDataContext()
//                let query = hiveQuery {for row in context.wordrelationtable do
//                                       where (float(row.rscore) > 0.0
//                                              && (row.wordone = word || row.wordtwo = word))
//                                       select row}
//                query.Run() |> Seq.map(fun row ->
//                                        {Id=row.rowkey; WordOne=row.wordone; WordOneId=int(row.wordoneid); 
//                                        WordTwo=row.wordtwo; WordTwoId=int(row.wordtwoid); rScore=float(row.rscore)})
//            //IEnumerable<FSWordRelationship> 
//            member this.ReadWordRelationshipRange(startId: int, endId : int) =
//                failwith "Not Implemented"
//            //IEnumerable<FSTweet> 
//            member this.ReadAllTweets() =
//                failwith "Not Implemented"
//            //IEnumerable<FSTweet> 
//            member this.ReadTweetsRange(startTime:DateTime, numYouWant:int) =
//                failwith "Not Implemented"
//            //IEnumerable<FSAnnotatedTweet> 
//            member this.ReadAllAnnotatedTweets() =
//                failwith "Not Implemented"
//            //IEnumerable<FSAnnotatedTweet> 
//            member this.ReadAnnotatedTweetsRange(startTime:DateTime, numYouWant:int) =
//                failwith "Not Implemented"
//            //IEnumerable<FSConversation> 
//            member this.ReadAllConversations() =
//                failwith "Not Implemented"
//            //IEnumerable<FSConversation> 
//            member this.ReadConversationsRange(startTime:DateTime, numYouWant:int) =
//                failwith "Not Implemented"
//            //IEnumerable<FSAnnotatedConversation> 
//            member this.ReadAllAnnotatedConversations() =
//                failwith "Not Implemented"
//            //IEnumerable<FSAnnotatedConversation> 
//            member this.ReadAnnotatedConversationsRange(startTime:DateTime, numYouWant:int) = 
//                failwith "Not Implemented"
