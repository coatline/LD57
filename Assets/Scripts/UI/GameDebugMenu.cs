using UnityEngine;

public class GameDebugMenu : DebugMenu
{
    [SerializeField] GemStone gemStonePrefab;

    public void GiveGemStone()
    {
        Instantiate(gemStonePrefab, LevelManager.I.PlayerController.transform.GetChild(0).position, Quaternion.identity);
    }
}
