using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField, ReadOnlyDuringRuntime]
    private Kind kind;
    public GameObject cameraGameObj;

    public Key RegisteredKey { get; private set; }

    private void Start()
    {
        var key = CameraManagerScript.Instance.RegisterCamera(this, kind);
        if (!key.HasValue)
        {
            enabled = false;
            return;
        }
        RegisteredKey = key.Value;
    }

    private void OnDestroy()
    {
        if (enabled)
        {
            CameraManagerScript.Instance.UnregisterCamera(RegisteredKey);
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

    public void ActivateCamera()
    {
        cameraGameObj.SetActive(true);
    }

    public void DeactivateCamera()
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

    public struct Key : IEquatable<Key>, INetworkSerializable
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

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref kind);
            serializer.SerializeValue(ref clientId);
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
