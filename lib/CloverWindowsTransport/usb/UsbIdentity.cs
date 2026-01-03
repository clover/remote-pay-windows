using System.Collections.Generic;

namespace com.clover.remotepay.transport.usb
{
    public partial class UsbIdentity
    {
        public static List<UsbIdentity> AllIdentities { get; } = new List<UsbIdentity>
        {
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x3003), // maplecutter cloverusb device
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x3000), // maplecutter adb device
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x3020), // knotty pine adb device
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x3023), // knottypine cloverusb device
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x2000), // leafcutter adb device
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x4000), // bayleaf adb device
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x4003), // bayleaf cloverusb device
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x4030),
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x4033),
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x3050), // maplethree adb device
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x3053), // maplethree cloverusb device
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x4050), // ficustree adb device
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x4053), // ficustree cloverusb device
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x8000), // pinetree-semi adb device
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x8003), // pinetree-semi cloverusb device
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x8010), // pinetree adb device
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x8013), // pinetree cloverusb device


            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x3002), // maplecutter accessory usb deviceF
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x3004), // maplecutter adb,accessory usb device
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x3022), // knottypine cloverusb device
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x3024), // knottypine adb,accessory usb device
            new UsbIdentity(UsbIdentityType.Customer, 0x18D1, 0x2D01), // google adb,accessory usb device
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x4002), // bayleaf accessory usb device
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x4004), // bayleaf adb,accessory usb device
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x4032), 
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x4034),
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x3052), // maplethree accessory usb device
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x3054), // maplethree adb,accessory usb device
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x4052), // ficustree accessory usb device
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x4054), // ficustree adb,accessory usb deviceF
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x8002), // pinetree-semi clover usb device
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x8004), // pinetree-semi adb,accessory usb device
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x8012), // pinetree clover usb device
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x8014), // pinetree adb,accessory usb device

        };
    }

    public partial class UsbIdentity
    {
        public UsbIdentityType Type { get; set; }

        public int Vid { get; set; }

        public int Pid { get; set; }

        public UsbIdentity(UsbIdentityType type, int vid, int pid)
        {
            Type = type;
            Vid = vid;
            Pid = pid;
        }
    }
}
