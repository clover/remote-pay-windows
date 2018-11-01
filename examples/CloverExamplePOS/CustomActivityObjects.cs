using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloverExamplePOS.CustomActivity
{
    // These are the objects used to communicate with the sample Custom Activities 
    // used in this Example Point of Sale
    // talking to the CFP sample apk provided in the Android Example projects on github
    // 
    //   https://github.com/clover/CustomerFacingPlatformExamples
    //   https://github.com/clover/android-examples/tree/master/clover-cfp-examples

    public class CustomerInfo
    {
        public string phoneNumber;
        public string customerName;
    }

    public class CustomerInfoMessage
    {
        public CustomerInfo customerInfo;
        public string payloadClassName;
        public string messageType;
    }

    public class PhoneNumberMessage
    {
        public string phoneNumber;
        public string payloadClassName;
        public MessageType messageType;
    }

    public class Rating
    {
        public string id;
        public string question;
        public int value;
    }

    public class RatingsMessage
    {
        public string payloadClassName;
        public string messageType;
        public Rating[] ratings;
    }

    public class RequestRatingsMessage
    {
        public string payloadClassName;
        public MessageType messageType;
    }

    public class PayloadMessage
    {
        public PayloadMessage(string PayloadClassName, MessageType MessageType)
        {
            payloadClassName = PayloadClassName;
            messageType = MessageType;
        }

        public string payloadClassName;
        public MessageType messageType;
    }

    public class WebViewMessage
    {
        public string payloadClassName;
        public string messageType;
        public string url;
        public string html;
    }

    public enum MessageType
    {
        PHONE_NUMBER,
        RATINGS,
        REQUEST_RATINGS,
        CUSTOMER_INFO,
        CONVERSATION_QUESTION,
        CONVERSATION_RESPONSE,
        WEBVIEW
    }

    public class ConversationQuestionMessage
    {
        public string payloadClassName;
        public string messageType;
        public string message;
    }

    public class ConversationResponseMessage
    {
        public string payloadClassName;
        public string messageType;
        public string message;
    }
}
