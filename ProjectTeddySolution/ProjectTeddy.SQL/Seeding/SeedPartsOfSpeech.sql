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
SET IDENTITY_INSERT [dbo].[PartsOfSpeech] ON
GO
Insert into [dbo].[PartsOfSpeech] (Id, PartOfSpeech)
values 
(0, 'Noun'),
(1, 'Pronoun'),
(2, 'PosesivePronoun'),
(3, 'ProperPerson'),
(4, 'ProperPlace'),
(5, 'ProperThing'),
(6, 'QuantifierPronoun'),
(7, 'Adverb'),
(8, 'Preposition'),
(9, 'ActionVerb'),
(10, 'BeingVerb'),
(11, 'HelpingVerb'),
(12, 'ConcatenatedWords'),
(13, 'WordAndPunctuation'),
(14, 'Adjective'),
(15, 'Punctuation'),
(16, 'Emoticon'),
(17, 'Contraction'),
(18, 'Number'),
(19, 'Probing'),
(20, 'Unknown')
SET IDENTITY_INSERT [dbo].[PartsOfSpeech] OFF
GO