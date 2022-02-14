using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameScript : MonoBehaviour
{
    public ValidatedField fieldJoinCode;
    public StatusStack statusStack;

    public Selectable[] disableOnLoad;

    public void OnJoinGameClick()
    {
        var joinCodeValid = fieldJoinCode.Validate();
        if (!joinCodeValid)
        {
            return;
        }

        JoinGame(fieldJoinCode.field.text.ToUpper());
    }

    private void SetFormInteractable(bool interactable)
    {
        foreach (var obj in disableOnLoad)
        {
            obj.interactable = interactable;
        }
    }

    private async void JoinGame(string joinCode)
    {
        SetFormInteractable(false);

        statusStack.ClearStatuses();
        var allocationStatus = statusStack.AddStatus("Join game");
        var startClientStatus = statusStack.AddStatus("Start client");

        JoinAllocation allocation;
        try
        {
            allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
            allocationStatus.SetOK("Joined game");
        }
        // Haven't found any documentation, but some manual testing suggests that 15001
        // is the "wrong code" or "invalid code" error code
        catch (RelayServiceException ex) when (ex.Reason == RelayExceptionReason.InvalidRequest && ex.ErrorCode == 15001)
        {
            Debug.LogError($"Failed to join by join code (reason={ex.Reason}, code={ex.ErrorCode}): {ex}", this);
            SetFormInteractable(true);
            allocationStatus.SetError($"Join game: wrong join code");
            return;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to join by join code: {ex}", this);
            SetFormInteractable(true);
            allocationStatus.SetError($"Join game: {ex.Message}");
            return;
        }

        try
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                ipAddress: allocation.RelayServer.IpV4,
                port: (ushort)allocation.RelayServer.Port,
                allocationId: allocation.AllocationIdBytes,
                key: allocation.Key,
                connectionData: allocation.ConnectionData,
                hostConnectionData: allocation.HostConnectionData
            );
            NetworkManager.Singleton.StartClient();
            startClientStatus.SetOK();

            var rand = new System.Random();
            string randomName = "0x"+(rand.Next() % 10000).ToString("X4");
            NotifyOfJoinServerRpc(randomName);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to start client: {ex}", this);
            SetFormInteractable(true);
            startClientStatus.SetError($"Start client: " + ex.Message);
            return;
        }
    }

    [ServerRpc]
    private void NotifyOfJoinServerRpc(string name) {
        FindObjectOfType<PlayerList>().Add(name);
    }
}
