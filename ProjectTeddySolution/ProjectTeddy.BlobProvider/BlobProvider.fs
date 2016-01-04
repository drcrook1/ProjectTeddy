namespace ProjectTeddy.BlobProvider

open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Auth
open Microsoft.WindowsAzure.Storage.Blob
open Microsoft.WindowsAzure
open System.Configuration
open System.IO

module BlobProvider =
 
    let GetStorageContainer containerName = 
        try
            let conString = ConfigurationManager.AppSettings.["BlobStorageConString"]
            let storageAccount = CloudStorageAccount.Parse conString
            let blobClient = storageAccount.CreateCloudBlobClient()
            let blobContainer = blobClient.GetContainerReference(containerName)
            blobContainer.CreateIfNotExists() |> ignore
            let bcPermissions = new BlobContainerPermissions()
            bcPermissions.PublicAccess = BlobContainerPublicAccessType.Off |> ignore
            blobContainer.SetPermissions(bcPermissions) |> ignore
            blobContainer
        with
            | ex -> printfn "Error: %s" ex.Message; null
 
    let UploadToBlockBlob containerName blobName filePath =
        try
            let blobContainer = GetStorageContainer containerName
            let blockBlob = blobContainer.GetBlockBlobReference(blobName)
            use fileStream = System.IO.File.OpenRead filePath
            blockBlob.UploadFromStream fileStream |> ignore
        with
            | ex -> printfn "Error: %s" ex.Message; 
 
    let rec IterCloudBlobDirectory (dir:CloudBlobDirectory) = 
        dir.ListBlobs() 
        |> Seq.iter (fun (blob2 : IListBlobItem) ->
            if blob2 :? CloudBlockBlob then
                let blob2 = blob2 :?> CloudBlockBlob
                printfn "Blob Name: %s" blob2.Name
            else if blob2 :? CloudPageBlob then
                let blob2 = blob2 :?> CloudPageBlob //there are only 2 options, so it must be page.
                printfn "Blob Name: %s" blob2.Name
            else
                (blob2 :?> CloudBlobDirectory)
                |> IterCloudBlobDirectory
                )

    let ViewBlobs containerName =
        try
            let storageContainer = GetStorageContainer containerName
            storageContainer.ListBlobs() 
            |> Seq.iter (fun (blob : IListBlobItem) -> 
                if blob :? CloudBlobDirectory then
                    (blob :?> CloudBlobDirectory)
                    |> IterCloudBlobDirectory
                else if blob :? CloudBlockBlob then
                    let blob = blob :?> CloudBlockBlob
                    printfn "Blob Name: %s" blob.Name
                else if blob :? CloudPageBlob then
                    let blob = blob :?> CloudPageBlob
                    printfn "Blob Name: %s" blob.Name
                else
                    printfn "Returned incorrect directory type: %s" (blob.GetType().ToString())
            )
        with
            | ex -> printfn "Error: %s" ex.Message
 
    let DownloadBlockBlob containerName blobName filePath =
        try
            let storageContainer = GetStorageContainer containerName
            let blob = storageContainer.GetBlockBlobReference blobName
            use fileStream = System.IO.File.OpenWrite filePath
            blob.DownloadToStream fileStream |> ignore
        with
            | ex -> printfn "Error: %s" ex.Message 
 
    let DeleteBlockBlob containerName blobName =
        try
            let storageContainer = GetStorageContainer containerName
            let blob = storageContainer.GetBlockBlobReference blobName
            blob.Delete() |> ignore
        with
            | ex -> printfn "Error: %s" ex.Message

