using com.clover.sdk.v3.payments;
using Newtonsoft.Json;
using System;

namespace TestJson
{
    class Program
    {
        static void Main(string[] args)
        {

            String message = "{\"result\":\"SUCCESS\",\"createdTime\":1442532383839,\"taxRates\":null,\"cardTransaction\":{\"authCode\":\"78708\",\"entryType\":\"EMV_CONTACT\",\"extra\":{\"cvmResult\":\"PIN\",\"applicationIdentifier\":\"A0000000031010\"},\"state\":\"CLOSED\",\"referenceId\":\"908373454\",\"type\":\"AUTH\",\"transactionNo\":null,\"last4\":\"1650\",\"cardType\":\"VISA\"},\"offline\":false,\"cashTendered\":null,\"amount\":255,\"id\":\"G35Z224EA0QTG\",\"tipAmount\":0,\"lineItemPayments\":null,\"taxAmount\":0,\"order\":{\"id\":\"NGBY919ENZTFM\"},\"tender\":{\"id\":\"PB00TG1DFNHJC\",\"enabled\":true,\"visible\":true,\"instructions\":null,\"labelKey\":\"com.clover.tender.credit_card\",\"label\":\"Credit Card\",\"opensCashDrawer\":false,\"editable\":false},\"employee\":{\"id\":\"DFLTEMPLOYEE\"},\"externalPaymentId\":null,\"cashbackAmount\":null}";

            Payment fokmsg = deserialize<Payment>(message);
            
        }
        private static T deserialize<T>(String input)
        {

            T result = JsonConvert.DeserializeObject<T>(input,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            });
            return result;

        }
    }
}
