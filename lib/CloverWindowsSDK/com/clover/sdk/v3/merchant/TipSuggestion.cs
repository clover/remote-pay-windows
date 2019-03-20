using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.clover.sdk.v3.merchant
{
    public class TipSuggestion
    {
        /// <summary>
        /// Id can be anything, but best case is a java-uuid friendly Guid string.
        /// </summary>
        public string id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// Tip button title to display to user
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Tip percentage for this tip suggestion button
        /// </summary>
        public long percentage { get; set; }

        /// <summary>
        /// isEnabled is currently ignored in the context of remote-pay, but setting to true will avoid future surprises if that changes.
        /// </summary>
        public bool isEnabled { get; set; } = true;
    }
}
