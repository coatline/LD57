using UnityEngine;

public class LevelCompleteCutscene : Cutscene
{
    [SerializeField] BoxCollider boxCollider;

    protected override void Finish()
    {
        boxCollider.enabled = false;
        LevelManager.I.CompleteLevel();
    }
}
