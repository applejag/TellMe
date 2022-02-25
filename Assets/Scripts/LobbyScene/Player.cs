using Unity.Collections;
using Unity.Netcode;

public struct Player : System.IEquatable<Player>, INetworkSerializable
{
    public ulong clientId;
    public FixedString32Bytes playerName;

    public bool Equals(Player p) {
        return p.clientId == clientId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref playerName);
    }
}
