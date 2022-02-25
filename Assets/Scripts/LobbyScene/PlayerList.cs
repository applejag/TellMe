using UnityEngine;
using Unity.Netcode;
using TMPro;
using System.Collections.Generic;

public class PlayerList : MonoBehaviour
{
    public PlayerManager playerManager;

    public Transform playerTextsParent;
    public GameObject playerTextPrefab;
    public GameObject noPlayersText;
    private List<TMP_Text> playerTexts = new List<TMP_Text>();

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

        playerManager.PlayerListChanged += OnPlayerListChanged;
        foreach (Transform child in playerTextsParent)
        {
            playerTexts.Add(child.GetComponentInChildren<TMP_Text>());
        }
        UpdatePlayerTextList();
    }

    private void OnDestroy()
    {
        if (playerManager != null)
        {
            playerManager.PlayerListChanged -= OnPlayerListChanged;
        }
    }

    private void OnPlayerListChanged(NetworkListEvent<Player> _)
    {
        Debug.Log("Entered OnPlayerListChange");
        UpdatePlayerTextList();
    }

    private void UpdatePlayerTextList()
    {
        var players = playerManager.GetPlayerList();
        Debug.Log("Num players: " + players.Count);

        while (playerTexts.Count < players.Count)
        {
            var clone = Instantiate(playerTextPrefab, playerTextsParent);
            var text = clone.GetComponentInChildren<TMP_Text>();
            playerTexts.Add(text);
        }

        while (playerTexts.Count > players.Count)
        {
            var lastIdx = playerTexts.Count - 1;
            Destroy(playerTexts[lastIdx].gameObject);
            playerTexts.RemoveAt(lastIdx);
        }

        noPlayersText.SetActive(players.Count == 0);

        for (var i = 0; i < players.Count; i++)
        {
            playerTexts[i].SetText(players[i].playerName.ToString());
        }
    }
}
