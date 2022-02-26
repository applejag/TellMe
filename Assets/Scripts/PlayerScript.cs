using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerScript : NetworkBehaviour
{
    public NetworkVariable<FixedString32Bytes> joinCode = new();
    public NetworkVariable<FixedString128Bytes> playerName = new();
    public NetworkVariable<bool> playerIsReady = new();

    private void Awake()
    {
        playerName.Value = "<unnamed>";
    }

    [ServerRpc]
    public void SetPlayerNameServerRpc(string name)
    {
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

    [ServerRpc]
    public void SetPlayerIsReadyServerRpc(bool isReady)
    {
        var gameState = FindObjectOfType<GameStateScript>();
        if (!gameState)
        {
            Debug.LogWarning("Lacking game state script.", this);
            return;
        }

        if (gameState.isGameStarted.Value)
        {
            return;
        }

        playerIsReady.Value = isReady;
    }
}
