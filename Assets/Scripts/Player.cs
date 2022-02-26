using System;
using Unity.Collections;
using Unity.Netcode;

public struct Player : IEquatable<Player>, INetworkSerializable
{
    public ulong clientId;
    public FixedString32Bytes playerName;

    public bool IsServer => NetworkManager.Singleton != null && NetworkManager.Singleton.ServerClientId == clientId;
    public bool IsLocal => NetworkManager.Singleton != null && NetworkManager.Singleton.LocalClientId == clientId;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref playerName);
    }

    public override bool Equals(object obj)
    {
        return obj is Player player && Equals(player);
    }

    public bool Equals(Player player)
    {
        return clientId == player.clientId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(clientId);
    }
}
