// Copyright (C) 2018 Clover Network, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
//
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using com.clover.remotepay.transport;

namespace com.clover.remotepay.sdk
{
    /// <summary>
    /// Factory to create an instance of the CloverConnector.
    /// </summary>
    public class CloverConnectorFactory
    {
        /// <summary>
        /// Factory to create an instance of the CloverConnector
        /// </summary>
        /// <param name="config">Object that conveys the required information used by the connector.
        ///     Usually a USBCloverDeviceConfiguration (USB connection) or WebSocketCloverDeviceConfiguration (Network/SNPD connection)</param>
        /// <returns>Initialized instance conforming to the ICloverConnector</returns>
        public static ICloverConnector createICloverConnector(CloverDeviceConfiguration config)
        {
            return new CloverConnector(config);
        }

        /// <summary>
        /// Create a USB configured connection CloverConnector to a Clover Device running USB Pay Display (UsbPD)
        /// Convenience wrapper around creating a USBCloverDeviceConfiguration object and calling CloverConnectorFactory.createICloverConnector
        /// </summary>
        /// <param name="remoteApplicationId">Application ID for server reporting</param>
        /// <param name="posName">Point of Sale name for server reporting</param>
        /// <param name="serialNumber">Station ID / serial number for server reporting</param>
        /// <param name="enableLogging">Turn logging on or off</param>
        /// <returns></returns>
        public static ICloverConnector CreateUsbConnector(string remoteApplicationId, string posName, string serialNumber, bool enableLogging = false)
        {
            USBCloverDeviceConfiguration config = new USBCloverDeviceConfiguration("", remoteApplicationId, posName, serialNumber, enableLogging);
            return createICloverConnector(config);
        }

        /// <summary>
        /// Create a WebSockect configured connection CloverConnector to a Clover Device running Secure Network Pay Display (SNPD) 
        /// Convenience wrapper around creating a WebSocketCloverDeviceConfiguration object and calling CloverConnectorFactory.createICloverConnector
        /// </summary>
        /// <param name="endpoint">Clover Device Secure Network Pay Display (SNPD) network address, usually similar to "https://192.168.0.1:1234/remote_pay"</param>
        /// <param name="remoteApplicationId">Application ID for server reporting</param>
        /// <param name="posName">Point of Sale name for server reporting</param>
        /// <param name="serialNumber">Station ID / serial number for server reporting</param>
        /// <param name="pairingAuthToken">Previous paired auth token to allow quick reconnection without initiating antoerh pairing. Blank value, invalid or expired token will initiate a new pairing</param>
        /// <param name="pairingCodeHandler">Delegate method called with temporary pairing code to enter on device, like `6341` - display to user to enter on Clover Device to complete pairing</param>
        /// <param name="pairingSuccessHandler">Delegate method called with reconnection auth token when pairing has succeeded</param>
        /// <param name="pairingStateHandler">Delegate method called during pairing flow transitions</param>
        /// <param name="enableLogging">Turn logging on or off</param>
        /// <returns></returns>
        public static ICloverConnector CreateWebSocketConnector(string endpoint, string remoteApplicationId, string posName, string serialNumber, string pairingAuthToken, PairingDeviceConfiguration.OnPairingCodeHandler pairingCodeHandler, PairingDeviceConfiguration.OnPairingSuccessHandler pairingSuccessHandler, PairingDeviceConfiguration.OnPairingStateHandler pairingStateHandler, bool enableLogging = false)
        {
            WebSocketCloverDeviceConfiguration config = new WebSocketCloverDeviceConfiguration(endpoint, remoteApplicationId, enableLogging, 1, posName, serialNumber, pairingAuthToken, pairingCodeHandler, pairingSuccessHandler, pairingStateHandler);
            return createICloverConnector(config);
        }
    }
}
