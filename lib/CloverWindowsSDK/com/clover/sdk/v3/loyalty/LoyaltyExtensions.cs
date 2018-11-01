using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.clover.sdk.v3
{
    public static class LoyaltyExtensions
    {
        public static List<LoyaltyDataConfig> AsLoyaltyDataConfig(this IEnumerable<DataProviderConfig> providers)
        {
            List<LoyaltyDataConfig> configs = new List<LoyaltyDataConfig>();
            if (providers != null)
            {
                foreach (DataProviderConfig provider in providers)
                {
                    configs.Add(provider.AsLoyaltyDataConfig());
                }
            }
            return configs;
        }

        public static LoyaltyDataConfig AsLoyaltyDataConfig(this DataProviderConfig provider)
        {
            LoyaltyDataConfig loyaltyDataConfig = new LoyaltyDataConfig
            {
                type = provider.type,
                configuration = provider.configuration
            };
            return loyaltyDataConfig;
        }
    }
}
