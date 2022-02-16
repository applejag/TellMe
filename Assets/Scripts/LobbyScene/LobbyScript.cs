using UnityEngine;
using Unity.Netcode;

public class LobbyScript : MonoBehaviour
{
    public GameObject playerManagerPrefab;

    private void Start() {
        Debug.Log("Start called on LobbyScript");
        if (NetworkManager.Singleton.IsServer) {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            OnClientConnect(NetworkManager.Singleton.ServerClientId);
            Instantiate(playerManagerPrefab, Vector3.zero, Quaternion.identity);
        }
    }

    private async void OnClientConnect(ulong clientId) {
        Debug.Log("Player connected: " + (clientId + 1));
        Player p = new Player();
        p.clientId = clientId;
        p.playerName = "Player " + (clientId + 1);
        await PlayerManager.WaitUntilInitialized();
        PlayerManager.Instance.AddPlayer(p);
    }

    private async void OnClientDisconnect(ulong clientId) {
        Debug.Log("Player disconnected: " + (clientId + 1));
        await PlayerManager.WaitUntilInitialized();
        PlayerManager.Instance.RemovePlayer(clientId);
    }
}
