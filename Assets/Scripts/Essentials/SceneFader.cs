using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : Singleton<SceneFader>
{
    public event System.Action<string> SceneJustLoaded;

    [SerializeField] Image fadePrefab;
    Image thisSceneImage;

    bool calledLoaded;

    protected override void Awake()
    {
        base.Awake();

        if (markedForDestroy) return;

        SceneManager.activeSceneChanged += OnSceneLoaded;

        OnSceneLoaded(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
    }

    void OnSceneLoaded(Scene scene1, Scene scene2)
    {
        if (calledLoaded == true) return;
        calledLoaded = true;

        if (thisSceneImage != null)
            return;

        Image i = C.FindObjectOfNameFromArray("FadeOut(Clone)", FindObjectsByType<Image>(FindObjectsSortMode.None)) as Image;

        if (i != null)
        {
            thisSceneImage = i;
            return;
        }

        SceneJustLoaded?.Invoke(SceneManager.GetActiveScene().name);

        Canvas c = C.FindObjectOfNameFromArray("Canvas", FindObjectsByType<Canvas>(FindObjectsSortMode.None)) as Canvas;
        thisSceneImage = Instantiate(fadePrefab, c.transform);

        thisSceneImage.color = Color.black;
        Time.timeScale = 1;
        thisSceneImage.CrossFadeAlpha(0, .5f, false);
    }

    public void ReloadCurrentScene(float fadeTime) => LoadNewScene(SceneManager.GetActiveScene().name, fadeTime);

    public void LoadNewScene(string sceneName, float fadeTime = 0.25f)
    {
        StartLoadingScene(sceneName, fadeTime);
    }

    public void LoadNewScene(int sceneIndex, float fadeTime = 0.25f)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
        StartLoadingScene(sceneName, fadeTime);
    }

    void StartLoadingScene(string name, float fadeTime)
    {
        StopAllCoroutines();
        thisSceneImage.CrossFadeAlpha(1, fadeTime, true);
        StartCoroutine(LoadNewSceneDelay(name, fadeTime + 0f));
    }

    public void LoadSceneInDirection(int dir)
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex + dir;

        print($"{buildIndex} {SceneManager.sceneCountInBuildSettings}");

        if (buildIndex >= SceneManager.sceneCountInBuildSettings)
            buildIndex = 0;
        else if (buildIndex < 0)
            buildIndex = SceneManager.sceneCountInBuildSettings - 1;

        LoadNewScene(buildIndex);
    }

    IEnumerator LoadNewSceneDelay(string sceneName, float fadeTime)
    {
        yield return new WaitForSecondsRealtime(fadeTime);
        calledLoaded = false;
        SceneManager.LoadScene(sceneName);
    }

    public void FadeOutAndIn(float fadeTime, float fadeInDelay)
    {
        StartCoroutine(FadeOutThenIn(fadeTime, fadeInDelay));
    }

    IEnumerator FadeOutThenIn(float fadeTime, float fadeInDelay)
    {
        thisSceneImage.CrossFadeAlpha(1, fadeTime, true);
        yield return new WaitForSecondsRealtime(fadeTime + fadeInDelay);
        thisSceneImage.CrossFadeAlpha(0, fadeTime, true);
    }
}