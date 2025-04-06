using UnityEngine;

public class CutscenePlayer : MonoBehaviour
{
    [SerializeField] string[] conversation;
    [SerializeField] DialogueSystem dialogueSystem;
    [SerializeField] Transform character;

    Vector3 targetPosition;

    void Start()
    {
        dialogueSystem.SentenceStarted += DialogueSystem_SentenceStarted;
        dialogueSystem.DisplayConversation(conversation);
    }

    private void DialogueSystem_SentenceStarted()
    {
        targetPosition.z += 1f;
        targetPosition.y = 1;

        if (targetPosition.z == 4)
            SceneFader.I.LoadNewScene("Game");
    }

    private void Update()
    {
        character.position = Vector3.MoveTowards(character.position, targetPosition, Time.deltaTime * 3);

        if (Input.GetMouseButtonDown(0))
            dialogueSystem.PressedNext();
    }
}
