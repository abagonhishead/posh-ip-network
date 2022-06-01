# PoshIPNetwork
## What is it?

PoshIPNetwork is a small, simple PowerShell module written in C# that provides a means to parse and process IPv4/IPv6 CIDR prefixes using the excellent `System.Net.IPNetwork` (https://github.com/lduchosal/ipnetwork).

It can be used to: 
- List all available IP addresses in a given prefix/subnet
- Get the broadcast address for a subnet
- Validate a CIDR prefix

... among other things.

## Compatibility
PoshIPNetwork is written in .NET Standard 2.0, thanks to IPNetwork2's support of .NET Standard. This means it should be compatible with PowerShell 5 & PowerShell Core across any platforms that PowerShell Core supports.

## Using it
Builds & packages will be available shortly. In the interim, you can build and use it yourself if you have the .NET SDK by cloning the repo, then, in PowerShell:

````powershell
cd "posh-ip-network"

dotnet publish

Import-Module ".\Jossellware.PoshIPNetwork\bin\Debug\netstandard2.0\publish\PoshIPNetwork.dll"
````

Use `dotnet publish` rather than `dotnet build` so that dependent assemblies are included with the build.

To view available commands:
````powershell
Get-Command -Module PoshIPNetwork
````

## Commands

The below commands have common parameters as below, and accept them as pipeline input:
  - Parameter set 1 (default): 
    - `-Network` - A string representation of a CIDR prefix using RFC 4632 notation (e.g. 192.168.0.0/24) - this is then parsed using `IPNetwork.Parse`. 
      - This can also be a `System.Net.IPNetwork` instance piped from another command or from `[System.Net.IPNetwork]::Parse()`/`[System.Net.IPNetwork]::new()`
  - Parameter set 2:
    - `-IPAddress` - an IP address under the target prefix
    - `-Cidr` - a number representing the netmask as per the RFC 4632 CIDR notation. This is a value between 0 and 32 for an IPv4 address & 0 and 128 for an IPv6 address.

### Currently working:
- `Resolve-IPAddressBlock` - create an instance of `System.Net.IPNetwork` based on a string CIDR prefix or an IP address followed by a byte value between 0 & 32 (IPv4) or 0 & 128 (IPv6)


````powershell
Resolve-IPAddressBlock -Network 192.168.0.1/24

Value         : 192.168.0.0/24
Network       : 192.168.0.0
AddressFamily : InterNetwork
Netmask       : 255.255.255.0
Broadcast     : 192.168.0.255
FirstUsable   : 192.168.0.1
LastUsable    : 192.168.0.254
Usable        : 254
Total         : 256
Cidr          : 24
````
````powershell
Resolve-IPAddressBlock -IPAddress 123.123.123.123 -Cidr 12

Value         : 123.112.0.0/12
Network       : 123.112.0.0
AddressFamily : InterNetwork
Netmask       : 255.240.0.0
Broadcast     : 123.127.255.255
FirstUsable   : 123.112.0.1
LastUsable    : 123.127.255.254
Usable        : 1048574
Total         : 1048576
Cidr          : 12
````

````powershell
Resolve-IPAddressBlock -Network 2001:db8::/44

Value         : 2001:db8::/44
Network       : 2001:db8::
AddressFamily : InterNetworkV6
Netmask       : ffff:ffff:fff0::
Broadcast     :
FirstUsable   : 2001:db8::
LastUsable    : 2001:db8:f:ffff:ffff:ffff:ffff:ffff
Usable        : 19342813113834066795298816
Total         : 19342813113834066795298816
Cidr          : 44
````

- `Get-IPNetworkAddresses` - returns a collection of all available IP addresses in a given CIDR prefix. 
  - `-All` - return all addresses (including broadcast etc.) rather than only those addresses that are 'usable' (that is, they are assignable to a single NIC on a given network.)

````powershell
Get-IPNetworkAddresses -Network 10.0.0.0/24

AddressFamily      : InterNetwork
ScopeId            :
IsIPv6Multicast    : False
IsIPv6LinkLocal    : False
IsIPv6SiteLocal    : False
IsIPv6Teredo       : False
IsIPv6UniqueLocal  : False
IsIPv4MappedToIPv6 : False
Address            : 16777226
IPAddressToString  : 10.0.0.1

[ -- snip -- ]

AddressFamily      : InterNetwork
ScopeId            :
IsIPv6Multicast    : False
IsIPv6LinkLocal    : False
IsIPv6SiteLocal    : False
IsIPv6Teredo       : False
IsIPv6UniqueLocal  : False
IsIPv4MappedToIPv6 : False
Address            : 4261412874
IPAddressToString  : 10.0.0.254
````

````powershell
Get-IPNetworkAddresses -Network 2001:db8::/126

AddressFamily      : InterNetworkV6
ScopeId            : 0
IsIPv6Multicast    : False
IsIPv6LinkLocal    : False
IsIPv6SiteLocal    : False
IsIPv6Teredo       : False
IsIPv6UniqueLocal  : False
IsIPv4MappedToIPv6 : False
Address            :
IPAddressToString  : 2001:db8::1

AddressFamily      : InterNetworkV6
ScopeId            : 0
IsIPv6Multicast    : False
IsIPv6LinkLocal    : False
IsIPv6SiteLocal    : False
IsIPv6Teredo       : False
IsIPv6UniqueLocal  : False
IsIPv4MappedToIPv6 : False
Address            :
IPAddressToString  : 2001:db8::2
````

With pipeline input:
````powershell
Resolve-IPAddressBlock -IPAddress 123.123.123.123 -Cidr 24 | Get-IPNetworkAddresses

AddressFamily      : InterNetwork
ScopeId            :
IsIPv6Multicast    : False
IsIPv6LinkLocal    : False
IsIPv6SiteLocal    : False
IsIPv6Teredo       : False
IsIPv6UniqueLocal  : False
IsIPv4MappedToIPv6 : False
Address            : 24869755
IPAddressToString  : 123.123.123.1

[ -- snip -- ]

AddressFamily      : InterNetwork
ScopeId            :
IsIPv6Multicast    : False
IsIPv6LinkLocal    : False
IsIPv6SiteLocal    : False
IsIPv6Teredo       : False
IsIPv6UniqueLocal  : False
IsIPv4MappedToIPv6 : False
Address            : 4269505403
IPAddressToString  : 123.123.123.254
````

Include broadcast, unusable, etc.:
````powershell
Resolve-IPAddressBlock -IPAddress 192.168.0.0 -Cidr 24 | Get-IPNetworkAddresses -All

AddressFamily      : InterNetwork
ScopeId            :
IsIPv6Multicast    : False
IsIPv6LinkLocal    : False
IsIPv6SiteLocal    : False
IsIPv6Teredo       : False
IsIPv6UniqueLocal  : False
IsIPv4MappedToIPv6 : False
Address            : 43200
IPAddressToString  : 192.168.0.0

[ -- snip --]

AddressFamily      : InterNetwork
ScopeId            :
IsIPv6Multicast    : False
IsIPv6LinkLocal    : False
IsIPv6SiteLocal    : False
IsIPv6Teredo       : False
IsIPv6UniqueLocal  : False
IsIPv4MappedToIPv6 : False
Address            : 4278233280
IPAddressToString  : 192.168.0.255
````

## Todo
- Remaining unit tests
- Add netmask to existing commands
- Additional commands:
  - `Get-IPNetworkSupernets` - list all supernets for a given IP address or prefix
  - `Get-IPNetworkSubnets` - list all subnets for a given prefix
- Performance tests for large subnets
- Documentation
