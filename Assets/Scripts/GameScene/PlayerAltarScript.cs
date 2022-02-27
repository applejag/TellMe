using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class PlayerAltarScript : NetworkBehaviour
{
    public TMP_Text textPlayerName;
    public SegmentDisplay displayScore;
    private PlayerScript playerScript;

    private void Reset()
    {
        textPlayerName = GetComponentInChildren<TMP_Text>();
        displayScore = GetComponentInChildren<SegmentDisplay>();
    }

    private void Start()
    {
        var gameState = FindObjectOfType<GameStateScript>();
        if (!gameState)
        {
            Debug.LogWarning("Unable to find game state script.", this);
            return;
        }

        playerScript = gameState.GetPlayerScript(OwnerClientId);
        if (!playerScript)
        {
            Debug.LogWarning($"Unable to find player script for clientId={OwnerClientId}", this);
            return;
        }
        playerScript.playerName.OnValueChanged += OnPlayerNameChanged;
        textPlayerName.text = playerScript.playerName.Value.ToString();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        if (playerScript)
        {
            playerScript.playerName.OnValueChanged -= OnPlayerNameChanged;
        }
    }

    private void OnPlayerNameChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        textPlayerName.text = newValue.Value;
    }
}
