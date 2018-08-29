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

/**
 * A class that orchestrates the creation of an PreAuthRequest, as well
 * as implementing additional ICloverConnectorListener methods with
 * an override of the onPreAuthResponse and onCapturePreAuthResponse callback methods that is made by
 * the Clover Mini after the transaction is processed.
 */
namespace CloverExampleCLI
{
    public class PreAuthExecutor : AuthExecutor
    {

        public PreAuthExecutor(ICloverConnector cloverConnector, long? amount, string externalId) : base(cloverConnector, amount, externalId)
        {
        }

        /**
         * method called by the executor service. Will prompt for amount if needed and
         * creates a PreAuthRequest with amount, then adds itself as a listener in
         * order to get the onPreAuthResponse and onCapturePreAuthResponse callback from the Clover Mini
         */
        public override void run()
        {
            if (amount == null)
            {
                string amt = readLine("Amount? ");
                amount = long.Parse(amt);
            }
            if (externalId == null)
            {
                externalId = "" + new Random().Next().ToString();
            }

            // Creating a simple PreAuthRequest and passing it to the cloverConnector preAuth() method.
            // expecting an onPreAuthResponse callback because we registered ourselves as the listener
            PreAuthRequest request = new PreAuthRequest();
            request.Amount = (long)amount;
            request.ExternalId = externalId;
            cloverConnector.AddCloverConnectorListener(this);
            cloverConnector.PreAuth(request);
            waitForResponse();
        }

        /**
         * will print out the payment information, as well as orchestrate additional
         * action that can be taken on the payment like, capturing, refunding or voiding the payment
         * @param response
         */
        public override void OnPreAuthResponse(PreAuthResponse response)
        {
            if (response.Success)
            {
                this.payment = response.Payment;
                System.Console.WriteLine("#");
                System.Console.WriteLine("# PreAuth successful: " + response.Payment.amount);
                System.Console.WriteLine("#");

                String tip = readLine("Do you want to capture the pre auth for $20? (y/N)");
                if (Utils.stringEqualsIgnoreCase(tip, "y"))
                {
                    doCapturePreAuth(2000);
                }
                else
                {
                    System.Console.WriteLine("N");
                    done();
                }
            }
            else
            {
                System.Console.WriteLine("!");
                System.Console.WriteLine("! PreAuth failed.");
                System.Console.WriteLine("!");
                done();
            }
        }

        /**
         * Creates a CapturePreAuthRequest based on the payment
         * that was retrieved in the onPreAuthResponse callback.
         */
        protected void doCapturePreAuth(long amt)
        {
            CapturePreAuthRequest cpar = new CapturePreAuthRequest();
            cpar.Amount = amt;
            cpar.TipAmount = 0;
            cpar.PaymentID = payment.id;
            cloverConnector.CapturePreAuth(cpar);
            waitForResponse();
        }

        /**
         * handles the callback from the device after a CapturePreAuthRequest is
         * sent to the Mini
         * @param response
         */
        public override void OnCapturePreAuthResponse(CapturePreAuthResponse response)
        {
            if (response.Success)
            {
                System.Console.WriteLine("#");
                System.Console.WriteLine("# Capture PreAuth successful!");
                System.Console.WriteLine("#");

                String tipAdjust = readLine("Would you like to add a $2.00 tip? (y/N)");
                if (Utils.stringEqualsIgnoreCase(tipAdjust, "y"))
                {
                    doTip(200);
                }
                else
                {
                    System.Console.WriteLine("N");
                    String refund = readLine("Would you like to refund the payment? (y/N)");
                    if (Utils.stringEqualsIgnoreCase(refund, "y"))
                    {
                        String fullRefund = readLine("Would you like to refund the full amount? (y/N)");
                        if (Utils.stringEqualsIgnoreCase(fullRefund, "y"))
                        {
                            doFullRefund();
                        }
                        else
                        {
                            System.Console.WriteLine("N");
                            doPartialRefund();
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("N");
                        String voidPayment = readLine("Would you like to void the payment? (y/N)");
                        if (Utils.stringEqualsIgnoreCase(voidPayment, "y"))
                        {
                            doVoidPayment();
                        }
                        else
                        {
                            System.Console.WriteLine("N");
                            done();
                        }
                    }
                }
            }
            else
            {
                System.Console.WriteLine("#");
                System.Console.WriteLine("# Capture PreAuth failed!");
                System.Console.WriteLine("#");
                done();
            }
        }
    }
}