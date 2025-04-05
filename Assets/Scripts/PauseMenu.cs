using UnityEngine;

public class PauseMenu : Singleton<PauseMenu>
{
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    void Resume()
    {
        visuals.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
}
