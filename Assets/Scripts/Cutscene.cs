using UnityEngine;
using UnityEngine.InputSystem;

public class Cutscene : MonoBehaviour
{
    public event System.Action Finished;

    [SerializeField] CutsceneFrame[] frames;
    [SerializeField] Sentence[] conversation;

    IntervalTimer frameDelayTimer;
    IntervalTimer walkTimer;
    int currentFrameIndex;

    bool waitingForDialogue;
    bool waitingForFrameDelay;
    bool active;

    PlayerController player;

    public void Play(PlayerController playerController)
    {
        this.player = playerController;
        active = true;
        currentFrameIndex = 0;

        DialogueSystem.I.StartConversation(conversation);
        DialogueSystem.I.SentenceFinished += OnSentenceFinished;

        BeginFrame();
    }

    void Update()
    {
        if (active == false || waitingForDialogue)
            return;

        if (waitingForFrameDelay)
        {
            if (frameDelayTimer.DecrementIfRunning(Time.deltaTime))
            {
                frameDelayTimer.Stop();

                waitingForFrameDelay = false;

                walkTimer.StartWithInterval(CurrentFrame.moveDuration);
                player.transform.GetChild(0).GetComponent<PlayerMovement>().SetIsSprinting(CurrentFrame.sprint);

                if (CurrentFrame.jump)
                    player.transform.GetChild(0).GetComponent<Jumper>().TryJump();
            }
            else
                return;
        }

        if (waitingForDialogue)
            return;

        if (walkTimer.DecrementIfRunning(Time.deltaTime))
        {
            walkTimer.Stop();

            player.transform.GetChild(0).GetComponent<PlayerMovement>().SetMoveInput(Vector2.zero);

            OnFrameFinished();
        }
        else
        {
            player.transform.GetChild(0).GetComponent<PlayerMovement>().SetMoveInput(CurrentFrame.moveDirection);
            return;
        }
    }

    void BeginFrame()
    {
        //print($"Frame started.. {currentFrameIndex}/{frames.Length - 1}");
        frameDelayTimer.StartWithInterval(CurrentFrame.startDelay);
        waitingForFrameDelay = true;

        if (currentFrameIndex > 0)
            DialogueSystem.I.StartNextSentence();

        waitingForDialogue = DialogueSystem.I.Active;
    }

    void OnSentenceFinished()
    {
        waitingForDialogue = false;
    }

    void OnFrameFinished()
    {
        //print($"Frame finished");

        if (++currentFrameIndex >= frames.Length)
            Finish();
        else
            BeginFrame();
    }

    protected virtual void Finish()
    {
        //print($"Finished cutscene!");
        active = false;
        DialogueSystem.I.EndConversation();
        DialogueSystem.I.SentenceFinished -= OnSentenceFinished;
        Finished?.Invoke();
    }

    CutsceneFrame CurrentFrame =>
        (currentFrameIndex >= 0 && currentFrameIndex < frames.Length) ? frames[currentFrameIndex] : default;
}

[System.Serializable]
struct CutsceneFrame
{
    public float startDelay;
    public float moveDuration;
    public Vector2 moveDirection;
    public bool jump;
    public bool sprint;
}