using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace com.clover.sdk.v3
{
    public class LoyaltyDataTypes
    {
        // We have some types we define as standards
        public static string VAS_TYPE = "VAS";
        public static string EMAIL_TYPE = "EMAIL";
        public static string PHONE_TYPE = "PHONE";
        public static string BARCODE_TYPE = "BARCODE";
        public static string CLEAR_TYPE = "CLEAR";

        public class VAS_TYPE_KEYS
        {
            public static string PUSH_URL = "PUSH_URL";
            public static string PROTOCOL_CONFIG = "PROTOCOL_CONFIG";
            public static string PROTOCOL_ID = "PROTOCOL_ID";
            public static string PROVIDER_PACKAGE = "PROVIDER_PACKAGE";
            public static string PUSH_TITLE = "PUSH_TITLE";
            public static string SUPPORTED_SERVICES = "SUPPORTED_SERVICES";
        }

        public class VasConfig
        {
            /// <summary>
            /// Identifying id for this config
            /// </summary>
            public string ProviderPackage { get; set; }

            /// <summary>
            /// Protocol of this VAS tap config, currently "ST" for Google Smart Tap or "PK" for Apple Pass Kit. Common values listed in VasProtocol enum for convenience.
            /// </summary>
            public string ProtocolId { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string ProtocolConfig { get; set; }

            /// <summary>
            /// Custom URL to send to Apple/Google identifying merchant per their rules
            /// </summary>
            public string PushUrl { get; set; }
            /// <summary>
            /// Title to send to Apple/Google identifying merchant per their rules
            /// </summary>
            public string PushTitle { get; set; }

            /// <summary>
            /// The specific requested services. Common values listed in VasDataTypeType enum for convenience, for everything use "ALL"
            /// </summary>
            public List<string> SupportedServices { get; set; }

            public Dictionary<string, string> Serialize()
            {
                return new Dictionary<string, string>
                {
                    { VAS_TYPE_KEYS.PUSH_URL, PushUrl },
                    { VAS_TYPE_KEYS.PROTOCOL_CONFIG, ProtocolConfig },
                    { VAS_TYPE_KEYS.PROVIDER_PACKAGE, ProviderPackage },
                    { VAS_TYPE_KEYS.PROTOCOL_ID, ProtocolId },
                    { VAS_TYPE_KEYS.PUSH_TITLE, PushTitle },
                    { VAS_TYPE_KEYS.SUPPORTED_SERVICES, JsonConvert.SerializeObject(SupportedServices ?? new List<string>()) }
                };
            }
        }
    }
}
