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
 * A class that orchestrates the creation of an AuthRequest, as well
 * as implementing additional ICloverConnectorListener methods with
 * an override of the onAuthResponse and onTipAdjustAuthResponse callback methods that is made by
 * the Clover Mini after the transaction is processed.
 */
namespace CloverExampleCLI
{
    public class AuthExecutor : SaleExecutor
    {

        public AuthExecutor(ICloverConnector cloverConnector, long? amount, string externalId) : base(cloverConnector, amount, externalId)
        {
        }

        /**
         * method called by the executor service. Will prompt for amount if needed and
         * creates a AuthRequest with amount, then adds itself as a listener in
         * order to get the onAuthResponse and onTipAdjustAuth callback from the Clover Mini
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

            // We should now have a non-null ammount
            long valAmount = amount.Value;

            // Creating a simple AuthRequest and passing it to the cloverConnector auth() method.
            // expecting an onAuthResponse callback because we registered ourselves as the listener
            AuthRequest request = new AuthRequest();
            request.Amount = valAmount;
            request.ExternalId = externalId;

            request.TaxAmount = valAmount - (long)Math.Round(valAmount / 1.07);
            cloverConnector.AddCloverConnectorListener(this);
            cloverConnector.Auth(request);
            waitForResponse();
        }

        /**
         * will print out the payment information, as well as orchestrate additional
         * action that can be taken on the payment like tip adjusting, refunding or voiding the payment
         * @param response
         */
        public override void OnAuthResponse(AuthResponse response)
        {
            if (response.Success)
            {
                this.payment = response.Payment;
                System.Console.WriteLine("#");
                System.Console.WriteLine("# Subtotal:   " + Utils.currencyFormat(payment.amount - payment.taxAmount));
                System.Console.WriteLine("# Tax:        " + Utils.currencyFormat(payment.taxAmount));
                System.Console.WriteLine("# Total:      " + Utils.currencyFormat(payment.amount));
                System.Console.WriteLine("#");
                System.Console.WriteLine("# Card:       " + payment.cardTransaction.cardType + "  xxxxxxxxxxxx" + payment.cardTransaction.last4);
                System.Console.WriteLine("#");

                long tipAmount = (long)Math.Round(response.Payment.amount * .15);

                string tip = readLine("Do you want to add a tip of " + Utils.currencyFormat(tipAmount) + "? (y/N)");
                if (Utils.stringEqualsIgnoreCase(tip, "y"))
                {
                    doTip(tipAmount);
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
                System.Console.WriteLine("! Auth failed.");
                System.Console.WriteLine("!");
                done();
            }
        }

        /**
         * Creates a TipAdjustAuthRequest based on the payment
         * that was retrieved in the onAuthResponse callback.
         * @param tipAmount
         */
        protected void doTip(long tipAmount)
        {
            TipAdjustAuthRequest taar = new TipAdjustAuthRequest();
            taar.PaymentID = (payment.id);
            taar.OrderID = (payment.order.id);
            taar.TipAmount = (tipAmount);
            cloverConnector.TipAdjustAuth(taar);
        }

        /**
         * will print out the payment information, as well as orchestrate additional
         * action that can be taken on the payment like refunding or voiding the payment
         * @param response
         */
        public override void OnTipAdjustAuthResponse(TipAdjustAuthResponse response)
        {
            if (response.Success)
            {
                System.Console.WriteLine("#");
                System.Console.WriteLine("# Subtotal:   " + Utils.currencyFormat(payment.amount - payment.taxAmount));
                System.Console.WriteLine("# Tax:        " + Utils.currencyFormat(payment.taxAmount));
                System.Console.WriteLine("# Total:      " + Utils.currencyFormat(payment.amount));
                System.Console.WriteLine("# Tip:        " + Utils.currencyFormat(response.TipAmount));
                System.Console.WriteLine("# Total + Tip:" + Utils.currencyFormat(payment.amount + response.TipAmount));
                System.Console.WriteLine("#");
                System.Console.WriteLine("# Card:       " + payment.cardTransaction.cardType + "  xxxxxxxxxxxx" + payment.cardTransaction.last4);
                System.Console.WriteLine("#");


                string refund = readLine("Would you like to refund the payment? (y/N)");
                if (Utils.stringEqualsIgnoreCase(refund, "y"))
                {
                    string fullRefund = readLine("Would you like a full refund? (y/N)");
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
                    string voidPayment = readLine("Would you like to void the payment? (y/N)");
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
            else
            {
                System.Console.WriteLine("!");
                System.Console.WriteLine("! Tip failed!");
                System.Console.WriteLine("!");
                done();
            }
        }


    }
}