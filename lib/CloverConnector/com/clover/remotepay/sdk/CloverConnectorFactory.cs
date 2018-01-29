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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.clover.remotepay.transport;

namespace com.clover.remotepay.sdk
{
    /// <summary>
    /// Factory to create an instance of the CloverConnector.
    /// </summary>
    public class CloverConnectorFactory
    {
        /// Factory to create an instance of the CloverConnector
        ///
        /// - Parameter config: Object that conveys the required information used by the connector
        /// - Returns: Initialized instance conforming to the ICloverConnector
        /// 
        public static ICloverConnector createICloverConnector(CloverDeviceConfiguration config)
        {
            return new CloverConnector(config);
        }
    }
}
