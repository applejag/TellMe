using UnityEngine;

public class NetworkSessionData : MonoBehaviour
{
    public static string joinCode;

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }
}
