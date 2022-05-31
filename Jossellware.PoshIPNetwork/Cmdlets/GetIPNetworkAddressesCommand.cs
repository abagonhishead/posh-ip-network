namespace Jossellware.PoshIPNetwork.Cmdlets
{
    using System.Collections.Generic;
    using System.Linq;
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
             * It's probably best that we do anyway though, just in case for some reason something changes */
            using (var addresses = this.Network.ListIPAddress(this.All.IsPresent ? FilterEnum.All : FilterEnum.Usable))
            {
                // Iterate so that we're writing the objects one-by-one, otherwise the consumer is forced to wait for the entire collection to be enumerated before we start writing some output.
                foreach (var address in addresses)
                {
                    this.WriteObject(address);
                }
            }
        }
    }
}
