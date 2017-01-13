// Copyright (C) 2016 Clover Network, Inc.
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

using System;
using com.clover.remotepay.transport;

namespace com.clover.remotepay.sdk
{
    public class MerchantInfo
    {
        public DeviceInfo Device { get; set; }

        public String merchantID { get; set; }
        public String merchantName { get; set; }
        public String merchantMId { get; set; }

        public bool supportsPreAuths { get; set; }
        public bool supportsVaultCards { get; set; }
        public bool supportsManualRefunds { get; set; }
        public bool supportsTipAdjust { get; set; }
        public bool supportsRemoteConfirmation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MerchantInfo()
        {
            supportsManualRefunds = true;
            supportsTipAdjust = true;
            supportsVaultCards = true;
            supportsPreAuths = true;
            supportsRemoteConfirmation = false;
            merchantID = "";
            merchantMId = "";
            merchantName = "";

            Device = new DeviceInfo();
            Device.Name = "";
            Device.Serial = "";
            Device.Model = "";
        }
        /// <summary>
        /// Contains merchant information about the device
        /// </summary>
        /// <param name="drm"></param>
        public MerchantInfo(DiscoveryResponseMessage drm)
        {
            supportsManualRefunds = drm.supportsManualRefund;
            supportsTipAdjust = drm.supportsTipAdjust;
            supportsVaultCards = drm.supportsMultiPayToken;
            supportsPreAuths = drm.supportsTipAdjust;
            supportsRemoteConfirmation = drm.supportsRemoteConfirmation;
            merchantID = drm.merchantId;
            merchantMId = drm.merchantMId;
            merchantName = drm.merchantName;

            Device = new DeviceInfo();
            Device.Name = drm.name;
            Device.Serial = drm.serial;
            Device.Model = drm.model;
            Device.SupportsAcks = drm.supportsAcknowledgement;
        }

    }

    /// <summary>
    /// Contains the device information of the connected device
    /// </summary>
    public class DeviceInfo
    {
        /// <summary>
        /// The merchant assigned name of the device
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The serial number of the device
        /// </summary>
        public String Serial { get; set; }

        /// <summary>
        /// The model identifier of the device
        /// </summary>
        public String Model { get; set; }

        /// <summary>
        /// Remote pay version supports message acks
        /// </summary>
        public bool SupportsAcks { get; set; }
    }
    /// <summary>
    /// Descriptive information about this SDK
    /// </summary>
    public class SDKInfo
    {
        /// <summary>
        /// Gets or sets the SDK name.
        /// </summary>
        /// <value>
        /// The SDK name.
        /// </value>
        public String Name { get; set; }
        /// <summary>
        /// Gets or sets the SDK version.
        /// </summary>
        /// <value>
        /// The SDK version.
        /// </value>
        public String Version { get; set; }
    }
}
