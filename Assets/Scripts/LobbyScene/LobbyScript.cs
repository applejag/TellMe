using UnityEngine;
using Unity.Netcode;

public class LobbyScript : MonoBehaviour
{
    public PlayerManager playerManager;

    private void Start()
    {
        if (playerManager == null)
        {
            playerManager = FindObjectOfType<PlayerManager>();
            if (playerManager == null)
            {
                Debug.LogWarning("Lacking player manager", this);
                enabled = false;
                return;
            }
        }

        if (NetworkManager.Singleton == null)
        {
            Debug.LogWarning("Lacking network manager", this);
            enabled = false;
            return;
        }

        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            OnClientConnect(NetworkManager.Singleton.ServerClientId);
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton && NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
        }
    }

    private void OnClientConnect(ulong clientId)
    {
        Debug.Log("Player connected: " + (clientId + 1));
        Player p = new Player
        {
            clientId = clientId,
            playerName = "Player " + (clientId + 1)
        };
        playerManager.AddPlayer(p);
    }

    private void OnClientDisconnect(ulong clientId)
    {
        Debug.Log("Player disconnected: " + (clientId + 1));
        playerManager.RemovePlayer(clientId);
    }
}

