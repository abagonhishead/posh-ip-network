namespace Jossellware.PoshIPNetwork.Commands
{
    using System.Management.Automation;
    using Jossellware.PoshIPNetwork.Constants;
    using Jossellware.PoshIPNetwork.Commands.Base;
    using System.Net;

    [Cmdlet(CommandData.CommandNames.ResolveIPAddressBlock.Verb, CommandData.CommandNames.ResolveIPAddressBlock.Noun, DefaultParameterSetName = CommandData.ParameterSetNames.Shared.DefaultParameterSetName)]
    [OutputType(typeof(IPNetwork))]
    public class ResolveIPAddressBlockCommand : IPNetworkCommandBase
    {
        protected override void ProcessRecordImplementation()
        {
            this.WriteObject(this.Network);
        }
    }
}
