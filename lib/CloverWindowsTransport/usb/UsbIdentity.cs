using System.Collections.Generic;

namespace com.clover.remotepay.transport.usb
{
    public partial class UsbIdentity
    {
        public static List<UsbIdentity> AllIdentities { get; } = new List<UsbIdentity>
        {
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x3003),
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x3000),
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x3020),
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x3023),
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x2000),
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x4000),
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x4003),
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x4030),
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x4033),
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x3050),
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x3053),
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x4050),
            new UsbIdentity(UsbIdentityType.Merchant, 0x28F3, 0x4053),


            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x3002),
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x3004),
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x3022),
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x3024),
            new UsbIdentity(UsbIdentityType.Customer, 0x18D1, 0x2D01),
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x4002),
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x4004),
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x4032),
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x4034),
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x3052),
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x3054),
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x4052),
            new UsbIdentity(UsbIdentityType.Customer, 0x28F3, 0x4054),
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
