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
using System;

/**
 * A class that orchestrates the creation of a SaleRequest, as well
 * as implementing additional ICloverConnectorListener methods with
 * an override of the onSaleResponse callback that is made by
 * the Clover Mini after the transaction is processed.
 */
namespace CloverExampleCLI
{
    public class SaleExecutor : DefaultListener
    {
        protected long? amount { get; set; }
        protected string externalId { get; set; }
        protected Payment payment { get; set; }

        public SaleExecutor(ICloverConnector cloverConnector, long? amount, string externalId) : base(cloverConnector)

        {
            this.amount = amount;
            this.externalId = externalId;
        }

        /**
         * method called by the executor service. Will prompt for amount if needed and
         * creates a SaleRequest with amount, then adds itself as a listener in
         * order to get the onSaleResponse callback from the Clover Mini
         */
        public virtual void run()
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

            // Creating a simple SaleRequest and passing it to the cloverConnector sale() method.
            // expecting an onSaleResponse callback because we registered ourselves as the listener
            SaleRequest request = new SaleRequest();
            request.Amount = amount.Value;
            request.ExternalId = externalId;

            // compute the tax as a part of the amount
            request.TaxAmount = amount - (long)Math.Round(amount.Value / 1.07, 2);
            //request.setTippableAmount((long)(amount*.9)); // if the tip suggestions should be calculated on a different amount
            //allowable card entry methods can be changed.
            //request.setCardEntryMethods(CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT | CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE | CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS | CloverConnector.CARD_ENTRY_METHOD_MANUAL);
            cloverConnector.AddCloverConnectorListener(this);
            cloverConnector.Sale(request);
            waitForResponse();
        }

        /**
         * will print out the payment information, as well as orchestrate additional
         * action that can be taken on the payment like, refunding or voiding the payment
         * @param response
         */
        public override void OnSaleResponse(SaleResponse response)
        {
            if (response.Success)
            {
                payment = response.Payment;
                System.Console.WriteLine("#");
                System.Console.WriteLine("# Sale successful");
                System.Console.WriteLine("#");
                System.Console.WriteLine("# Subtotal:   " + Utils.currencyFormat(payment.amount - payment.taxAmount));
                System.Console.WriteLine("# Tax:        " + Utils.currencyFormat(payment.taxAmount));
                System.Console.WriteLine("# Total:      " + Utils.currencyFormat(payment.amount));
                System.Console.WriteLine("# Tip:        " + Utils.currencyFormat(payment.tipAmount));
                System.Console.WriteLine("# Total + Tip:" + Utils.currencyFormat(payment.amount + payment.tipAmount));
                System.Console.WriteLine("#");
                System.Console.WriteLine("# Card:       " + payment.cardTransaction.type + "  xxxxxxxxxxxx" + payment.cardTransaction.last4);
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
                    System.Console.WriteLine("N");
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
                System.Console.WriteLine("! Sale failed.");
                System.Console.WriteLine("!");
                done();
            }
        }

        /**
         * create the appropriate request to do a full refund
         */
        protected virtual void doFullRefund()
        {
            RefundPaymentRequest rpr = new RefundPaymentRequest();
            rpr.PaymentId = payment.id;
            rpr.OrderId = payment.order.id;
            rpr.FullRefund = true;
            cloverConnector.RefundPayment(rpr);
        }

        /**
         * create the appropriate request to do a partial refund
         */
        protected virtual void doPartialRefund()
        {
            RefundPaymentRequest rpr = new RefundPaymentRequest();
            rpr.PaymentId = payment.id;
            rpr.OrderId = payment.order.id;
            rpr.FullRefund = false;
            rpr.Amount = (payment.amount / 2); // refund half by default
            cloverConnector.RefundPayment(rpr);
        }

        /**
         * handles the callback from the device after a RefundPaymentRequest is
         * sent to the Mini
         * @param response
         */
        public override void OnRefundPaymentResponse(RefundPaymentResponse response)
        {
            if (response.Success)
            {
                System.Console.WriteLine("#");
                System.Console.WriteLine("# Refund Succeeded!");
                System.Console.WriteLine("#");
            }
            else
            {
                System.Console.WriteLine("!");
                System.Console.WriteLine("! Refund failed.");
                System.Console.WriteLine("!");
            }
            done();
        }

        /**
         * Creates a VoidPaymentRequest based on the payment
         * that was retrieved in the onSaleResponse callback.
         */
        protected virtual void doVoidPayment()
        {
            VoidPaymentRequest vpr = new VoidPaymentRequest();
            vpr.PaymentId = payment.id;
            vpr.OrderId = payment.order.id;
            vpr.VoidReason = "USER_CANCEL";         
            cloverConnector.VoidPayment(vpr);
        }

        /**
         * handles the callback from the device after a VoidPaymentRequest is
         * sent to the Mini
         * @param response
         */
        public override void OnVoidPaymentResponse(VoidPaymentResponse response)
        {
            if (response.Success)
            {
                System.Console.WriteLine("#");
                System.Console.WriteLine("# Payment was voided!");
                System.Console.WriteLine("#");
            }
            else
            {
                System.Console.WriteLine("!");
                System.Console.WriteLine("! Payment NOT voided.");
                System.Console.WriteLine("!");
            }
            done();
        }
    }
}