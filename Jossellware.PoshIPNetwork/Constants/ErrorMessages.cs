namespace Jossellware.PoshIPNetwork.Constants
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class ErrorMessages
    {
        public const string IPv4AndIPv6Only = "Only IPv4 and IPV6 networks & addresses are supported.";

        public const string IPv4CidrOutOfRange = "An IPv4 network cannot have a CIDR prefix greater than 32.";
    }
}
