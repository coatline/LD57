using UnityEngine;

public class PauseMenu : Singleton<PauseMenu>
{
    public static System.Action Paused;
    public static System.Action Resumed;

    [SerializeField] GameObject visuals;

    public void TogglePause()
    {
        if (Time.timeScale == 1)
            Pause();
        else
            Resume();
    }

    void Pause()
    {
        visuals.SetActive(true);
        Time.timeScale = 0;
        IsPaused = true;
        Paused?.Invoke();
    }

    void Resume()
    {
        visuals.SetActive(false);
        Time.timeScale = 1;
        IsPaused = false;
        Resumed?.Invoke();
    }

    public bool IsPaused { get; private set; }
}
