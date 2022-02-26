using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class PlayerListUIScript : MonoBehaviour
{
    public Transform playerTextsParent;
    public GameObject playerTextPrefab;
    private readonly List<PlayerUIScript> playerTexts = new List<PlayerUIScript>();

    private void Start()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogWarning("Lacking network manager", this);
            enabled = false;
            return;
        }

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;

        foreach (Transform child in playerTextsParent)
        {
            playerTexts.Add(child.GetComponentInChildren<PlayerUIScript>());
        }
        UpdatePlayerTextList();
    }

    private void OnClientDisconnect(ulong obj)
    {
        UpdatePlayerTextList();
    }

    private void OnClientConnect(ulong obj)
    {
        UpdatePlayerTextList();
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
        }
    }

    private void UpdatePlayerTextList()
    {
        if (!NetworkManager.Singleton)
        {
            return;
        }

        var players = NetworkManager.Singleton.SpawnManager.SpawnedObjectsList
            .Where(o => o.IsPlayerObject)
            .Select(o => o.GetComponent<PlayerScript>())
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
