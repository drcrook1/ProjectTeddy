/*
Copyright (c) Microsoft Corporation
All rights reserved. 
MIT License
 
Permission is hereby granted, free of charge, to any person obtaining a copy of this 
software and associated documentation files (the "Software"), to deal in the Software 
without restriction, including without limitation the rights to use, copy, modify, merge, 
publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons 
to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or 
substantial portions of the Software.
THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using MicrosoftProjectOxford;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace MicrosoftProjectOxfordExample
{
    class SpeechRecognitionServiceExample
    {
        string m_filename = null;
        SpeechRecognitionMode m_recoMode;
        bool m_isMicrophoneReco;
        MicrophoneRecognitionClient m_micClient;

        /// <summary>
        ///     Called when a final response is received; 
        /// </summary>
        void OnResponseReceivedHandler(object sender, SpeechResponseEventArgs e)
        {
            bool isFinalDicationMessage = m_recoMode == SpeechRecognitionMode.LongDictation &&
                                                        (e.PhraseResponse.RecognitionStatus == RecognitionStatus.EndOfDictation ||
                                                         e.PhraseResponse.RecognitionStatus == RecognitionStatus.DictationEndSilenceTimeout);

            if (m_isMicrophoneReco && ((m_recoMode == SpeechRecognitionMode.ShortPhrase) || isFinalDicationMessage))
            {
                // we got the final result, so it we can end the mic reco.  No need to do this
                // for dataReco, since we already called endAudio() on it as soon as we were done
                // sending all the data.
                m_micClient.EndMicAndRecognition();
            }

            if (!isFinalDicationMessage)
            {
                Console.WriteLine("********* Final NBEST Results *********");
                for (int i = 0; i < e.PhraseResponse.Results.Length; i++)
                {
                    Console.WriteLine("[{0}] Confidence={1} Text=\"{2}\"",
                                      i, e.PhraseResponse.Results[i].Confidence,
                                      e.PhraseResponse.Results[i].DisplayText);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        ///     Called when a final response is received and its intent is parsed 
        /// </summary>
        void OnIntentHandler(object sender, SpeechIntentEventArgs e)
        {
            Console.WriteLine("********* Final Intent *********");
            Console.WriteLine("{0}", e.Payload);
            Console.WriteLine();
        }

        /// <summary>
        ///     Called when a partial response is received; 
        /// </summary>
        void OnPartialResponseReceivedHandler(object sender, PartialSpeechResponseEventArgs e)
        {
            Console.WriteLine("********* Partial Result *********");
            Console.WriteLine("{0}", e.PartialResult);
            Console.WriteLine();
        }

        /// <summary>
        ///     Called when an error is received; 
        /// </summary>
        void OnConversationErrorHandler(object sender, SpeechErrorEventArgs e)
        {
            Console.WriteLine("********* Error Detected *********");
            Console.WriteLine("{0}", e.SpeechErrorCode);
            Console.WriteLine("{0}", e.SpeechErrorText);
            Console.WriteLine();
        }

        /// <summary>
        ///     Called when the microphone status has changed.
        /// </summary>
        void OnMicrophoneStatus(object sender, MicrophoneEventArgs e)
        {
            Console.WriteLine("********* Microphone status: {0} *********", e.Recording);
            if (!e.Recording)
            {
                m_micClient.EndMicAndRecognition();
            }
            Console.WriteLine();
        }

        /// <summary>
        ///     Speech recognition with data (for example from a file or audio source).  
        ///     The data is broken up into buffers and each buffer is sent to the Speech Recognition Service.
        ///     No modification is done to the buffers, so the user can apply their
        ///     own Silence Detection if desired.
        /// </summary>
        void DoDataRecognition(DataRecognitionClient dataClient)
        {
            // Choose between a two minute recitation of the wikipedia page for batman
            // or a a short utterance
            string filename = (m_recoMode == SpeechRecognitionMode.LongDictation) ? "C:\\dev\\audio\\v1.wav" :
                                                                                    "C:\\dev\\audio\\v1.wav";
            if (m_filename != null)
            {
                filename = m_filename;
            }
            int waitSeconds = (m_recoMode == SpeechRecognitionMode.LongDictation) ? 200 : 15;

            using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                // Note for wave files, we can just send data from the file right to the server.
                // In the case you are not an audio file in wave format, and instead you have just
                // raw data (for example audio coming over bluetooth), then before sending up any 
                // audio data, you must first send up an SpeechAudioFormat descriptor to describe 
                // the layout and format of your raw audio data via DataRecognitionClient's sendAudioFormat() method.

                int bytesRead = 0;
                byte[] buffer = new byte[1024];

                try
                {
                    do
                    {
                        // Get more Audio data to send into byte buffer.
                        bytesRead = fileStream.Read(buffer, 0, buffer.Length);

                        // Send of audio data to service. 
                        dataClient.SendAudio(buffer, bytesRead);
                    } while (bytesRead > 0);
                }
                finally
                {
                    // We are done sending audio.  Final recognition results will arrive in OnResponseReceived event call.
                    dataClient.EndAudio();
                }

                // sleep until the final result in OnResponseReceived event call, or waitSeconds, whichever is smaller.
                bool isReceivedResponse = dataClient.WaitForFinalResponse(waitSeconds * 1000);
                if (!isReceivedResponse)
                {
                    Console.WriteLine("{0}: Timed out waiting for conversation response after {1} ms",
                                      DateTime.UtcNow, waitSeconds * 1000);
                }
            }
        }

        /// <summary>
        ///     Speech recognition from the microphone.  The microphone is turned on and data from the microphone
        ///     is sent to the Speech Recognition Service.  A built in Silence Detector
        ///     is applied to the microphone data before it is sent to the recognition service.
        /// </summary>
        void DoMicrophoneRecognition(MicrophoneRecognitionClient micClient)
        {
            int waitSeconds = (m_recoMode == SpeechRecognitionMode.LongDictation) ? 200 : 15;

            try
            {
                // Turn on the microphone and stream audio to the Speech Recognition Service
                micClient.StartMicAndRecognition();
                Console.WriteLine("Start talking");

                // sleep until the final result in OnResponseReceived event call, or waitSeconds, whichever is smaller.
                bool isReceivedResponse = micClient.WaitForFinalResponse(waitSeconds * 1000);
                if (!isReceivedResponse)
                {
                    Console.WriteLine("{0}: Timed out waiting for conversation response after {1} ms",
                                      DateTime.UtcNow, waitSeconds * 1000);
                }
            }
            finally
            {
                // We are done sending audio.  Final recognition results will arrive in OnResponseReceived event call.
                micClient.EndMicAndRecognition();
            }
        }

        //static void Main(string[] args)
        //{
        //    SpeechRecognitionServiceExample recoExample = new SpeechRecognitionServiceExample();

        //    recoExample.m_recoMode = SpeechRecognitionMode.ShortPhrase;
        //    recoExample.m_isMicrophoneReco = true;
        //    bool isIntent = true;
        //    bool tryMultipleRecos = false;

        //    if (args.Length == 1)
        //    {
        //        recoExample.m_filename = args[0];
        //    }

        //    string primaryOrSecondaryKey = ConfigurationManager.AppSettings["primaryKey"];
        //    string luisAppID = ConfigurationManager.AppSettings["luisAppID"];
        //    string luisSubscriptionID = ConfigurationManager.AppSettings["luisSubscriptionID"];

        //    if (recoExample.m_isMicrophoneReco)
        //    {
        //        MicrophoneRecognitionClientWithIntent intentMicClient;

        //        if (!isIntent)
        //        {
        //            recoExample.m_micClient = SpeechRecognitionServiceFactory.CreateMicrophoneClient(recoExample.m_recoMode, "en-us",
        //                                                                                             primaryOrSecondaryKey);
        //        }
        //        else
        //        {
        //            Debug.Assert(recoExample.m_recoMode == SpeechRecognitionMode.ShortPhrase);
        //            intentMicClient =
        //                SpeechRecognitionServiceFactory.CreateMicrophoneClientWithIntent("en-us",
        //                                                                                 primaryOrSecondaryKey,
        //                                                                                 luisAppID,
        //                                                                                 luisSubscriptionID);
        //            intentMicClient.OnIntent += recoExample.OnIntentHandler;
        //            recoExample.m_micClient = intentMicClient;
        //        }
        //        // Event handlers for speech recognition results
        //        recoExample.m_micClient.OnResponseReceived += recoExample.OnResponseReceivedHandler;
        //        recoExample.m_micClient.OnPartialResponseReceived += recoExample.OnPartialResponseReceivedHandler;
        //        recoExample.m_micClient.OnConversationError += recoExample.OnConversationErrorHandler;
        //        recoExample.m_micClient.OnMicrophoneStatus += recoExample.OnMicrophoneStatus;

        //        try
        //        {
        //            recoExample.DoMicrophoneRecognition(recoExample.m_micClient);
        //        }
        //        finally
        //        {
        //            recoExample.m_micClient.Dispose();
        //        }
        //    }
        //    else
        //    {
        //        DataRecognitionClient dataClient;
        //        DataRecognitionClientWithIntent intentDataClient;

        //        if (!isIntent)
        //        {
        //            dataClient = SpeechRecognitionServiceFactory.CreateDataClient(recoExample.m_recoMode, "en-us",
        //                                                                          primaryOrSecondaryKey);
        //        }
        //        else
        //        {
        //            Debug.Assert(recoExample.m_recoMode == SpeechRecognitionMode.ShortPhrase);
        //            intentDataClient =
        //                SpeechRecognitionServiceFactory.CreateDataClientWithIntent("en-us",
        //                                                                           primaryOrSecondaryKey,
        //                                                                           luisAppID,
        //                                                                           luisSubscriptionID);
        //            intentDataClient.OnIntent += recoExample.OnIntentHandler;
        //            dataClient = intentDataClient;
        //        }
        //        // Event handlers for speech recognition results
        //        dataClient.OnResponseReceived += recoExample.OnResponseReceivedHandler;
        //        dataClient.OnPartialResponseReceived += recoExample.OnPartialResponseReceivedHandler;
        //        dataClient.OnConversationError += recoExample.OnConversationErrorHandler;

        //        try
        //        {
        //            recoExample.DoDataRecognition(dataClient);
        //            if (tryMultipleRecos)
        //            {
        //                recoExample.DoDataRecognition(dataClient);
        //            }
        //        }
        //        finally
        //        {
        //            dataClient.Dispose();
        //        }
        //    }

        //    Console.WriteLine("Press any key to exit");
        //    Console.ReadLine();
        //}

        public static void Listen(bool useItent)
        {
            SpeechRecognitionServiceExample recoExample = new SpeechRecognitionServiceExample();

            bool isIntent = false;
            bool tryMultipleRecos = false;

            if (useItent == true)
            {
                isIntent = true;
                recoExample.m_recoMode = SpeechRecognitionMode.ShortPhrase;
                recoExample.m_isMicrophoneReco = true;
            }
            else if (useItent == false)
            {
                isIntent = false;
                recoExample.m_recoMode = SpeechRecognitionMode.LongDictation;
                recoExample.m_isMicrophoneReco = true;
            }

            //if (args.Length == 1)
            //{
            //    recoExample.m_filename = args[0];
            //}

            string primaryOrSecondaryKey = ConfigurationManager.AppSettings["primaryKey"];
            string luisAppID = ConfigurationManager.AppSettings["luisAppID"];
            string luisSubscriptionID = ConfigurationManager.AppSettings["luisSubscriptionID"];

            if (recoExample.m_isMicrophoneReco)
            {
                MicrophoneRecognitionClientWithIntent intentMicClient;

                if (!isIntent)
                {
                    recoExample.m_micClient = SpeechRecognitionServiceFactory.CreateMicrophoneClient(recoExample.m_recoMode, "en-us",
                                                                                                     primaryOrSecondaryKey);
                }
                else
                {
                    Debug.Assert(recoExample.m_recoMode == SpeechRecognitionMode.ShortPhrase);
                    intentMicClient =
                        SpeechRecognitionServiceFactory.CreateMicrophoneClientWithIntent("en-us",
                                                                                         primaryOrSecondaryKey,
                                                                                         luisAppID,
                                                                                         luisSubscriptionID);
                    intentMicClient.OnIntent += recoExample.OnIntentHandler;
                    recoExample.m_micClient = intentMicClient;
                }
                // Event handlers for speech recognition results
                recoExample.m_micClient.OnResponseReceived += recoExample.OnResponseReceivedHandler;
                recoExample.m_micClient.OnPartialResponseReceived += recoExample.OnPartialResponseReceivedHandler;
                recoExample.m_micClient.OnConversationError += recoExample.OnConversationErrorHandler;
                recoExample.m_micClient.OnMicrophoneStatus += recoExample.OnMicrophoneStatus;

                try
                {
                    recoExample.DoMicrophoneRecognition(recoExample.m_micClient);
                }
                finally
                {
                    recoExample.m_micClient.Dispose();
                }
            }
            else
            {
                DataRecognitionClient dataClient;
                DataRecognitionClientWithIntent intentDataClient;

                if (!isIntent)
                {
                    dataClient = SpeechRecognitionServiceFactory.CreateDataClient(recoExample.m_recoMode, "en-us",
                                                                                  primaryOrSecondaryKey);
                }
                else
                {
                    Debug.Assert(recoExample.m_recoMode == SpeechRecognitionMode.ShortPhrase);
                    intentDataClient =
                        SpeechRecognitionServiceFactory.CreateDataClientWithIntent("en-us",
                                                                                   primaryOrSecondaryKey,
                                                                                   luisAppID,
                                                                                   luisSubscriptionID);
                    intentDataClient.OnIntent += recoExample.OnIntentHandler;
                    dataClient = intentDataClient;
                }
                // Event handlers for speech recognition results
                dataClient.OnResponseReceived += recoExample.OnResponseReceivedHandler;
                dataClient.OnPartialResponseReceived += recoExample.OnPartialResponseReceivedHandler;
                dataClient.OnConversationError += recoExample.OnConversationErrorHandler;

                try
                {
                    recoExample.DoDataRecognition(dataClient);
                    if (tryMultipleRecos)
                    {
                        recoExample.DoDataRecognition(dataClient);
                    }
                }
                finally
                {
                    dataClient.Dispose();
                }
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

    }
}




