namespace Jossellware.PoshIPNetwork.Objects
{
    using System;
    using System.Management.Automation;
    using System.Net;
    using System.Numerics;
    using Jossellware.PoshIPNetwork.Objects.Enums;

    public class AddressBlock
    {
        public IPAddress Network { get; }

        public IPAddress FirstUsableAddress { get; }

        public IPAddress LastUsableAddress { get; }

        public IPAddress Netmask { get; }

        public IPAddress Broadcast { get; }

        public byte Cidr { get; }

        public BigInteger UsableAddressCount { get; }

        public BigInteger TotalAddressCount { get; }

        public NetworkType NetworkType { get; }

        public AddressBlock(IPNetwork ipNetwork)
        {
            if (!Enum.IsDefined(typeof(NetworkType), (int)ipNetwork.AddressFamily))
            {
                throw new PSNotSupportedException(Constants.ErrorMessages.IPv4AndIPv6Only);
            }

            this.NetworkType = (NetworkType)ipNetwork.AddressFamily;
            this.Network = ipNetwork.Network;
            this.FirstUsableAddress = ipNetwork.FirstUsable;
            this.LastUsableAddress = ipNetwork.LastUsable;
            this.Netmask = ipNetwork.Netmask;
            this.Broadcast = ipNetwork.Broadcast;
            this.UsableAddressCount = ipNetwork.Usable;
            this.TotalAddressCount = ipNetwork.Total;
            this.Cidr = ipNetwork.Cidr;
        }
    }
}
