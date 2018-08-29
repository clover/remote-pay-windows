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
/**
 * 
 * Utility class that provides helper methods for commonly needed operations
 * 
 */
namespace CloverExampleCLI
{
    public class Utils
    {
        public static string currencyFormat(long? value)
        {
            double amount = (double)value / 100;
            return string.Format("{0:C}", amount);
        }

        public static bool stringEqualsIgnoreCase(string s1, string s2)
        {
            return (string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase));
        }
    }
}
