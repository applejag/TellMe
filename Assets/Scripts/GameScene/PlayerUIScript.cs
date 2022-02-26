using TMPro;
using Unity.Collections;
using UnityEngine;

public class PlayerUIScript : MonoBehaviour
{
    public TMP_Text textName;
    public TMP_Text textType;
    private PlayerScript previousPlayer;

    private void OnDestroy()
    {
        if (previousPlayer != null)
        {
            previousPlayer.playerName.OnValueChanged -= OnPlayerNameChanged;
        }
    }

    public void SetPlayer(PlayerScript player)
    {
        if (previousPlayer!=null)
        {
            previousPlayer.playerName.OnValueChanged -= OnPlayerNameChanged;
        }
        player.playerName.OnValueChanged += OnPlayerNameChanged;

        textName.text = player.playerName.Value.Value;
        var type = player.IsOwnedByServer ? "HOST" : "CONTESTANT";
        if (player.IsLocalPlayer)
        {
            type += " (you)";
        }
        textType.text = type;
        previousPlayer = player;
    }

    private void OnPlayerNameChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        textName.text = newValue.Value;
    }
}
