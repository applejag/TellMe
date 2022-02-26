using System;
using Unity.Netcode;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HostGameScript : MonoBehaviour
{
    public ValidatedField fieldGameName;
    public ValidatedField fieldHostName;

    public Selectable[] disableOnLoad;

    public StatusStack statusStack;

    public string nextScene = "LobbyScene";

    public void OnHostGameClick()
    {
        var gameNameValid = fieldGameName.Validate();
        var hostNameValid = fieldHostName.Validate();

        if (!gameNameValid || !hostNameValid)
        {
            return;
        }

        HostGame();
    }

    private void SetFormInteractable(bool interactable)
    {
        foreach (var obj in disableOnLoad)
        {
            obj.interactable = interactable;
        }
    }

    private async void HostGame()
    {
        SetFormInteractable(false);
        statusStack.ClearStatuses();
        var allocationStatus = statusStack.AddStatus("Allocate game");
        var joinCodeStatus = statusStack.AddStatus("Get join code");
        var startHostStatus = statusStack.AddStatus("Start host");
        Allocation allocation;
        try
        {
            const int maxConnections = 10;
            const string region = null;
            allocation = await Relay.Instance.CreateAllocationAsync(maxConnections, region);
            Debug.Log($"server conn: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
            Debug.Log($"server alloc: {allocation.AllocationId}");
            allocationStatus.SetOK();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to allocate game: {ex}", this);
            allocationStatus.SetError("Allocate game: " + ex.Message);
            SetFormInteractable(true);
            return;
        }

        try
        {
            var joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            joinCodeStatus.SetOK("Join code: " + joinCode);
            Debug.Log(joinCode);
            NetworkSessionData.joinCode = joinCode;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to join code: {ex}", this);
            joinCodeStatus.SetError("Get join code: " + ex.Message);
            SetFormInteractable(true);
            return;
        }

        try
        {
            StartHost(allocation);
            startHostStatus.SetOK();
        }
        catch (Exception ex)
        {
            startHostStatus.SetError("Start host: " + ex.Message);
            SetFormInteractable(true);
            return;
        }

        NetworkManager.Singleton.SceneManager.LoadScene(nextScene, LoadSceneMode.Single);

        Debug.Log("done");
    }

    private void StartHost(Allocation allocation)
    {
        try
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                ipAddress: allocation.RelayServer.IpV4,
                port: (ushort)allocation.RelayServer.Port,
                allocationId: allocation.AllocationIdBytes,
                key: allocation.Key,
                connectionData: allocation.ConnectionData
            );
            NetworkManager.Singleton.StartHost();
            var localPlayerNetObj = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
            var localPlayer = localPlayerNetObj.GetComponent<PlayerScript>();
            localPlayer.playerName.Value = fieldHostName.field.text;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to start host: {ex}", this);
            throw;
        }
    }
}
