namespace Jossellware.PoshIPNetwork.Cmdlets
{
    using System.Management.Automation;
    using System.Net;
    using Jossellware.Shared.PSTools.Cmdlets;
    using Jossellware.PoshIPNetwork.Objects;
    using System.Net.Sockets;
    using System;

    [Cmdlet(Constants.CmdletNames.ResolveIPAddressBlock.Verb, Constants.CmdletNames.ResolveIPAddressBlock.Noun, DefaultParameterSetName = ResolveIPAddressBlock.ParameterSetIPNetwork)]
    [OutputType(typeof(IPAddressBlockData))]
    public class ResolveIPAddressBlock : PSCmdletBase<IPAddressBlockData>
    {
        private const string ParameterSetIPNetwork = "IPNetwork";
        private const string ParameterSetIPAddressAndCidr = "IPAddressAndCidr";

        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ResolveIPAddressBlock.ParameterSetIPNetwork,
            HelpMessage = Constants.Documentation.ResolveIPAddressBlock.Parameters.ParameterSetIPNetwork.Network)]
        public IPNetwork Network { get; set; }

        [Parameter(Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ResolveIPAddressBlock.ParameterSetIPAddressAndCidr,
            HelpMessage = Constants.Documentation.ResolveIPAddressBlock.Parameters.ParameterSetIPAddressAndCidr.IPAddress)]
        [ValidateNotNull]
        public IPAddress IPAddress { get; set; }

        [Parameter(Mandatory = true,
            Position = 1,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ResolveIPAddressBlock.ParameterSetIPAddressAndCidr,
            HelpMessage = Constants.Documentation.ResolveIPAddressBlock.Parameters.ParameterSetIPAddressAndCidr.Cidr)]
        [ValidateRange(0, 64)]
        public byte Cidr { get; set; }

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
