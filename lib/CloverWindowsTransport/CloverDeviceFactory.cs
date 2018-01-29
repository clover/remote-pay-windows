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

namespace com.clover.remotepay.transport
{
    public static class CloverDeviceFactory
    {
        public static CloverDevice Get(CloverDeviceConfiguration configuration)
        {
            string name = configuration.getCloverDeviceTypeName();
            Type deviceType = Type.GetType(name);

            if (deviceType == null)
            {
                throw new ArgumentException($"Cannot locate type \"{name}\" specified in Configuration.");
            }

            return (CloverDevice)Activator.CreateInstance(deviceType, configuration);
        }
    }
}
