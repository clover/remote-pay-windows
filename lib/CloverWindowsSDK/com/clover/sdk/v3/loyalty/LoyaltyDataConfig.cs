using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.clover.sdk.v3
{
    public class LoyaltyDataConfig
    {
        /// <summary>
        /// The Loyalty API data type
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Configurations related to the Loyalty API data type
        /// </summary>
        public Dictionary<string, string> configuration { get; set; }
    }
}
