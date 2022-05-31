namespace Jossellware.PoshIPNetwork.Constants
{
    using System.Management.Automation;

    public static class CommandData
    {
        public static class CommandNames
        {
            public static class ResolveIPAddressBlock
            {
                public const string Verb = VerbsDiagnostic.Resolve;

                public const string Noun = "IPAddressBlock";
            }

            public static class GetIPNetworkAddresses
            {
                public const string Verb = VerbsCommon.Get;

                public const string Noun = "IPNetworkAddresses";
            }
        }

        public static class ParameterSetNames
        {
            public static class Shared
            {
                public const string IPNetwork = "IPNetwork";
                public const string IPAddressAndCidr = "IPAddressAndCidr";

                public const string DefaultParameterSetName = IPNetwork;
            }
        }
    }
}
