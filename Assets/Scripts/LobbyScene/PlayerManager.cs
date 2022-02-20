using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : NetworkBehaviour
{
    private NetworkList<Player> players;

    public event NetworkList<Player>.OnListChangedDelegate PlayerListChanged
    {
        add => players.OnListChanged += value;
        remove => players.OnListChanged -= value;
    }

    private void Awake()
    {
        players = new NetworkList<Player>();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (IsServer)
        {
            NetworkObject.Spawn(destroyWithScene: false);
        }
    }

    public void AddPlayer(Player p)
    {
        Debug.Log("AddPlayer called on PlayerManager");

        if (!IsServer)
        {
            Debug.Log("Shouldn't be called from non-server");
            return;
        }

        players.Add(p);

        Debug.Log("Added");
    }

    public void RemovePlayer(Player p)
    {
        Debug.Log("RemovePlayer<Player> called on PlayerManager");

        if (!IsServer)
        {
            Debug.Log("Shouldn't be called from non-server");
            return;
        }

        if (players.Remove(p))
        {
            Debug.Log("Removed successfully");
        }
        else
        {
            Debug.Log("Player with ID " + p.clientId + " was not found");
        }
    }

    public void RemovePlayer(ulong clientId)
    {
        if (!IsServer)
        {
            Debug.Log("Shouldn't be called from non-server");
            return;
        }

        if (TryGetPlayer(clientId, out Player p) && players.Remove(p))
        {
            Debug.Log("Removed successfully");
        }
        else
        {
            Debug.Log("Player with ID " + clientId + " was not found");
        }
    }

    public bool TryGetPlayer(ulong clientId, out Player player)
    {
        foreach (var p in players)
        {
            if (p.clientId == clientId)
            {
                player = p;
                return true;
            }
        }

        player = new Player();
        return false;
    }

    public List<Player> GetPlayerList()
    {
        var list = new List<Player>(players.Count);
        foreach (var player in players)
        {
            list.Add(player);
        }
        return list;
    }
}
