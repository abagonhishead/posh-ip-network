namespace Jossellware.PoshIPNetwork.Cmdlets
{
    using System.Management.Automation;
    using Jossellware.PoshIPNetwork.Constants;
    using Jossellware.PoshIPNetwork.Cmdlets.Base;
    using System.Net;

    [Cmdlet(CommandData.CommandNames.ResolveIPAddressBlock.Verb, CommandData.CommandNames.ResolveIPAddressBlock.Noun, DefaultParameterSetName = CommandData.ParameterSetNames.Shared.DefaultParameterSetName)]
    [OutputType(typeof(IPNetwork))]
    public class ResolveIPAddressBlockCommand : IPNetworkCmdletBase
    {
        protected override void ProcessRecordImplementation()
        {
            this.WriteObject(this.Network);
        }
    }
}
