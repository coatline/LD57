using UnityEngine;

public class LevelIntroCutscene : Cutscene
{
    [SerializeField] Collider boxCollider;

    protected override void Finish()
    {
        boxCollider.enabled = true;
        base.Finish();
    }
}
