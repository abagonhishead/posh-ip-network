namespace Jossellware.PoshIPNetwork.Cmdlets
{
    using System.Collections.Generic;
    using System.Management.Automation;
    using System.Net;
    using Jossellware.PoshIPNetwork.Cmdlets.Base;
    using Jossellware.PoshIPNetwork.Constants;

    [Cmdlet(CommandData.CommandNames.GetIPNetworkAddresses.Verb, CommandData.CommandNames.GetIPNetworkAddresses.Noun, DefaultParameterSetName = CommandData.ParameterSetNames.Shared.DefaultParameterSetName)]
    [OutputType(typeof(IEnumerable<IPAddress>))]
    public class GetIPNetworkAddressesCommand : IPNetworkCmdletBase
    {
        [Parameter(Mandatory = false,
            Position = 100,
            HelpMessage = Documentation.Shared.ParameterSets.ParameterSetIPAddressAndCidr.IPAddress)]
        public SwitchParameter All { get; set; }

        protected override void ProcessRecordImplementation()
        {
            /* Note that we don't really need to dispose the IPNetworkCollection, as there's nothing to dispose -- see: https://github.com/lduchosal/ipnetwork/blob/master/src/System.Net.IPNetwork/IPAddressCollection.cs#L125 
             *
             * We don't enumerate the collection as (obviously) it could potentially be huge, and we don't want to
             * force the consumer to wait for enumeration to complete before we start sending data down the pipeline.
             */
            
            this.WriteObject(this.Network.ListIPAddress(this.All.IsPresent ? FilterEnum.All : FilterEnum.Usable), false);
        }
    }
}
