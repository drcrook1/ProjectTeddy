# Config Setting related to HBase 502 Bad gateway error 
#If you don't have a publish settings file, uncomment the get settings file
#Execute this script from the PowerShell ISE

$CoreConfigValues = @{"fs.azure.io.copyblob.retry.max.retries"="30"}
$ClusterNodeCount = 1
$BlobStorageAccount = "projectteddy"
$PrimaryStorageKey = "SY0iHK1xnRCpBTezwAZw7vLy6ojTu3l046pvhFbMz3hr0lzDwJ4TfyHP197RkPg1zQWoW2wEQmhpgUZafwIarA=="
$DefaultStorageContainer = "teddydialogue"
$ClusterDnsName = "projectteddy"
$secpasswd = ConvertTo-SecureString "David!2345" -AsPlainText -Force
$HdiClusterCredentials = New-Object System.Management.Automation.PSCredential ("drcrook", $secpasswd)

$ClusterVersion = "3.2"
$DataCenterLocation = "East US"

#Get-AzurePublishSettingsFile
Import-AzurePublishSettingsFile -PublishSettingsFile "C:\Users\dacrook\Downloads\drcrookdata-credentials.publishsettings" 
Select-AzureSubscription -SubscriptionName "BizSpark"

$Config = New-AzureHDInsightClusterConfig -ClusterSizeInNodes $ClusterNodeCount -ClusterType HBase | `

          Set-AzureHDInsightDefaultStorage -StorageAccountName $BlobStorageAccount -StorageAccountKey $PrimaryStorageKey -StorageContainerName $DefaultStorageContainer | `

          Add-AzureHDInsightConfigValues -Core $CoreConfigValues | `

          New-AzureHDInsightCluster -Name $ClusterDnsName -Credential $HdiClusterCredentials -Version $ClusterVersion -Location $DataCenterLocation
