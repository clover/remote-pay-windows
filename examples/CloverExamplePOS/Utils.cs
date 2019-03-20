using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace CloverExamplePOS
{
    static class Utils
    {
        public static string SummaryReport(object item, bool suppressNulls = true, string prefix = "")
        {
            // Use the json serializer to create a prettily formatted string version of the object, then strip json syntax and null/0 values for readability
            string json = JsonConvert.SerializeObject(item, Formatting.Indented);
            json = Regex.Replace(json, @"""([^""]+)""\s*:", "$1:"); // remove quotes around json names { "key": "value" } -> { key: "value" }
            json = Regex.Replace(json, @"\s*,\s*$", "", RegexOptions.Multiline); // remove trailing commas
            json = Regex.Replace(json, @"^.*:\s*(null|0|false)\s*,?\s*$", "", RegexOptions.Multiline); // remove lines with null or zero values:  "key": null and "key2": 0 
            json = Regex.Replace(json, @"[{}\[\]]", ""); // remove json container syntax {} and []
            json = Regex.Replace(json, @"\r?\n\s*\r?\n", "\n"); // remove blank lines, \n whitespace \n => \n
            json = Regex.Replace(json, @"^(\s*\r?\n)+", ""); // remove leading empty line(s)

            return json;
        }
    }
}
