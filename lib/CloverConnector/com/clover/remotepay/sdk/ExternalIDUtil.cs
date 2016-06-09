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

namespace com.clover.remotepay.sdk
{
    /// <summary>
    /// Used for generating a unique string of the length specified
    /// </summary>
    public static class ExternalIDUtil
    {
        static Random randomGenerator = new Random();
        /// <summary>
        /// Generates the random string.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string GenerateRandomString(int length)
        {
            byte[] randomBytes = new byte[randomGenerator.Next(length)];
            randomGenerator.NextBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
