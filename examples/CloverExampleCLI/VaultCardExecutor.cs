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
using com.clover.sdk.v3.payments;

/**
 * Created by blakewilliams on 10/15/16.
 */
namespace CloverExampleCLI
{
    public class VaultCardExecutor : DefaultListener
    {

        public VaultCardExecutor(ICloverConnector cloverConnector) : base(cloverConnector)
        {
        }

        /**
         * invokes vault card, enable swipt, contactless and chip
         */
        public void run()
        {
            cloverConnector.AddCloverConnectorListener(this);
            cloverConnector.VaultCard(CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT | CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS | CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE);
            waitForResponse();
        }

        /**
         * will print the VaultedCard returned from the device in response to
         * a vaultCard call.
         * @param response
         */
        public override void OnVaultCardResponse(VaultCardResponse response)
        {
            if (response.Success)
            {
                VaultedCard vaultedCard = response.Card;


                System.Console.WriteLine("#");
                System.Console.WriteLine("# Vault Card Successful.");
                System.Console.WriteLine("#");
                System.Console.WriteLine("# Token:           " + vaultedCard.token);
                System.Console.WriteLine("# Cardholder Name: " + vaultedCard.cardholderName);
                System.Console.WriteLine("# Card First 6:    " + vaultedCard.first6);
                System.Console.WriteLine("# Card Last 4:     " + vaultedCard.last4);
                System.Console.WriteLine("# Card Expiration: " + vaultedCard.expirationDate);
                System.Console.WriteLine("#");
            }
            else
            {
                System.Console.WriteLine("!");
                System.Console.WriteLine("! Vault Card Failed");
                System.Console.WriteLine("!");
            }
            done();
        }
    }
}