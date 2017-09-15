/**
 * Autogenerated by Avro
 * 
 * DO NOT EDIT DIRECTLY
 */

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
using System.Collections.Generic;

namespace com.clover.sdk.v3.inventory {


public class Discount {

  /// <summary>
  /// Unique identifier
  /// </summary>
  public String id { get; set; }

  /// <summary>
  /// Name of the discount
  /// </summary>
  public String name { get; set; }

  /// <summary>
  /// Discount amount in fraction of currency unit (e.g. cents) based on currency fraction digits supported
  /// </summary>
  public Int64 amount { get; set; }

  /// <summary>
  /// Discount amount in percent
  /// </summary>
  public Int64 percentage { get; set; }

}

}
