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

using System.Security.Cryptography;
using System.Text;

namespace com.clover.remotepay.sdk
{
    /// <summary>
    /// Used for generating a unique string of the length specified
    /// Uses the linear congruential generator (LCG) algorithm
    /// in conjunction with the .NET RNGCryptoServiceProvider class.
    /// </summary>
    public static class ExternalIDUtil
    {
        /// <summary>
        /// Generates the random string.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string GenerateRandomString(int length)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();

            byte[] data = new byte[length];
            crypto.GetNonZeroBytes(data);

            StringBuilder result = new StringBuilder(length);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }

            return result.ToString();
        }
    }
}
