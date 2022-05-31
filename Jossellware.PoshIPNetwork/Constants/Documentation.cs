namespace Jossellware.PoshIPNetwork.Constants
{
    public static class Documentation
    {
        public static class Commands
        {
            public static class GetIPNetworkAddresses
            {
                public const string All = "Pass this to return all addresses, including broadcast addresses etc., rather than just 'usable' addresses (i.e. addresses that can be assigned to a single NIC on a given network).";
            }
        }

        public static class Shared
        {
            public static class ParameterSets
            {
                public static class ParameterSetIPAddressAndCidr
                {
                    public const string Cidr = "A number between 0 and 32 (for IPv4) or 0 and 64 (for IPv6) that represents the size of the subnet.";

                    public const string IPAddress = "An IP address within the target subnet.";
                }

                public static class ParameterSetIPNetwork
                {
                    public const string Network = @"A CIDR prefix either formatted as a string (e.g. 192.168.123.0/24) or instantiated using a System.Net.IPNetwork constructor.";
                }
            }
        }
    }
}
