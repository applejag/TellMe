using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraHandleScript : MonoBehaviour
{
    private static CameraHandleScript activeCamera;
    private readonly static Dictionary<Key, CameraHandleScript> allCameras = new();

    [SerializeField, ReadOnlyDuringRuntime]
    private Kind kind;
    public GameObject cameraGameObj;

    public Key RegisteredKey { get; private set; }

    private void Start()
    {
        if (cameraGameObj.activeSelf)
        {
            if (!activeCamera)
            {
                activeCamera = this;
            }
            else
            {
                Debug.LogWarning("Multiple cameras active. Disabling self.", this);
                DeactivateCamera();
            }
        }

        var clientId = 0uL;
        var netObj = GetComponentInParent<NetworkObject>();
        if (netObj != null)
        {
            clientId = netObj.OwnerClientId;
        }

        RegisteredKey = new Key(kind, clientId);
        if (!allCameras.TryAdd(RegisteredKey, this))
        {
            Debug.LogWarning($"Failed to add camera due to key collision: {RegisteredKey}", this);
        }
    }

    private void Reset()
    {
        var camera = GetComponentInChildren<Camera>();
        if (camera)
        {
            cameraGameObj = camera.gameObject;
        }
    }

    private void ActivateCamera()
    {
        cameraGameObj.SetActive(true);
    }

    private void DeactivateCamera()
    {
        cameraGameObj.SetActive(false);
    }

    public enum Kind
    {
        Lobby,
        QuestionBoard,
        // 0x00-0x0F reserved for scene cameras

        HostCloseUp = 0x10,
        // 0x10-0x1F reserved for host cameras

        PlayerCloseUp = 0x20,
        // 0x20-0x2F reserved for player cameras
    }

    public struct Key : IEquatable<Key>
    {
        public Kind kind;
        public ulong clientId;

        public Key(Kind kind, ulong clientId = 0)
        {
            this.kind = kind;
            this.clientId = clientId;
        }

        public override string ToString()
        {
            return $"{kind}({clientId})";
        }

        public override bool Equals(object obj)
        {
            return obj is Key key && Equals(key);
        }

        public bool Equals(Key other)
        {
            return kind == other.kind && clientId == other.clientId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(kind, clientId);
        }

        public static bool operator ==(Key left, Key right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Key left, Key right)
        {
            return !(left == right);
        }
    }
}
