namespace ProjectTeddy.CorrelationBuilder

open ProjectTeddy.Core.EntityFramework
open System.Data
open System
open System.Linq
open ProjectTeddy.HadoopHandler
open Microsoft.Azure
open Microsoft.Azure.WebJobs

module Start =   
    [<EntryPoint>] 
    let main argv =  
//        let _dashboardConn = @"DefaultEndpointsProtocol=https;AccountName=portalvhds88z53rq311gj2;AccountKey=Hw35TfCUjBMZ4OmPjx7wDARtK4pojV8MH0rmF0kvScPffnJXF9CyM1+YEYLX9lW8y/JW6tVq86vPmOCSfCisLw==" 
//        let _storageConn = @"DefaultEndpointsProtocol=https;AccountName=portalvhds88z53rq311gj2;AccountKey=Hw35TfCUjBMZ4OmPjx7wDARtK4pojV8MH0rmF0kvScPffnJXF9CyM1+YEYLX9lW8y/JW6tVq86vPmOCSfCisLw==" 
//        let config = new JobHostConfiguration() 
//        config.DashboardConnectionString <- _dashboardConn 
//        config.StorageConnectionString <- _storageConn 
//        printf "%s" config.DashboardConnectionString 
//        let host = new JobHost(config)  
//        //let host = new JobHost() 
//        host.RunAndBlock() 
        WordsDB.CreateWordRelationDB()
        0 // return an integer exit code 
