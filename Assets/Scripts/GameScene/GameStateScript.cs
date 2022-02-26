using System;
using System.Collections.Generic;
using Unity.Netcode;

public class GameStateScript : NetworkBehaviour
{
    public NetworkVariable<bool> isGameStarted = new();
    private readonly Dictionary<ulong, PlayerScript> playerScripts = new();
    /// <summary>Number of players, excluding the host.</summary>
    public NetworkVariable<int> playerCount = new();
    /// <summary>Number of players that are ready, excluding the host.</summary>
    public NetworkVariable<int> playersReadyCount = new();

    private void Start()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += ServerOnClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback += ServerOnClientDisconnect;
            var playerObj = NetworkManager.Singleton.LocalClient.PlayerObject;
            playerScripts[playerObj.OwnerClientId] = playerObj.GetComponent<PlayerScript>();
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        if (NetworkManager.Singleton && IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= ServerOnClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback -= ServerOnClientDisconnect;
        }
    }

    private void ServerOnClientDisconnect(ulong id)
    {
        if (playerScripts.TryGetValue(id, out var playerScript) && playerScripts.Remove(id))
        {
            playerCount.Value--;
            if (playerScript.playerIsReady.Value)
            {
                playersReadyCount.Value--;
            }
        }
    }

    private void ServerOnClientConnect(ulong id)
    {
        playerCount.Value++;
        var playerScript = NetworkManager.Singleton.ConnectedClients[id].PlayerObject.GetComponent<PlayerScript>();
        playerScripts[id] = playerScript;
        playerScript.playerIsReady.OnValueChanged += OnPlayerReadyChanged;
    }

    private void OnPlayerReadyChanged(bool previousValue, bool newValue)
    {
        if (!previousValue && newValue)
        {
            playersReadyCount.Value++;
        }
        else if (previousValue && !newValue)
        {
            playersReadyCount.Value--;
        }
    }

    public void StartGame()
    {
        if (isGameStarted.Value)
        {
            return;
        }

        isGameStarted.Value = true;
    }
}
