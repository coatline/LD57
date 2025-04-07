using UnityEngine;

public class IntroCutscene : Cutscene
{
    [SerializeField] CutscenePlayer cutscenePlayer;
    [SerializeField] SoundType decendSound;

    private void Start()
    {
        cutscenePlayer.PlayCutscene(this);
    }

    protected override void Finish()
    {
        base.Finish();
        SoundPlayer.I.PlaySound(decendSound, transform.position);
        SceneFader.I.LoadNewScene("Game", 0.5f);
    }
}
