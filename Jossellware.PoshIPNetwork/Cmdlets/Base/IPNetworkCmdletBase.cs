namespace Jossellware.PoshIPNetwork.Cmdlets.Base
{
    using System.Management.Automation;
    using System.Net;
    using System.Net.Sockets;
    using Jossellware.PoshIPNetwork.Constants;
    using Jossellware.Shared.PSTools.Cmdlets;

    public abstract class IPNetworkCmdletBase : PSCmdletBase
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = CommandData.ParameterSetNames.Shared.IPNetwork,
            HelpMessage = Documentation.Shared.ParameterSets.ParameterSetIPNetwork.Network)]
        public IPNetwork Network { get; set; }

        [Parameter(Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = CommandData.ParameterSetNames.Shared.IPAddressAndCidr,
            HelpMessage = Documentation.Shared.ParameterSets.ParameterSetIPAddressAndCidr.IPAddress)]
        [ValidateNotNull]
        public IPAddress IPAddress { get; set; }

        [Parameter(Mandatory = true,
            Position = 1,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = CommandData.ParameterSetNames.Shared.IPAddressAndCidr,
            HelpMessage = Documentation.Shared.ParameterSets.ParameterSetIPAddressAndCidr.Cidr)]
        [ValidateRange(0, 64)]
        public byte Cidr { get; set; }

        protected override void BeginProcessingImplementation()
        {
            base.BeginProcessingImplementation();

            var error = default(ErrorRecord);
            if (this.IsParameterSetNamed(CommandData.ParameterSetNames.Shared.IPAddressAndCidr))
            {
                if (this.IPAddress.AddressFamily != AddressFamily.InterNetwork && this.IPAddress.AddressFamily != AddressFamily.InterNetworkV6)
                {
                    var exception = new PSNotSupportedException(Errors.UnsupportedAddressFamily.Message);
                    error = this.ErrorFactory.BuildError(exception, Errors.UnsupportedAddressFamily.ErrorId, ErrorCategory.InvalidArgument, this.IPAddress);
                }
                else if (this.IPAddress.AddressFamily == AddressFamily.InterNetwork && this.Cidr > 32)
                {
                    var exception = new PSArgumentOutOfRangeException(nameof(this.Cidr), this.Cidr, Errors.CidrOutOfRange.Message);
                    error = this.ErrorFactory.BuildError(exception, Errors.CidrOutOfRange.ErrorId, ErrorCategory.InvalidArgument, this.Cidr);
                }
                else
                {
                    this.Network = new IPNetwork(this.IPAddress, this.Cidr);
                }
            }

            if (error != null)
            {
                this.SetError(error);
            }
        }
    }
}
