using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    public static event System.Action Enabled;
    public static event System.Action Disabled;

    [SerializeField] GameObject visuals;

    bool active;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (active)
                Disable();
            else
                Enable();
        }
    }

    void Enable()
    {
        active = true;
        visuals.SetActive(true);
        Enabled?.Invoke();
    }

    void Disable()
    {
        active = false;
        visuals.SetActive(false);
        Disabled?.Invoke();
    }

    public void ReloadScene()
    {
        SceneFader.I.ReloadCurrentScene(0.1f);
    }

    public void NextScene(int direction)
    {
        SceneFader.I.LoadSceneInDirection(direction);
    }
}
