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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace com.clover.remotepay.transport
{
    /// <summary>
    /// Json custom parsing utility class for the json objects passed to/from the transport
    /// </summary>
    public class JsonUtils
    {
        public static T deserializeSDK<T>(string input)
        {
            return deserialize<T>(input, new JsonConverter[] { new OrderConverter(), new PaymentConverter(), new RefundConverter(), new CreditConverter(), new BatchConverter(), new VaultedCardConverter(), new PrinterConverter(), new StringEnumConverter() });
        }

        public static T deserialize<T>(string input)
        {
            return deserialize<T>(input, new JsonConverter[] { new StringEnumConverter() });
        }

        public static T deserialize<T>(string input, JsonConverter[] converters)
        {
            return JsonConvert.DeserializeObject<T>(input, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore, Converters = converters});
        }

        public static string serialize(object toSer)
        {
            return serialize(toSer, new JsonConverter[] { new StringEnumConverter() });
        }

        public static string serializeSDK(object toSer)
        {
            return serialize(toSer, new JsonConverter[] { new OrderConverter(), new PaymentConverter(), new RefundConverter(), new CreditConverter(), new BatchConverter(), new VaultedCardConverter(), new PrinterConverter(), new StringEnumConverter() });
        }

        public static string serialize(object toSer, JsonConverter[] converters)
        {
            return JsonConvert.SerializeObject(toSer, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Converters = converters });
        }
    }

    /// <summary>
    /// Custom Json parsing for Payment
    /// </summary>
    public class PaymentConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(com.clover.sdk.v3.payments.Payment));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            string str = reader.Value.ToString();
            com.clover.sdk.v3.payments.Payment result = JsonUtils.deserialize<com.clover.sdk.v3.payments.Payment>(str, new JsonConverter[] {
                new ListConverter<com.clover.sdk.v3.payments.PaymentTaxRate>(),
                new ListConverter<com.clover.sdk.v3.payments.Refund>(),
                new ListConverter<com.clover.sdk.v3.payments.LineItemPayment>()
            });
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(JsonUtils.serialize(value));
        }
    }

    /// <summary>
    /// Custom Json parsing for Credit
    /// </summary>
    public class CreditConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(com.clover.sdk.v3.payments.Credit));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            string str = reader.Value.ToString();
            com.clover.sdk.v3.payments.Credit result = JsonUtils.deserialize<com.clover.sdk.v3.payments.Credit>(str, new JsonConverter[] { new ListConverter<com.clover.sdk.v3.payments.TaxableAmountRate>() });
            return result;

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(JsonUtils.serialize(value));
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

            string str = reader.Value.ToString();
            com.clover.sdk.v3.order.Order result = JsonUtils.deserialize<com.clover.sdk.v3.order.Order>(str, new JsonConverter[] {
                new ListConverter<com.clover.sdk.v3.order.LineItem>(),
                new ListConverter<com.clover.sdk.v3.order.OrderTaxRate>(),
                new ListConverter<com.clover.sdk.v3.order.Modification>(),
                new ListConverter<com.clover.sdk.v3.order.Discount>(),
                new ListConverter<com.clover.sdk.v3.inventory.TaxRate>(),
                new ListConverter<com.clover.sdk.v3.customers.Customer>(),
                new ListConverter<com.clover.sdk.v3.payments.Payment>(),
                new ListConverter<com.clover.sdk.v3.payments.LineItemPayment>(),
                new ListConverter<com.clover.sdk.v3.payments.Refund>(),
                new ListConverter<com.clover.sdk.v3.payments.Credit>(),
                new ListConverter<com.clover.sdk.v3.payments.PaymentTaxRate>()
            });
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(JsonUtils.serialize(value));
        }
    }

    /// <summary>
    /// Custom Json parsing for Refund
    /// </summary>
    public class RefundConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(com.clover.sdk.v3.payments.Refund));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            string str = reader.Value.ToString();
            com.clover.sdk.v3.payments.Refund result = JsonUtils.deserialize<com.clover.sdk.v3.payments.Refund>(str, new JsonConverter[] { new ListConverter<com.clover.sdk.v3.payments.TaxableAmountRate>(), new ListConverter<com.clover.sdk.v3.base_.Reference>() }); // reference for line items
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(JsonUtils.serialize(value));
        }
    }

    /// <summary>
    /// Custom Json parsing for VaultedCard
    /// </summary>
    public class VaultedCardConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(com.clover.sdk.v3.payments.VaultedCard));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            string str = reader.Value.ToString();
            com.clover.sdk.v3.payments.VaultedCard result = JsonUtils.deserialize<com.clover.sdk.v3.payments.VaultedCard>(str, new JsonConverter[] { }); // reference for line items
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(JsonUtils.serialize(value));
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

            string str = reader.Value.ToString();
            com.clover.sdk.v3.printer.Printer result = JsonUtils.deserialize<com.clover.sdk.v3.printer.Printer>(str, new JsonConverter[] { });
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(JsonUtils.serialize(value));
        }
    }

    /// <summary>
    /// Custom Json parser for Batch
    /// </summary>
    public class BatchConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(com.clover.sdk.v3.payments.Batch));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            string str = reader.Value.ToString();
            com.clover.sdk.v3.payments.Batch result = JsonUtils.deserialize<com.clover.sdk.v3.payments.Batch>(str, new JsonConverter[] { new ListConverter<com.clover.sdk.v3.payments.TaxableAmountRate>(), new ListConverter<com.clover.sdk.v3.payments.ServerTotalStats>(), new ListConverter<com.clover.sdk.v3.payments.BatchCardTotal>() });
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(JsonUtils.serialize(value));
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
            return (objectType == typeof(System.Collections.Generic.List<T>));
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
            writer.WriteValue(JsonUtils.serialize(value));
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
}
