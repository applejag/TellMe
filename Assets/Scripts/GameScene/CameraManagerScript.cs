using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class CameraManagerScript : NetworkBehaviour
{
    public static CameraManagerScript Instance => instance ? instance : throw new InvalidOperationException("Lacking singleton camera manager.");
    private static CameraManagerScript instance;

    private CameraScript activeCamera;
    private readonly Dictionary<CameraScript.Key, CameraScript> allCameras = new();

    private CameraScript.Key? serverPrevNonTempCamera;

    private void Awake()
    {
        instance = this;
    }

    public CameraScript.Key? RegisterCamera(CameraScript script, CameraScript.Kind kind)
    {
        if (script.cameraGameObj.activeSelf)
        {
            if (!activeCamera)
            {
                activeCamera = script;
            }
            else
            {
                Debug.LogWarning("Multiple cameras active. Disabling camera.", script);
                script.DeactivateCamera();
            }
        }

        var clientId = 0uL;
        var netObj = script.GetComponentInParent<NetworkObject>();
        if (netObj != null)
        {
            clientId = netObj.OwnerClientId;
        }

        var key = new CameraScript.Key(kind, clientId);
        if (!allCameras.TryAdd(key, script))
        {
            Debug.LogWarning($"Failed to add camera due to key collision: {key}", script);
            return null;
        }
        return key;
    }

    public bool UnregisterCamera(CameraScript.Key key)
    {
        return allCameras.Remove(key);
    }

    public void ActivateCameraServer(CameraScript.Kind kind, ulong clientId)
    {
        ActivateCameraServer(new CameraScript.Key(kind, clientId));
    }

    public void ActivateCameraServer(CameraScript.Key key)
    {
        if (!IsServer)
        {
            Debug.LogWarning($"{nameof(ActivateCameraServer)} was not called from the server.", this);
            return;
        }

        serverPrevNonTempCamera = null;
        ActivateCameraClientRpc(key);
    }

    public void ActivateCameraTemporarilyServer(CameraScript.Kind kind, ulong clientId, float duration)
    {
        ActivateCameraTemporarilyServer(new CameraScript.Key(kind, clientId), duration);
    }

    public void ActivateCameraTemporarilyServer(CameraScript.Key tempKey, float duration)
    {
        if (!IsServer)
        {
            Debug.LogWarning($"{nameof(ActivateCameraTemporarilyServer)} was not called from the server.", this);
            return;
        }

        if (activeCamera && !serverPrevNonTempCamera.HasValue)
        {
            serverPrevNonTempCamera = activeCamera.RegisteredKey;
        }

        if (serverPrevNonTempCamera.HasValue)
        {
            ActivateCameraTemporarilyClientRpc(tempKey, serverPrevNonTempCamera.Value, duration);
        }
        else
        {
            ActivateCameraClientRpc(tempKey);
        }
    }

    [ClientRpc]
    private void ActivateCameraClientRpc(CameraScript.Key key)
    {
        ActivateCamera(key);
    }

    [ClientRpc]
    private void ActivateCameraTemporarilyClientRpc(CameraScript.Key tempKey, CameraScript.Key prevKey, float duration)
    {
        StartCoroutine(ActivateCameraTemporarilyCoroutine(tempKey, prevKey, duration));
    }

    private IEnumerator ActivateCameraTemporarilyCoroutine(CameraScript.Key tempKey, CameraScript.Key prevKey, float duration)
    {
        ActivateCamera(tempKey);
        yield return new WaitForSeconds(duration);
        ActivateCamera(prevKey);
    }

    private void ActivateCamera(CameraScript.Key key)
    {
        if (!allCameras.TryGetValue(key, out var cameraScript))
        {
            Debug.LogWarning($"Tried to activate unregistered camera: {key}", this);
            return;
        }

        if (activeCamera == cameraScript)
        {
            return;
        }

        if (activeCamera)
        {
            activeCamera.DeactivateCamera();
        }

        cameraScript.ActivateCamera();
        activeCamera = cameraScript;
    }

}
