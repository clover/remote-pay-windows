/*
 * Copyright (C) 2018 Clover Network, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * You may obtain a copy of the License at
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using com.clover.remotepay.sdk;
using com.clover.remotepay.transport;
using System;

/**
 * Created by blakewilliams on 10/15/16.
 */
namespace CloverExampleCLI
{
    public class ReadCardDataExecutor : DefaultListener
    {
        public ReadCardDataExecutor(ICloverConnector cloverConnector) : base(cloverConnector)
        {
        }

        public void run()
        {
            ReadCardDataRequest request = new ReadCardDataRequest();
            request.CardEntryMethods = CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT | CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE | CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS;
            cloverConnector.AddCloverConnectorListener(this);
            cloverConnector.ReadCardData(request);
            waitForResponse();
        }

        /**
         * will print the CardData returned from the device in response to
         * a readCardData call.
         * @param response
         */
        public override void OnReadCardDataResponse(ReadCardDataResponse response)
        {
            if (response.Success)
            {
                CardData cardData = response.CardData;
                System.Console.WriteLine("#");
                System.Console.WriteLine("# Read Card Data was successful");
                System.Console.WriteLine("#");
                System.Console.WriteLine("# Cardholder Name: " + cardData.CardholderName);
                System.Console.WriteLine("# First Name:      " + cardData.FirstName);
                System.Console.WriteLine("# Last Name:       " + cardData.LastName);
                System.Console.WriteLine("# First 6:         " + cardData.First6);
                System.Console.WriteLine("# Last 4:          " + cardData.Last4);
                System.Console.WriteLine("# Expiration:      " + cardData.Exp);
                System.Console.WriteLine("# Encrypted?       " + cardData.Encrypted);
                System.Console.WriteLine("# Track 1:         " + cardData.Track1);
                System.Console.WriteLine("# Track 2:         " + cardData.Track2);
                System.Console.WriteLine("# Track 3:         " + cardData.Track3);
                System.Console.WriteLine("#");
            }
            else
            {
                System.Console.WriteLine("!");
                System.Console.WriteLine("! Read Card Data Failed!");
                System.Console.WriteLine("!");
            }
            done();
        }
    }
}