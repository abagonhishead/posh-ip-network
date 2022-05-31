namespace Jossellware.PoshIPNetwork.Constants
{
    public static class Errors
    {
        public static class CidrOutOfRange
        {
            public const string Message = "CIDR cannot be greater than 32 for IPv4 networks or 64 for IPv6 networks.";

            public const string ErrorId = "CidrOutOfRangeError";
        }

        public static class UnsupportedAddressFamily
        {
            public const string Message = "Only IPv4 and IPV6 networks & addresses are supported.";

            public const string ErrorId = "UnsupportedAddressFamilyError";
        }
    }
}
