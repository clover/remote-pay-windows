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

namespace com.clover.sdk.v3.base_ {


public class CountryInfo {

  /// <summary>
  /// ISO 3166-1-alpha-2 code
  /// </summary>
  public String countryCode { get; set; }

  public String displayName { get; set; }

  public String localDisplayName { get; set; }

  public String defaultCurrency { get; set; }

  public String defaultTimezone { get; set; }

  /// <summary>
  /// Indicates whether the state/province field is required when creating the address
  /// </summary>
  public Boolean stateProvinceRequired { get; set; }

  /// <summary>
  /// Indicates whether the ZIP/Postal code field is required when creating the address
  /// </summary>
  public Boolean zipPostalRequired { get; set; }

  /// <summary>
  /// Indicates whether the county field is required when creating the address
  /// </summary>
  public Boolean countyRequired { get; set; }

  public String defaultLocale { get; set; }

  /// <summary>
  /// Indicates whether the country is enabled for app market billing
  /// </summary>
  public Boolean appMarketBillingEnabled { get; set; }

}

}
