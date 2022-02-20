using UnityEngine;
using Unity.Netcode;
using TMPro;
using System.Collections.Generic;

public class PlayerList : MonoBehaviour
{
    public Transform playerTextsParent;
    public GameObject playerTextPrefab;
    private List<TMP_Text> playerTexts = new List<TMP_Text>();

    private async void Start() {
        Debug.Log("Start called on PlayerList");

        await PlayerManager.WaitUntilInitialized();
        PlayerManager.Instance.AddOnListChangedListener(OnPlayerListChanged);
    }

    private void OnPlayerListChanged(NetworkListEvent<Player> ev) {
        Debug.Log("Entered OnPlayerListChange");

        var players = PlayerManager.Instance.GetPlayerList();
        Debug.Log("Num players: " + players.Count);

        while (playerTexts.Count < players.Count)
        {
            var clone = Instantiate(playerTextPrefab, playerTextsParent);
            var text = clone.GetComponentInChildren<TMP_Text>();
            playerTexts.Add(text);
        }

        for (var i = 0; i < players.Count; i++) {
            playerTexts[i].SetText(players[i].playerName.ToString());
        }

        for (var i = playerTexts.Count - 1; i >= players.Count; i--) {
            Destroy(playerTexts[i].gameObject);
            playerTexts.RemoveAt(i);
        }
    }
}
