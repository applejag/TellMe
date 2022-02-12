using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScript : MonoBehaviour
{
    public string nextSceneName;

    public StepStatus stepInit;
    public StepStatus stepLogin;
    public StepStatus stepLoadScene;

    public CanvasGroup canvasGroup;
    public float fadeOutDuration = 2;
    public AnimationCurve fadeOutAlpha = AnimationCurve.Linear(0, 1, 1, 0);

    private async void Start()
    {
        if (!stepInit || !stepLogin || !stepLoadScene || !canvasGroup)
        {
            Debug.LogWarning("Missing required fields. Aborting.", this);
            enabled = false;
            return;
        }

        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("No next scene has been set.", this);
            enabled = false;
            return;
        }

        await InitializeUnityServicesAsync();
        await SignInAnonymouslyAsync();
        StartCoroutine(LoadSceneCoroutine());
    }

    public async Task InitializeUnityServicesAsync()
    {
        try
        {
            await UnityServices.InitializeAsync();
            stepInit.SetOK();
        }
        catch (Exception e)
        {
            stepInit.SetError(e.Message);
            throw;
        }
    }

    public async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            stepLogin.SetOK("Logged in anonymously");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogWarning($"Login auth failed, error code {ex.ErrorCode}: {ex.Message}", this);
            stepLogin.SetError($"Login auth error code: {ex.ErrorCode}");
            throw;
        }
        catch (RequestFailedException ex)
        {
            Debug.LogWarning($"Login request failed, error code {ex.ErrorCode}: {ex.Message}", this);
            stepLogin.SetError($"Login request error code: {ex.ErrorCode}");
            throw;
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Login failed unexpectedly: {ex.Message}", this);
            stepLogin.SetError(ex.Message);
            throw;
        }
    }

    public IEnumerator LoadSceneCoroutine()
    {
        var activeScene = SceneManager.GetActiveScene();
        yield return SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        stepLoadScene.SetOK();

        var time = 0f;
        var fraction = 1f / fadeOutDuration;
        while (time < 1)
        {
            yield return null;
            time += fraction * Time.deltaTime;
            canvasGroup.alpha = fadeOutAlpha.Evaluate(time);
        }
        canvasGroup.alpha = fadeOutAlpha.Evaluate(1);

        SceneManager.UnloadSceneAsync(activeScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
    }
}
