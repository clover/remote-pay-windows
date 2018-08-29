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

namespace com.clover.sdk.v3.payments
{
    public class AdditionalCharge
    {
        public String id { get; set; }
        public com.clover.sdk.v3.base_.Reference merchant { get; set; }
        public AdditionalChargeType type { get; set; }
        public Int64 amount { get; set; }
        public Int64 percentageDecimal { get; set; }
        public Boolean isCashbackOnly { get; set; }
        public Boolean enabled { get; set; }
        public Int64 modifiedTime { get; set; }
        public Int64 deletedTime { get; set; }
    }
}
