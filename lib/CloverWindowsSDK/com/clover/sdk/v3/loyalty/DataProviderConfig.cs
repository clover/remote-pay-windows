using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.clover.sdk.v3
{
    public class DataProviderConfig
    {
        /// <summary>
        /// The Loyalty API data type
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Configurations related to the Loyalty API data type
        /// </summary>
        public Dictionary<string, string> configuration { get; set; } = new Dictionary<string, string>();

        public DataProviderConfig()
        {
        }

        public DataProviderConfig(string type)
        {
            this.type = type;
        }

        public DataProviderConfig(string type, Dictionary<string, string> configuration)
        {
            this.type = type;
            this.configuration = configuration;
        }
    }
}
