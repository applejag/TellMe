using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class PlayerListUIScript : MonoBehaviour
{
    public Transform playerTextsParent;
    public GameObject playerTextPrefab;
    public GameStateScript gameState;
    private readonly List<PlayerUIScript> playerTexts = new();

    private void Start()
    {
        if (!gameState)
        {
            Debug.LogWarning("Lacking game state script", this);
            enabled = false;
            return;
        }

        gameState.playerList.OnListChanged += ClientOnPlayerListChanged;

        foreach (Transform child in playerTextsParent)
        {
            playerTexts.Add(child.GetComponentInChildren<PlayerUIScript>());
        }
        UpdatePlayerTextList();
    }

    private void OnDestroy()
    {
        if (gameState)
        {
            gameState.playerList.OnListChanged -= ClientOnPlayerListChanged;
        }
    }

    private void ClientOnPlayerListChanged(NetworkListEvent<NetworkBehaviourReference> changeEvent)
    {
        UpdatePlayerTextList();
    }

    private void UpdatePlayerTextList()
    {
        if (!gameState)
        {
            return;
        }

        var players = gameState.PlayerScripts.ToArray();
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
