// Copyright (C) 2018 Clover Network, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
//
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using com.clover.remotepay.transport;

namespace com.clover.remotepay.sdk
{
    public class CloverDeviceEvent
    {
        public enum DeviceEventState
        {
            START,
            FAILED,
            FATAL,
            TRY_AGAIN,
            INPUT_ERROR,
            PIN_BYPASS_CONFIRM,
            CANCELED,
            TIMED_OUT,
            DECLINED,
            VOIDED,
            CONFIGURING,
            PROCESSING,
            REMOVE_CARD,
            PROCESSING_GO_ONLINE,
            PROCESSING_CREDIT,
            PROCESSING_SWIPE,
            SELECT_APPLICATION,
            PIN_PAD,
            MANUAL_CARD_NUMBER,
            MANUAL_CARD_CVV,
            MANUAL_CARD_CVV_UNREADABLE,
            MANUAL_CARD_EXPIRATION,
            SELECT_ACCOUNT,
            CASHBACK_CONFIRM,
            CASHBACK_SELECT,
            CONTACTLESS_TAP_REQUIRED,
            VOICE_REFERRAL_RESULT,
            CONFIRM_PARTIAL_AUTH,
            PACKET_EXCEPTION,
            CONFIRM_DUPLICATE_CHECK,
            VERIFY_SIGNATURE_ON_PAPER,
            VERIFY_SIGNATURE_ON_PAPER_CONFIRM_VOID,
            VERIFY_SIGNATURE_ON_SCREEN,
            VERIFY_SIGNATURE_ON_SCREEN_CONFIRM_VOID,
            ADD_SIGNATURE,
            SIGNATURE_ON_SCREEN_FALLBACK,
            RETURN_TO_MERCHANT,
            SIGNATURE_REJECT,
            ADD_SIGNATURE_CANCEL_CONFIRM,
            STARTING_CUSTOM_ACTIVITY,
            START_QR_CODE_MODE,
            CUSTOM_ACTIVITY,
            ADD_TIP,
            RECEIPT_OPTIONS,
            HANDLE_TENDER,
            SELECT_WITHDRAW_FROM_ACCOUNT,
            VERIFY_SURCHARGES,
            VOID_CONFIRM,

            // for Argentina
            ENTER_PAN_LAST_FOUR,
            ERROR_SCREEN,
            FISCAL_INVOICE_NUMBER,
            ENTER_INSTALLMENTS,
            SELECT_INSTALLMENT_PLAN,
            ENTER_INSTALLMENT_CODE,
            PERSONAL_ID_ENTRY,
            PERSONAL_ID_ENTRY_PAS,
            SWIPE_CVV_ENTRY,
            SIGNATURE_CUSTOMER_MODE,
            MANUAL_ENTRY_FALLBACK,
            SELECT_MULTI_MID
        }

        public CloverDeviceEvent()
        {

        }
        public CloverDeviceEvent(int code, string msg)
        {
            Code = code;
            Message = msg;
        }
        public DeviceEventState EventState { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public Object[] Options { get; set; }
        public InputOption[] InputOptions { get; set; }
    }

    public class CloverDeviceErrorEvent
    {
        //
        // Summary:
        //     The USB device has errors in its Configuration descriptor.
        public static int InvalidConfig = -16384;
        //
        // Summary:
        //     A synchronous device IO operation failed.
        public static int IoSyncFailed = -16383;
        //
        // Summary:
        //     A request for a string failed.
        public static int GetString = -16382;
        //
        // Summary:
        //     A specified endpoint is invalid for the operation.
        public static int InvalidEndpoint = -16381;
        //
        // Summary:
        //     A request to cancel IO operation failed.
        public static int AbortEndpoint = -16380;
        //
        // Summary:
        //     A call to the core Win32 API DeviceIOControl failed.
        public static int DeviceIoControl = -16379;
        //
        // Summary:
        //     A call to the core Win32 API GetOverlappedResult failed.
        public static int GetOverlappedResult = -16378;
        //
        // Summary:
        //     An Endpoints receive thread was dangerously terminated.
        public static int ReceiveThreadTerminated = -16377;
        //
        // Summary:
        //     A write operation failed.
        public static int WriteFailed = -16376;
        //
        // Summary:
        //     A read operation failed.
        public static int ReadFailed = -16375;
        //
        // Summary:
        //     An endpoint 0 IO control message failed.
        public static int IoControlMessage = -16374;
        //
        // Summary:
        //     The action of cancelling the IO operation failed.
        public static int CancelIoFailed = -16373;
        //
        // Summary:
        //     An IO operation was cancelled by the user before it completed.
        //
        // Remarks:
        //     IoCancelled errors may occur as normal operation; for this reason they are not
        //     logged as a LibUsbDotNet.UsbError.
        public static int IoCancelled = -16372;
        //
        // Summary:
        //     An IO operation timed out before it completed.
        //
        // Remarks:
        //     IoTimedOut errors may occur as normal operation; for this reason they are not
        //     logged as a LibUsbDotNet.UsbError.
        public static int IoTimedOut = -16371;
        //
        // Summary:
        //     An IO operation was cancelled and will be re-submiited when ready.
        //
        // Remarks:
        //     IoEndpointGlobalCancelRedo errors may occur as normal operation; for this reason
        //     they are not logged as a LibUsbDotNet.UsbError.
        public static int IoEndpointGlobalCancelRedo = -16370;
        //
        // Summary:
        //     Failed retrieving a custom USB device key value.
        public static int GetDeviceKeyValueFailed = -16369;
        //
        // Summary:
        //     Failed setting a custom USB device key value.
        public static int SetDeviceKeyValueFailed = -16368;
        //
        // Summary:
        //     The error is a standard windows error.
        public static int Win32Error = -16367;
        //
        // Summary:
        //     An attempt was made to lock a device that is already locked.
        public static int DeviceAllreadyLocked = -16366;
        //
        // Summary:
        //     An attempt was made to lock an endpoint that is already locked.
        public static int EndpointAllreadyLocked = -16365;
        //
        // Summary:
        //     The USB device request failed because the USB device was not found.
        public static int DeviceNotFound = -16364;
        //
        // Summary:
        //     Operation was intentionally cancelled by the user or application.
        public static int UserAborted = -16363;
        //
        // Summary:
        //     Invalid parameter.
        public static int InvalidParam = -16362;
        //
        // Summary:
        //     Access denied (insufficient permissions).
        public static int AccessDenied = -16361;
        //
        // Summary:
        //     Resource Busy.
        public static int ResourceBusy = -16360;
        //
        // Summary:
        //     Overflow.
        public static int Overflow = -16359;
        //
        // Summary:
        //     Pipe error or endpoint halted.
        public static int PipeError = -16358;
        //
        // Summary:
        //     System call interrupted (perhaps due to signal).
        public static int Interrupted = -16357;
        //
        // Summary:
        //     Insufficient memory.
        public static int InsufficientMemory = -16356;
        //
        // Summary:
        //     Operation not supported or unimplemented on this platform.
        public static int NotSupported = -16355;
        //
        // Summary:
        //     Unknown or other error.
        public static int UnknownError = -16354;
        //
        // Summary:
        //     The error is one of the MonoLibUsb.MonoUsbError
        public static int MonoApiError = -16353;
        //
        // Summary:
        //     No error. (None, Success, and Ok)
        public static int None = 0;
        //
        // Summary:
        //     No error.
        public static int Success = 0;
        //
        // Summary:
        //     No error.
        public static int Ok = 0;

        public enum CloverDeviceErrorType
        {
            COMMUNICATION_ERROR,
            VALIDATION_ERROR,
            EXCEPTION
        }

        public CloverDeviceErrorEvent()
        {

        }
        public CloverDeviceErrorEvent(CloverDeviceErrorType errorType, int code, Exception cause, string msg)
        {
            ErrorType = errorType;
            Code = code;
            Message = msg;
            Cause = cause;
        }

        public CloverDeviceErrorType ErrorType { get; set; }
        public int? Code { get; set; }
        public Exception Cause { get; set; }
        public string Message { get; set; }
    }
}
