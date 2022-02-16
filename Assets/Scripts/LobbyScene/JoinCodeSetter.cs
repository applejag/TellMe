using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinCodeSetter : MonoBehaviour
{
    public JoinCodeScript joinCodeScript;

    private void Start() {
        Debug.Log("Setting Join Code to '" + NetworkSessionData.joinCode.ToUpper() + "'");
        joinCodeScript.SetCode(NetworkSessionData.joinCode.ToUpper());
    }
}
