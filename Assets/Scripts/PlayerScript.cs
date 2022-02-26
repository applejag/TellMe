using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerScript : NetworkBehaviour
{
    public NetworkVariable<FixedString128Bytes> playerName = new NetworkVariable<FixedString128Bytes>();

    private void Awake()
    {
        playerName.Value = "<unnamed>";
    }

    [ServerRpc]
    public void SetPlayerNameServerRpc(string name)
    {
        Debug.Log($"SetPlayerNameServerRpc called with: {name}");
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        name = name.Trim();
        if (name.Length > 128)
        {
            name = name[..128];
        }
        playerName.Value = name;
    }
}
