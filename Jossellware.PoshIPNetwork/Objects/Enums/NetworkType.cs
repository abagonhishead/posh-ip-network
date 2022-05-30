namespace Jossellware.PoshIPNetwork.Objects.Enums
{
    /// <summary>
    /// The Internet Protocol type that the address/prefix belongs to, one of <see cref="NetworkType.InternetProtocolVersion4"/> or <see cref="InternetProtocolVersion6"/>
    /// </summary>
    /// <remarks>
    /// The valid integer values map directly to <see cref="System.Net.Sockets.AddressFamily"/>
    /// </remarks>
    public enum NetworkType
    {
        Unknown = 0,
        InternetProtocolVersion4 = 2,
        InternetProtocolVersion6 = 23,
    }
}
