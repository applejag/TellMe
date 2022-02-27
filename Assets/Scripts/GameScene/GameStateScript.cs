using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

public class GameStateScript : NetworkBehaviour
{
    private readonly Dictionary<ulong, PlayerScript> serverPlayerScripts = new();

    public NetworkVariable<bool> isGameStarted = new();
    /// <summary>Number of players, excluding the host.</summary>
    public NetworkVariable<int> playerCount = new();
    /// <summary>Number of players that are ready, excluding the host.</summary>
    public NetworkVariable<int> playersReadyCount = new();
    public NetworkList<NetworkBehaviourReference> playerList;
    public IEnumerable<PlayerScript> PlayerScripts => playerList?.AsEnumerable()
        .Select(behRef => behRef.TryGet(out PlayerScript p) ? p : null).Where(p => p);

    private void Awake()
    {
        // Cant initialize NetworkList in ctor :(
        playerList = new NetworkList<NetworkBehaviourReference>();
    }

    private void Start()
    {
        if (IsHost)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += ServerOnClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback += ServerOnClientDisconnect;
            var hostPlayerObj = NetworkManager.Singleton.LocalClient.PlayerObject;
            var hostPlayerScript = hostPlayerObj.GetComponent<PlayerScript>();
            serverPlayerScripts[hostPlayerObj.OwnerClientId] = hostPlayerScript;
            playerList.Add(new NetworkBehaviourReference(hostPlayerScript));
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

    public PlayerScript GetPlayerScript(ulong clientId)
    {
        foreach (var player in PlayerScripts)
        {
            if (player.OwnerClientId == clientId)
            {
                return player;
            }
        }
        return null;
    }

    private void ServerOnClientDisconnect(ulong id)
    {
        if (serverPlayerScripts.TryGetValue(id, out var playerScript) && serverPlayerScripts.Remove(id))
        {
            playerCount.Value--;
            if (playerScript.playerIsReady.Value)
            {
                playersReadyCount.Value--;
            }
            playerList.Remove(playerScript);
        }
    }

    private void ServerOnClientConnect(ulong id)
    {
        playerCount.Value++;
        var playerScript = NetworkManager.Singleton.ConnectedClients[id].PlayerObject.GetComponent<PlayerScript>();
        if (serverPlayerScripts.TryAdd(id, playerScript))
        {
            serverPlayerScripts[id] = playerScript;
            playerScript.playerIsReady.OnValueChanged += OnPlayerReadyChanged;
            playerList.Add(playerScript);
        }
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
