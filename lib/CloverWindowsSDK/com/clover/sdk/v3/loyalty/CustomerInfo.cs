using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.clover.sdk.v3.customers;

namespace com.clover.sdk.v3
{
    public class CustomerInfo
    {
        public Customer customer { get; set; }
        public string displayString { get; set; }
        public string externalId { get; set; }
        public string externalSystemName { get; set; }
        public Dictionary<string, string> extras { get; set; } = new Dictionary<string, string>();
    }
}
