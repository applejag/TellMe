using UnityEngine;
using Unity.Netcode;
using System.Threading.Tasks;

public class PlayerList : MonoBehaviour
{
    public GameObject entriesParent;
    private TMPro.TMP_Text[] entries;
    private int numEntries;

    private async void Start() {
        Debug.Log("Start called on PlayerList");

        entries = entriesParent.GetComponentsInChildren<TMPro.TMP_Text>();
        numEntries = entries.Length;

        await PlayerManager.WaitUntilInitialized();
        PlayerManager.Instance.AddOnListChangedListener(OnPlayerListChanged);
    }

    private void OnPlayerListChanged(NetworkListEvent<Player> ev) {
        Debug.Log("Entered OnPlayerListChange");

        var players = PlayerManager.Instance.GetPlayerList();
        Debug.Log("Num players: " + players.Count);
        for (var i = 0; i < players.Count; i++) {
            entries[i].SetText(players[i].playerName.ToString());
        }

        for (var i = players.Count; i < numEntries; i++) {
            entries[i].SetText("");
        }
    }
}
