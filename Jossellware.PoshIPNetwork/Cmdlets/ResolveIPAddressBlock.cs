namespace Jossellware.PoshIPNetwork.Cmdlets
{
    using System.Management.Automation;
    using System.Net;
    using Jossellware.Shared.PSTools.Cmdlets;
    using Jossellware.PoshIPNetwork.Objects;
    using System.Net.Sockets;

    [Cmdlet(VerbsDiagnostic.Resolve, "AddressBlock", DefaultParameterSetName = ResolveIPAddressBlock.ParameterSetCidrPrefix)]
    [OutputType(typeof(IPNetwork))]
    public class ResolveIPAddressBlock : PSCmdletBase
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
            HelpMessage = "A number between 0 and 64 representing the size of the subnet.")]
        [ValidateRange(0, 64)]
        public byte Cidr { get; set; }

        public ResolveIPAddressBlock()
        {
        }

        public override string GetResourceString(string baseName, string resourceId)
        {
            return base.GetResourceString(baseName, resourceId);
        }

        protected override void BeginProcessing()
        {
            if (string.Equals(this.ParameterSetName, ResolveIPAddressBlock.ParameterSetIPAddressAndCidr))
            {
                if (this.IPAddress.AddressFamily != AddressFamily.InterNetwork && this.IPAddress.AddressFamily != AddressFamily.InterNetworkV6)
                {
                    this.BuildAndWriteError(new PSNotSupportedException(Constants.ErrorMessages.IPv4AndIPv6Only), "01", ErrorCategory.InvalidArgument, this.IPAddress);
                }
                else if (this.IPAddress.AddressFamily == AddressFamily.InterNetwork && this.Cidr > 32)
                {
                    this.BuildAndWriteError(new PSArgumentOutOfRangeException(Constants.ErrorMessages.IPv4CidrOutOfRange), "02", ErrorCategory.InvalidArgument);
                }

                this.Network = new IPNetwork(this.IPAddress, this.Cidr);
            }
        }

        protected override void ProcessRecord()
        {
            this.WriteObject(new AddressBlock(this.Network));

            base.ProcessRecord();
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

        protected override void StopProcessing()
        {
            base.StopProcessing();
        }
    }
}
