using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] GameObject finishedArrow;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] Animator letterbox;
    [SerializeField] GameObject visuals;
    [SerializeField] float charDelay;

    IntervalTimer nextCharTimer;
    float currentCharDelay;
    string toType;
    int charIndex;

    bool active;

    private void Awake()
    {
        nextCharTimer.Stop();
    }

    public void Hide()
    {
        visuals.SetActive(false);
        letterbox.Play("Hide");
        active = false;
    }

    public void StartTyping(string toType)
    {
        if (active == false)
        {
            active = true;
            visuals.SetActive(true);
            letterbox.Play("Show");
        }

        charIndex = 0;
        this.toType = toType;
        dialogueText.text = "";
        currentCharDelay = charDelay;

        finishedArrow.SetActive(false);
        nextCharTimer.StartWithInterval(currentCharDelay);
    }

    public void Skip()
    {
        nextCharTimer.Timer = 0;
        currentCharDelay /= 4f;
    }

    private void Update()
    {
        if (nextCharTimer.DecrementIfRunning(Time.deltaTime))
        {
            dialogueText.text += toType[charIndex++];

            if (FinishedTyping())
            {
                nextCharTimer.Stop();
                FinishSentence();
            }
            else
                nextCharTimer.StartWithInterval(currentCharDelay);
        }
    }

    void FinishSentence()
    {
        finishedArrow.SetActive(true);
    }

    public bool FinishedTyping() => active = false || charIndex >= toType.Length;
}
