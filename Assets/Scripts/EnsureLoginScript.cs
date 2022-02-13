using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnsureLoginScript : MonoBehaviour
{
    private IEnumerator Start()
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
            yield return null;
            var loginScript = FindObjectOfType<LoginScript>();
            loginScript.nextSceneName = currentScene.name;
        }
    }
}
