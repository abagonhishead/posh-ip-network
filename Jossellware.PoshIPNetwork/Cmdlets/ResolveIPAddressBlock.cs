namespace Jossellware.PoshIPNetwork.Cmdlets
{
    using System.Management.Automation;
    using System.Net;
    using Jossellware.Shared.PSTools.Cmdlets;
    using Jossellware.PoshIPNetwork.Objects;
    using System.Net.Sockets;
    using System;

    [Cmdlet(Constants.CmdletNames.ResolveIPAddressBlock.Verb, Constants.CmdletNames.ResolveIPAddressBlock.Noun, DefaultParameterSetName = ResolveIPAddressBlock.ParameterSetCidrPrefix)]
    [OutputType(typeof(IPAddressBlockData))]
    public class ResolveIPAddressBlock : PSCmdletBase<IPAddressBlockData>
    {
        private const string ParameterSetCidrPrefix = "CidrPrefix";
        private const string ParameterSetIPAddressAndCidr = "IPAddressAndCidr";

        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ResolveIPAddressBlock.ParameterSetCidrPrefix,
            HelpMessage = Constants.Documentation.ResolveIPAddressBlock.Parameters.CidrPrefix)]
        public IPNetwork Network { get; set; }

        [Parameter(Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ResolveIPAddressBlock.ParameterSetIPAddressAndCidr,
            HelpMessage = "An IP address within the target subnet.")]
        [ValidateNotNull]
        public IPAddress IPAddress { get; set; }

        [Parameter(Mandatory = true,
            Position = 1,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ResolveIPAddressBlock.ParameterSetIPAddressAndCidr,
            HelpMessage = "A number between 0 and 32 (for IPv4) or 0 and 64 (for IPv6) that represents the size of the subnet.")]
        [ValidateRange(0, 64)]
        public byte Cidr { get; set; }

        public ResolveIPAddressBlock()
        {
        }

        protected override void BeginProcessingImplementation()
        {
            if (this.IsParameterSetNamed(ResolveIPAddressBlock.ParameterSetIPAddressAndCidr))
            {
                if (this.IPAddress.AddressFamily != AddressFamily.InterNetwork && this.IPAddress.AddressFamily != AddressFamily.InterNetworkV6)
                {
                    this.SetError(new PSNotSupportedException(Constants.ErrorMessages.IPv4AndIPv6Only), "01", ErrorCategory.InvalidArgument, this.IPAddress);
                }
                else if (this.IPAddress.AddressFamily == AddressFamily.InterNetwork && this.Cidr > 32)
                {
                    this.SetError(new PSArgumentOutOfRangeException(nameof(this.Cidr), this.Cidr, Constants.ErrorMessages.IPv4CidrOutOfRange), "02", ErrorCategory.InvalidArgument, this.IPAddress);
                }
            }
        }

        protected override IPAddressBlockData ProcessRecordImplementation()
        {
            if (this.IsParameterSetNamed(ResolveIPAddressBlock.ParameterSetIPAddressAndCidr))
            {
                this.Network = new IPNetwork(this.IPAddress, this.Cidr);
            }

            return new IPAddressBlockData(this.Network);
        }
    }
}
