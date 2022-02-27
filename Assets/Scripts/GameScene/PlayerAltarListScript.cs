using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class PlayerAltarListScript : NetworkBehaviour
{
    public GameObject altarPrefab;
    public Transform[] spawnPositions;
    public GameStateScript gameState;

    private void Start()
    {
        if (!gameState)
        {
            Debug.LogWarning("Lacking game state script", this);
            enabled = false;
            return;
        }

        if (IsServer)
        {
            gameState.playerList.OnListChanged += ServerOnPlayerListChanged;
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        if (gameState)
        {
            gameState.playerList.OnListChanged -= ServerOnPlayerListChanged;
        }
    }

    private void ServerOnPlayerListChanged(NetworkListEvent<NetworkBehaviourReference> changeEvent)
    {
        if (changeEvent.Type.IsAddOrInsert())
        {
            if (changeEvent.Value.TryGet(out PlayerScript playerScript) && !playerScript.IsOwnedByServer)
            {
                // Spawn in player
                SpawnPlayer(playerScript);
            }
        }
    }

    private void SpawnPlayer(PlayerScript playerScript)
    {
        var spawnPoint = FindNextSpawnPoint();
        if (!spawnPoint)
        {
            Debug.LogWarning($"Unable to find spawn point for player clientId={playerScript.OwnerClientId}", this);
            return;
        }
        var clone = Instantiate(altarPrefab, spawnPoint);
        clone.GetComponent<NetworkObject>().SpawnAsPlayerObject(playerScript.OwnerClientId, destroyWithScene: true);
    }

    private Transform FindNextSpawnPoint()
    {
        foreach (var spawn in spawnPositions)
        {
            var hasPlayer = spawn.GetComponentInChildren<NetworkObject>() == null;
            if (hasPlayer)
            {
                return spawn;
            }
        }
        return null;
    }
}
