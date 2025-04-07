using UnityEngine;
using UnityEngine.InputSystem;

public class CutscenePlayer : MonoBehaviour
{
    [SerializeField] InputAction nextAction;
    [SerializeField] PlayerController player;

    Cutscene currentCutscene;

    private void Awake()
    {
        nextAction.started += NextAction_started;
        nextAction.Enable();
    }

    public void PlayCutscene(Cutscene cutscene)
    {
        currentCutscene = cutscene;
        currentCutscene.Finished += CurrentCutscene_Finished;

        player.Disable();
        cutscene.Play(player);
    }

    private void CurrentCutscene_Finished()
    {
        currentCutscene.Finished -= CurrentCutscene_Finished;
        player.Enable();
        currentCutscene = null;
    }

    private void NextAction_started(InputAction.CallbackContext ctx)
    {
        if (currentCutscene != null)
            DialogueSystem.I.PressedNext();
    }
}