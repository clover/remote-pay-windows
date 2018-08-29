
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
using System;

namespace CloverExampleCLI
{
    public class ManualRefundExecutor : DefaultListener
    {
        long? amount;
        string externalId;

        public ManualRefundExecutor(ICloverConnector cloverConnector, long? amount, string externalId) : base(cloverConnector)
        {
        }

        /**
         * A class that orchestrates the creation of a ManualRefundRequest, as well
         * as implementing additional ICloverConnectorListener methods with
         * an override of the onManualRefundResponse callback that is made by
         * the Clover Mini after the manual refund is processed.
         */
        public void run()
        {
            if (amount == null)
            {
                String amt = readLine("Amount? ");
                amount = long.Parse(amt);
            }
            if (externalId == null)
            {
                externalId = "" + new Random().Next().ToString();
            }

            ManualRefundRequest request = new ManualRefundRequest();
            request.ExternalId = externalId;
            request.Amount = (long)amount;
            cloverConnector.AddCloverConnectorListener(this);
            cloverConnector.ManualRefund(request);
            waitForResponse();
        }

        /**
         * will print out the credit information
         * @param response
         */
        public override void OnManualRefundResponse(ManualRefundResponse response)
        {
            if (response.Success)
            {
                System.Console.WriteLine("#");
                System.Console.WriteLine("# Manual Refund successful.");
                System.Console.WriteLine("#");
                System.Console.WriteLine("# Refund Amount:      " + Utils.currencyFormat(response.Credit.amount));
                long? refundTaxAmount = response.Credit.taxAmount;
                if (refundTaxAmount == null)
                {
                    refundTaxAmount = 0;
                }

                System.Console.WriteLine(string.Format("# Refund Tax Amount:  ") + Utils.currencyFormat(refundTaxAmount));
                System.Console.WriteLine("#");
            }
            else
            {
                System.Console.WriteLine("!");
                System.Console.WriteLine("! Manual Refund failed!");
                System.Console.WriteLine("!");
            }
            done();
        }
    }

}