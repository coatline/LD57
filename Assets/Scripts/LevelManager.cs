using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public void GrabbedGemStone()
    {
        foreach (Light torch in FindObjectsByType<Light>(FindObjectsSortMode.None))
        {
            torch.intensity /= 2f;
        }
    }
}
