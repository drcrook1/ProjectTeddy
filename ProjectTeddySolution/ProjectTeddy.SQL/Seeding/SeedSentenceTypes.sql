/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
SET IDENTITY_INSERT [dbo].[SentenceTypes] ON
GO
insert into [dbo].[SentenceTypes] (Id, SentenceType)
values
(0, 'Imperative'),
(1, 'Declarative'),
(2, 'Interogative'),
(3, 'Exclamatory'),
(4, 'Probing')
SET IDENTITY_INSERT [dbo].[SentenceTypes] OFF
GO