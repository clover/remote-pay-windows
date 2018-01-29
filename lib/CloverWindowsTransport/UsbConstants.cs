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

namespace com.clover.remotepay.transport
{
    class UsbConstants
    {
        /// <summary>
        /// Bitmask used for extracting the { @link UsbEndpoint }
        /// direction from its address field.
        /// @see UsbEndpoint#getAddress
        /// @see UsbEndpoint#getDirection
        /// @see #USB_DIR_OUT
        /// @see #USB_DIR_IN
        /// </summary>
        public static readonly int USB_ENDPOINT_DIR_MASK = 0x80;

        /// <summary>
        /// Used to signify direction of data for a {@link UsbEndpoint} is OUT (host to device)
        /// @see UsbEndpoint#getDirection
        /// </summary>
        public static readonly int USB_DIR_OUT = 0;

        /// <summary>
        /// Used to signify direction of data for a {@link UsbEndpoint} is IN (device to host)
        /// @see UsbEndpoint#getDirection
        /// </summary>
        public static readonly int USB_DIR_IN = 0x80;

        /// <summary>
        /// Bitmask used for extracting the {@link UsbEndpoint} number its address field.
        /// @see UsbEndpoint#getAddress
        /// @see UsbEndpoint#getEndpointNumber
        /// </summary>
        public static readonly int USB_ENDPOINT_NUMBER_MASK = 0x0f;

        /// <summary>
        /// Bitmask used for extracting the {@link UsbEndpoint} type from its address field.
        /// @see UsbEndpoint#getAddress
        /// @see UsbEndpoint#getType
        /// @see #USB_ENDPOINT_XFER_CONTROL
        /// @see #USB_ENDPOINT_XFER_ISOC
        /// @see #USB_ENDPOINT_XFER_BULK
        /// @see #USB_ENDPOINT_XFER_INT
        /// </summary>
        public static readonly int USB_ENDPOINT_XFERTYPE_MASK = 0x03;

        /// <summary>
        /// Control endpoint type (endpoint zero)
        /// @see UsbEndpoint#getType
        /// </summary>
        public static readonly int USB_ENDPOINT_XFER_CONTROL = 0;

        /// <summary>
        /// Isochronous endpoint type (currently not supported)
        /// @see UsbEndpoint#getType
        /// </summary>
        public static readonly int USB_ENDPOINT_XFER_ISOC = 1;

        /// <summary>
        /// Bulk endpoint type
        /// @see UsbEndpoint#getType
        /// </summary>
        public static readonly int USB_ENDPOINT_XFER_BULK = 2;

        /// <summary>
        /// Interrupt endpoint type
        /// @see UsbEndpoint#getType
        /// </summary>
        public static readonly int USB_ENDPOINT_XFER_INT = 3;

        /// <summary>
        /// Bitmask used for encoding the request type for a control request on endpoint zero.
        /// </summary>
        public static readonly int USB_TYPE_MASK = (0x03 << 5);

        /// <summary>
        /// Used to specify that an endpoint zero control request is a standard request.
        /// </summary>
        public static readonly int USB_TYPE_STANDARD = (0x00 << 5);

        /// <summary>
        /// Used to specify that an endpoint zero control request is a class specific request.
        /// </summary>
        public static readonly int USB_TYPE_CLASS = (0x01 << 5);

        /// <summary>
        /// Used to specify that an endpoint zero control request is a vendor specific request.
        /// </summary>
        public static readonly int USB_TYPE_VENDOR = (0x02 << 5);

        /// <summary>
        /// Reserved endpoint zero control request type (currently unused).
        /// </summary>
        public static readonly int USB_TYPE_RESERVED = (0x03 << 5);

        /// <summary>
        /// USB class indicating that the class is determined on a per-interface basis.
        /// </summary>
        public static readonly int USB_CLASS_PER_INTERFACE = 0;

        /// <summary>
        /// USB class for audio devices.
        /// </summary>
        public static readonly int USB_CLASS_AUDIO = 1;

        /// <summary>
        /// USB class for communication devices.
        /// </summary>
        public static readonly int USB_CLASS_COMM = 2;

        /// <summary>
        /// USB class for human interface devices (for example, mice and keyboards).
        /// </summary>
        public static readonly int USB_CLASS_HID = 3;

        /// <summary>
        /// USB class for physical devices.
        /// </summary>
        public static readonly int USB_CLASS_PHYSICA = 5;

        /// <summary>
        /// USB class for still image devices (digital cameras).
        /// </summary>
        public static readonly int USB_CLASS_STILL_IMAGE = 6;

        /// <summary>
        /// USB class for printers.
        /// </summary>
        public static readonly int USB_CLASS_PRINTER = 7;

        /// <summary>
        /// USB class for mass storage devices.
        /// </summary>
        public static readonly int USB_CLASS_MASS_STORAGE = 8;

        /// <summary>
        /// USB class for USB hubs.
        /// </summary>
        public static readonly int USB_CLASS_HUB = 9;

        /// <summary>
        /// USB class for CDC devices (communications device class).
        /// </summary>
        public static readonly int USB_CLASS_CDC_DATA = 0x0a;

        /// <summary>
        /// USB class for content smart card devices.
        /// </summary>
        public static readonly int USB_CLASS_CSCID = 0x0b;

        /// <summary>
        /// USB class for content security devices.
        /// </summary>
        public static readonly int USB_CLASS_CONTENT_SEC = 0x0d;

        /// <summary>
        /// USB class for video devices.
        /// </summary>
        public static readonly int USB_CLASS_VIDEO = 0x0e;

        /// <summary>
        /// USB class for wireless controller devices.
        /// </summary>
        public static readonly int USB_CLASS_WIRELESS_CONTROLLER = 0xe0;

        /// <summary>
        /// USB class for wireless miscellaneous devices.
        /// </summary>
        public static readonly int USB_CLASS_MISC = 0xef;

        /// <summary>
        /// Application specific USB class.
        /// </summary>
        public static readonly int USB_CLASS_APP_SPEC = 0xfe;

        /// <summary>
        /// Vendor specific USB class.
        /// </summary>
        public static readonly int USB_CLASS_VENDOR_SPEC = 0xff;

        /// <summary>
        /// Boot subclass for HID devices.
        /// </summary>
        public static readonly int USB_INTERFACE_SUBCLASS_BOOT = 1;

        /// <summary>
        /// Vendor specific USB subclass.
        /// </summary>
        public static readonly int USB_SUBCLASS_VENDOR_SPEC = 0xff;
    }
}
