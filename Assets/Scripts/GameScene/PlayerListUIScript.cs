using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class PlayerListUIScript : NetworkBehaviour
{
    public Transform playerTextsParent;
    public GameObject playerTextPrefab;
    [SerializeField]
    private NetworkList<NetworkObjectReference> playerList;
    private readonly List<PlayerUIScript> playerTexts = new();

    private void Awake()
    {
        // Cant initialize NetworkList in ctor :(
        playerList = new NetworkList<NetworkObjectReference>();
    }

    private void Start()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogWarning("Lacking network manager", this);
            enabled = false;
            return;
        }

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += ServerOnClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback += ServerOnClientDisconnect;
            playerList.Add(NetworkManager.Singleton.LocalClient.PlayerObject);
        }
        else
        {
            playerList.OnListChanged += ClientOnPlayerListListChanged;
        }

        foreach (Transform child in playerTextsParent)
        {
            playerTexts.Add(child.GetComponentInChildren<PlayerUIScript>());
        }
        UpdatePlayerTextList();
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

    private void ClientOnPlayerListListChanged(NetworkListEvent<NetworkObjectReference> changeEvent)
    {
        UpdatePlayerTextList();
    }

    private void ServerOnClientConnect(ulong id)
    {
        playerList.Add(NetworkManager.Singleton.ConnectedClients[id].PlayerObject);
        UpdatePlayerTextList();
    }

    private void ServerOnClientDisconnect(ulong id)
    {
        var idx = playerList.AsEnumerable()
            .Select(p => p.TryGet(out var netObj) ? netObj : null)
            .IndexOf(p => p && p.OwnerClientId == id);
        if (idx == -1)
        {
            Debug.LogWarning($"Disconnect of unknown client: {id}");
        }
        else
        {
            playerList.RemoveAt(idx);
            UpdatePlayerTextList();
        }
    }

    private void UpdatePlayerTextList()
    {
        if (!NetworkManager.Singleton)
        {
            return;
        }

        var players = playerList.AsEnumerable()
            .Select(o => o.TryGet(out var netObj) ? netObj.GetComponent<PlayerScript>() : null)
            .Where(o => o)
            .ToArray();
        Debug.Log("Num players: " + players.Length);

        while (playerTexts.Count < players.Length)
        {
            var clone = Instantiate(playerTextPrefab, playerTextsParent);
            var text = clone.GetComponentInChildren<PlayerUIScript>();
            playerTexts.Add(text);
        }

        while (playerTexts.Count > players.Length)
        {
            var lastIdx = playerTexts.Count - 1;
            Destroy(playerTexts[lastIdx].gameObject);
            playerTexts.RemoveAt(lastIdx);
        }

        for (var i = 0; i < players.Length; i++)
        {
            playerTexts[i].SetPlayer(players[i]);
        }
    }
}
