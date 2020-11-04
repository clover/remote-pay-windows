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
using System.Diagnostics;
using System.Reflection;
using com.clover.sdk.v3.inventory;
using com.clover.sdk.v3.merchant;
using com.clover.sdk.v3.payments;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace com.clover.remotepay.transport
{
    /// <summary>
    /// Json custom parsing utility class for the json objects passed to/from the transport
    /// </summary>
    public class JsonUtils
    {
        public static T Deserialize<T>(string input, JsonConverter[] converters) => JsonConvert.DeserializeObject<T>(input, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, CheckAdditionalContent = false, MissingMemberHandling = MissingMemberHandling.Ignore, Converters = converters });
        public static T Deserialize<T>(string input) => Deserialize<T>(input, new JsonConverter[] { new StringEnumConverter() });
        public static T DeserializeSdk<T>(string input) => Deserialize<T>(input, StandardJsonConverters);

        public static string Serialize(object value, JsonConverter[] converters) => JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Converters = converters });
        public static string Serialize(object value) => Serialize(value, new JsonConverter[] { new StringEnumConverter() });
        public static string SerializeSdk(object value) => Serialize(value, StandardJsonConverters);

        public static JsonConverter[] StandardJsonConverters =>
            new JsonConverter[]
            {
                new OrderConverter(),
                new PaymentConverter(),
                new RefundConverter(),
                new CreditConverter(),
                new BatchConverter(),
                new VaultedCardConverter(),
                new PrinterConverter(),
                new AuthorizationConverter(),
                new StringEnumConverter(),
                new DataProviderConfigConverter(),

                new ListConverter<TaxRate>(),
                new ListConverter<PaymentTaxRate>(),
                new ListConverter<TipSuggestion>(),
                new ListConverter<PaymentTaxRate>(),
                new ListConverter<Refund>(),
                new ListConverter<LineItemPayment>(),
                new ListConverter<AdditionalChargeAmount>()
            };
    }

    /// <summary>
    /// Custom Json parsing for Payment
    /// </summary>
    public class PaymentConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Payment);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            string json = reader.Value.ToString();
            Payment result = JsonUtils.Deserialize<Payment>(json, LocalJsonConverters);
            return result;
        }

        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // writer.WriteValue(JsonUtils.Serialize(value, LocalJsonConverters));
        }

        public static JsonConverter[] LocalJsonConverters =>
            new JsonConverter[]
            {
                new ListConverter<TaxRate>(),
                new ListConverter<PaymentTaxRate>(),
                new ListConverter<TaxableAmountRate>(),
                new ListConverter<TipSuggestion>(),
                new ListConverter<PaymentTaxRate>(),
                new ListConverter<Refund>(),
                new ListConverter<LineItemPayment>(),
                new ListConverter<AdditionalChargeAmount>()
            };
    }

    /// <summary>
    /// Custom Json parsing for Credit
    /// </summary>
    public class CreditConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Credit));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            string json = reader.Value.ToString();
            Credit result = JsonUtils.Deserialize<Credit>(json, new JsonConverter[] { new ListConverter<TaxableAmountRate>() });
            return result;

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(JsonUtils.Serialize(value));
        }
    }

    /// <summary>
    /// Custom Json parsing for Order
    /// </summary>
    public class OrderConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(com.clover.sdk.v3.order.Order));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            string json = reader.Value.ToString();
            com.clover.sdk.v3.order.Order result = JsonUtils.Deserialize<com.clover.sdk.v3.order.Order>(json, new JsonConverter[] {
                new ListConverter<com.clover.sdk.v3.order.LineItem>(),
                new ListConverter<com.clover.sdk.v3.order.OrderTaxRate>(),
                new ListConverter<com.clover.sdk.v3.order.Modification>(),
                new ListConverter<com.clover.sdk.v3.order.Discount>(),
                new ListConverter<com.clover.sdk.v3.inventory.TaxRate>(),
                new ListConverter<com.clover.sdk.v3.customers.Customer>(),
                new ListConverter<TaxableAmountRate>(),
                new ListConverter<Payment>(),
                new ListConverter<LineItemPayment>(),
                new ListConverter<Refund>(),
                new ListConverter<Credit>(),
                new ListConverter<TipSuggestion>(),
                new ListConverter<PaymentTaxRate>()
            });
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(JsonUtils.Serialize(value));
        }
    }

    /// <summary>
    /// Custom Json parsing for Refund
    /// </summary>
    public class RefundConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Refund));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            string json = reader.Value.ToString();
            Refund result = JsonUtils.Deserialize<Refund>(json, new JsonConverter[] { new ListConverter<TaxableAmountRate>(), new ListConverter<com.clover.sdk.v3.base_.Reference>() }); // reference for line items
            return result;
        }

        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // writer.WriteValue(JsonUtils.Serialize(value));
        }
    }

    /// <summary>
    /// Custom Json parsing for VaultedCard
    /// </summary>
    public class VaultedCardConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(VaultedCard));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            string json = reader.Value.ToString();
            VaultedCard result = JsonUtils.Deserialize<VaultedCard>(json, new JsonConverter[] { }); // reference for line items
            return result;
        }

        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // writer.WriteValue(JsonUtils.Serialize(value));
        }
    }

    /// <summary>
    /// Custom Json parsing for Printer
    /// </summary>
    public class PrinterConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(com.clover.sdk.v3.printer.Printer));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            string json = reader.Value.ToString();
            com.clover.sdk.v3.printer.Printer result = JsonUtils.Deserialize<com.clover.sdk.v3.printer.Printer>(json, new JsonConverter[] { });
            return result;
        }

        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // writer.WriteValue(JsonUtils.Serialize(value));
        }
    }

    /// <summary>
    /// Custom Json parsing for Authorization
    /// </summary>
    public class AuthorizationConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(com.clover.sdk.v3.payments.Authorization));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            string json = reader.Value.ToString();
            com.clover.sdk.v3.payments.Authorization result = JsonUtils.Deserialize<com.clover.sdk.v3.payments.Authorization>(json, new JsonConverter[] { });
            return result;
        }

        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // writer.WriteValue(JsonUtils.Serialize(value));
        }
    }

    /// <summary>
    /// Custom Json parser for Batch
    /// </summary>
    public class BatchConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Batch));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            string json = reader.Value.ToString();
            Batch result = JsonUtils.Deserialize<Batch>(json, new JsonConverter[] { new ListConverter<TaxableAmountRate>(), new ListConverter<ServerTotalStats>(), new ListConverter<BatchCardTotal>() });
            return result;
        }

        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // writer.WriteValue(JsonUtils.Serialize(value));
        }
    }

    /// <summary>
    /// Custom Json parser for Lists of custom objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<T>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                ElementsWrapper<T> wrapper = (ElementsWrapper<T>)serializer.Deserialize(reader, typeof(ElementsWrapper<T>)); // If ElementsWrapper used a List<T>, it would recursively call here, so T[] resolves that recursive issue without creating a new deserializer
                return new List<T>(wrapper.elements);
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ElementsWrapper<T> wrapper = new ElementsWrapper<T>();
            wrapper.elements = ((List<T>)value).ToArray();
            serializer.Serialize(writer, wrapper);
        }
    }

    /// <summary>
    /// Generic elements array wrapper used in the above ListConverter to avoid recursive custom deserializer creations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class ElementsWrapper<T>
    {
        public T[] elements { get; set; }
    }

    /// <summary>
    /// Custom Json parser for DataProviderConfig - encoded as a string with json, needs unwrapping
    /// </summary>
    public class DataProviderConfigConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(com.clover.sdk.v3.DataProviderConfig));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            string json = reader.Value.ToString();
            com.clover.sdk.v3.DataProviderConfig  result = JsonUtils.Deserialize<com.clover.sdk.v3.DataProviderConfig>(json);
            return result;
        }

        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // writer.WriteValue(JsonUtils.Serialize(value));
        }
    }

}
