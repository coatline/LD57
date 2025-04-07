using TMPro;
using UnityEngine;

public class DialogueSystem : Singleton<DialogueSystem>
{
    public event System.Action ConversationEnded;
    public event System.Action SentenceFinished;

    [SerializeField] DialogueUI dialogueUI;

    public bool Active { get; private set; }

    IntervalTimer automaticAdvanceTimer;
    Sentence[] currentConversation;
    int sentenceIndex;

    protected override void Awake()
    {
        base.Awake();
        dialogueUI.FinishedTyping += DialogueUI_FinishedTyping;

        automaticAdvanceTimer.Stop();
    }

    public void StartConversation(Sentence[] conversation)
    {
        sentenceIndex = -1;
        currentConversation = conversation;
        Active = true;

        StartNextSentence();
    }

    public void StartNextSentence()
    {
        //print($"Active: {Active} trying to start next sentence");

        if (Active == false)
            return;

        if (++sentenceIndex >= currentConversation.Length)
            EndConversation();
        else
            dialogueUI.StartTyping(CurrentSentence);
    }

    public void PressedNext()
    {
        if (Active == false)
            return;

        if (dialogueUI.IsFinished())
        {
            if (CurrentSentence.automaticAdvance == false)
                FinishSentence();
        }
        else if (CurrentSentence.unskippable == false)
            dialogueUI.Skip();
    }

    private void DialogueUI_FinishedTyping()
    {
        if (Active == false)
            return;

        //print("Finished typing ");
        if (CurrentSentence.automaticAdvance)
            automaticAdvanceTimer.StartWithInterval(0.5f);
        else
            dialogueUI.ShowContinueArrow();
    }

    void FinishSentence()
    {
        //print("Finished sentence");
        automaticAdvanceTimer.Stop();

        if (CurrentSentence.hideOnComplete)
            dialogueUI.ClearText();
        else
            StartNextSentence();

        SentenceFinished?.Invoke();
    }

    public void EndConversation()
    {
        //print("Ended conversation");
        Active = false;
        currentConversation = null;
        dialogueUI.Hide();
        ConversationEnded?.Invoke();
    }

    private void Update()
    {
        if (automaticAdvanceTimer.DecrementIfRunning(Time.deltaTime))
        {
            automaticAdvanceTimer.Stop();
            FinishSentence();
        }
    }

    Sentence CurrentSentence => currentConversation[sentenceIndex];
}
