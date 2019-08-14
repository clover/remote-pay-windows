using System;
using com.clover.remotepay.sdk;

namespace CloverExamplePOS
{
    public class ResponseEventArgs<T> : EventArgs
        where T : BaseResponse
    {
        public T Response { get; set; }
    }

    public static class ResponseEventArgs
    {
        public static ResponseEventArgs<T> From<T>(T response)
            where T : BaseResponse
        {
            return new ResponseEventArgs<T> { Response = response };
        }
    }
}
