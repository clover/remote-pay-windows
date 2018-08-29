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

using System;

using com.clover.remotepay.sdk;
using com.clover.remotepay.transport;
using System.Threading;

namespace CloverExampleCLI
{
    /**
     * DefautListener provides some default behavior common to the Sale, Auth and PreAuth Executors
     * as listeners for the callbacks from the Clover Mini
     */
    public class DefaultListener : DefaultCloverConnectorListener
    {
        protected ICloverConnector cloverConnector { get; set; }
        private static EventWaitHandle ewh;

        public DefaultListener(ICloverConnector cc) : base(cc)
        {
            this.cloverConnector = cc;
            ewh = new EventWaitHandle(false, EventResetMode.AutoReset);
        }

        public static void waitForResponse()
        {
            ewh.WaitOne();
        }

        public override void OnVerifySignatureRequest(VerifySignatureRequest request)
        {
            System.Console.WriteLine("    > Verify Signature Request");
            System.Console.WriteLine("      [Auto-Accepting Signature...]");
            base.OnVerifySignatureRequest(request);
            cloverConnector.AcceptSignature(request);
        }

        public override void OnConfirmPaymentRequest(ConfirmPaymentRequest request)
        {
            foreach (Challenge challenge in request.Challenges)
            {
                string answer = readLine(challenge.message + " (Y/n)");
                if (string.Equals(answer, "n", StringComparison.OrdinalIgnoreCase))
                {
                    cloverConnector.RejectPayment(request.Payment, challenge);
                }
                else
                {
                    System.Console.WriteLine("Y");
                }
            }
            cloverConnector.AcceptPayment(request.Payment);
        }

        public override void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent)
        {
            System.Console.WriteLine("deviceError: " + deviceErrorEvent.Message);
            done();
        }

        public override void OnDeviceActivityStart(CloverDeviceEvent deviceEvent)
        {
            if (deviceEvent.Message != null)
            {
                System.Console.WriteLine("    > " + deviceEvent.Message);
            }
        }

        public override void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent)
        {
            // can ignore for now...
        }


        protected static String readLine(String message)
        {
            System.Console.Write(message);
            return Console.ReadLine();
        }

        public void done()
        {
            cloverConnector.RemoveCloverConnectorListener(this);
            ewh.Set();
        }
    }

}