using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartGameUIScript : MonoBehaviour
{
    public Button button;
    public TMP_Text buttonText;
    [Multiline]
    public string buttonTextFormat = @"Start game
<size=70%>({0}/{1} players ready)</size>";
    private GameStateScript gameState;

    private void Reset()
    {
        button = GetComponentInChildren<Button>();
        buttonText = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogWarning("Lacking network manager", this);
            enabled = false;
            return;
        }

        if (!NetworkManager.Singleton.IsHost)
        {
            gameObject.SetActive(false);
            return;
        }

        gameState = FindObjectOfType<GameStateScript>();
        if (!gameState)
        {
            Debug.LogWarning("Lacking game state.", this);
            enabled = false;
            return;
        }

        gameState.playerCount.OnValueChanged += OnPlayersOrReadinessChanged;
        gameState.playersReadyCount.OnValueChanged += OnPlayersOrReadinessChanged;
        UpdateButton(0, 0);
    }

    private void OnDestroy()
    {
        if (gameState)
        {
            gameState.playerCount.OnValueChanged -= OnPlayersOrReadinessChanged;
            gameState.playersReadyCount.OnValueChanged -= OnPlayersOrReadinessChanged;
        }
    }

    private void UpdateButton(int players, int readyPlayers)
    {
        button.interactable = players > 0 && readyPlayers >= players;
        buttonText.text = string.Format(buttonTextFormat, readyPlayers, players);
    }

    private void OnPlayersOrReadinessChanged(int _, int __)
    {
        UpdateButton(gameState.playerCount.Value, gameState.playersReadyCount.Value);
    }

    public void OnStartGameButtonClicked()
    {
        if (!enabled)
        {
            return;
        }
    }
}
