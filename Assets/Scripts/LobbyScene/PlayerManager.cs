using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager Instance;

    private NetworkList<Player> players;
    private NetworkVariable<bool> isInitialized = new NetworkVariable<bool>(NetworkVariableReadPermission.Everyone, false);

    private void Start() {
        if (Instance == null) {
            Instance = this;
        }
        if (IsServer) {
            Debug.Log("Start called on PlayerManager");
            Init();
            NetworkObject.Spawn(destroyWithScene: true);
        }
    }

    public override async void OnNetworkSpawn() {
        Debug.Log("OnNetworkSpawn called on PlayerManager");
        await WaitUntilSpawned();
        await WaitUntilInitialized();
        Debug.Log("OnNetworkSpawn ended on PlayerManager");
    }

    private async Task WaitUntilSpawned() {
        while (!IsSpawned) {
            Debug.Log("Waiting until PlayerManager is spawned");
            await Task.Delay(200);
        }
    }

    private void Init() {
        Debug.Log("Init called on PlayerManager");

        if (IsInitialized()) {
            Debug.Log("Already initialized!");
            return;
        }

        if (!IsServer) {
            Debug.Log("Shouldn't be called from non-server");
            return;
        }

        isInitialized.Value = true;
        players = new NetworkList<Player>();

        Debug.Log("Initialized!");
    }

    public void AddPlayer(Player p) {
        Debug.Log("AddPlayer called on PlayerManager");

        if (!IsInitialized()) {
            return;
        }

        if (!IsServer) {
            Debug.Log("Shouldn't be called from non-server");
            return;
        }

        players.Add(p);

        Debug.Log("Added");
    }

    public void RemovePlayer(Player p) {
        Debug.Log("RemovePlayer<Player> called on PlayerManager");

        if (!IsInitialized()) {
            return;
        }

        if (!IsServer) {
            Debug.Log("Shouldn't be called from non-server");
            return;
        }

        players.Remove(p);

        Debug.Log("Removed");
    }

    public void RemovePlayer(ulong clientId) {
        Debug.Log("RemovePlayer<ulong> called on PlayerManager");

        if (!IsInitialized()) {
            return;
        }

        if (!IsServer) {
            Debug.Log("Shouldn't be called from non-server");
            return;
        }

        if (TryGetPlayer(clientId, out Player p)) {
            players.Remove(p);
            Debug.Log("Removed successfully");
        } else {
            Debug.Log("Player with ID " + clientId + " was not found");
        }
    }

    public bool TryGetPlayer(ulong clientId, out Player player) {
        Debug.Log("TryGetPlayer called on PlayerManager");

        if (!IsInitialized()) {
            player = new Player();
            return false;
        }

        foreach (var p in players) {
            if (p.clientId == clientId) {
                player = p;
                Debug.Log("Found");
                return true;
            }
        }
        player = new Player();

        Debug.Log("Not found");
        return false;
    }

    public List<Player> GetPlayerList() {
        Debug.Log("GetPlayerList called on PlayerManager");

        if (!IsInitialized()) {
            Debug.Log("Not initialized yet, returning empty list");
            return new List<Player>();
        }

        List<Player> list = new List<Player>(players.Count);
        foreach (var player in players) {
            list.Add(player);
        }

        Debug.Log("List of players return. Count=" + list.Count);

        return list;
    }

    public void AddOnListChangedListener(NetworkList<Player>.OnListChangedDelegate listener) {
        Debug.Log("AddOnListChangedListener called on PlayerManager");

        if (!IsInitialized()) {
            return;
        }
        players.OnListChanged += listener;

        Debug.Log("OnListChangedListener added to PlayerManager");
    }

    public static bool IsInitialized() {
        return Instance != null && Instance.isInitialized != null && Instance.isInitialized.Value;
    }

    public static async Task WaitUntilInitialized() {
        while (!IsInitialized()) {
            Debug.Log("Waiting for PlayerManager initialization");
            await Task.Delay(500);
        }
    }
}
