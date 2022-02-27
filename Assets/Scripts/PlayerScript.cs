using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerScript : NetworkBehaviour
{
    public NetworkVariable<FixedString32Bytes> joinCode = new();
    public NetworkVariable<FixedString128Bytes> playerName = new();
    public NetworkVariable<bool> playerIsReady = new();
    public float tempFocusOnInitialNameChange = 5f;

    [ServerRpc]
    public void SetPlayerNameServerRpc(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        if (playerName.Value.Length == 0)
        {
            // It's the initial name change
            try
            {
                CameraManagerScript.Instance.ActivateCameraTemporarilyServer(CameraScript.Kind.PlayerCloseUp, OwnerClientId, tempFocusOnInitialNameChange);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, this);
            }
        }

        playerName.Value = FixedStringUtil.CreateTruncated128(name.Trim());
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
