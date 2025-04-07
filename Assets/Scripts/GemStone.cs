using UnityEngine;

public class GemStone : HoldableObject
{
    [SerializeField] SoundType firstTimePickupSound;

    protected override void FirstTimeBeingPickedUp()
    {
        SoundPlayer.I.PlaySound(firstTimePickupSound, transform.position);
        LevelManager.I.GrabbedGemStone();
        base.FirstTimeBeingPickedUp();
    }
}
