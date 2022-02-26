using System;
using System.Collections.Concurrent;
using UnityEngine;

public class Dispatcher : MonoBehaviour
{
    private static Dispatcher instance;
    private readonly ConcurrentQueue<Action> actions = new();

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        while (actions.TryDequeue(out Action action))
        {
            action();
        }
    }

    public static void Invoke(Action action)
    {
        if (instance == null)
        {
            var go = new GameObject("Dispatcher", typeof(Dispatcher));
            instance = go.GetComponent<Dispatcher>();
        }
        instance.actions.Enqueue(action);
    }
}
