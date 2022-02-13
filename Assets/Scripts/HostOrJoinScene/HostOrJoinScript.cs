using System;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class HostOrJoinScript : MonoBehaviour
{
    public RectTransform viewHome;
    public RectTransform viewHost;

    // Called by button onclick event
    public void ShowView(RectTransform view)
    {
        viewHome.anchorMin = new Vector2(-1, 0);
        viewHome.anchorMax = new Vector2(0, 1);
        viewHost.anchorMin = new Vector2(1, 0);
        viewHost.anchorMax = new Vector2(2, 1);

        view.anchorMin = new Vector2(0, 0);
        view.anchorMax = new Vector2(1, 1);
    }

    private async Task<Allocation> CreateAllocation(int maxConnections, string region = null)
    {
        try
        {
            Allocation allocation = await Relay.Instance.CreateAllocationAsync(maxConnections, region);
            Debug.Log($"server conn: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
            Debug.Log($"server alloc: {allocation.AllocationId}");

            return allocation;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to allocate game: {ex}", this);
            throw;
        }
    }

    private async Task<string> GetJoinCode(Guid allocationId)
    {
        try
        {
            return await Relay.Instance.GetJoinCodeAsync(allocationId);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to join code: {ex}", this);
            throw;
        }
    }
}

