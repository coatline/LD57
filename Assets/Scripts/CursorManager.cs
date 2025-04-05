using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Awake()
    {
        PauseMenu.Paused += ReleaseCursor;
        PauseMenu.Resumed += LockCursor;
        DebugMenu.Enabled += ReleaseCursor;
        DebugMenu.Disabled += LockCursor;
    }

    public void ReleaseCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
