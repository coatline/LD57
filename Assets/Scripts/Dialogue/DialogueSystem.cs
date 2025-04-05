using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public event System.Action SentenceStarted;

    [SerializeField] DialogueUI dialogueUI;

    string[] currentConversation;
    int sentenceIndex;

    public void DisplayConversation(string[] conversation)
    {
        sentenceIndex = 0;
        currentConversation = conversation;

        StartNextSentence();
    }

    public void PressedNext()
    {
        if (dialogueUI.FinishedTyping())
            StartNextSentence();
        else
            dialogueUI.Skip();
    }

    void StartNextSentence()
    {
        SentenceStarted?.Invoke();

        if (++sentenceIndex >= currentConversation.Length)
            EndConversation();
        else
            dialogueUI.StartTyping(currentConversation[sentenceIndex]);
    }

    void EndConversation()
    {
        dialogueUI.Hide();
    }
}
