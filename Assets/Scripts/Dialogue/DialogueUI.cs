using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public event System.Action FinishedTyping;

    [SerializeField] GameObject continueArrow;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] Animator letterbox;
    [SerializeField] GameObject visuals;
    [SerializeField] float charDelay;
    [SerializeField] SoundType typeSound;

    IntervalTimer nextCharTimer;
    float currentCharDelay;
    Sentence toType;
    int charIndex;

    bool active;

    private void Awake()
    {
        nextCharTimer.Stop();

#if UNITY_EDITOR
        charDelay /= 4f;
#endif
    }

    public void Hide()
    {
        visuals.SetActive(false);
        letterbox.Play("Hide");
        active = false;
    }

    public void ShowContinueArrow()
    {
        continueArrow.SetActive(true);
    }

    public void ClearText()
    {
        dialogueText.text = "";
        continueArrow.SetActive(false);
    }

    public void StartTyping(Sentence toType)
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

        continueArrow.SetActive(false);
        nextCharTimer.StartWithInterval(currentCharDelay);
    }

    public void Skip()
    {
        nextCharTimer.Timer = 0;
        currentCharDelay = charDelay / 4f;
        currentCharDelay /= 4f;
    }

    private void Update()
    {
        if (nextCharTimer.DecrementIfRunning(Time.deltaTime))
        {
            char c = toType.text[charIndex++];

            if (c != ' ')
                SoundPlayer.I.PlaySound(typeSound, transform.position, 1, 0);

            dialogueText.text += c;

            if (IsFinished())
            {
                nextCharTimer.Stop();
                FinishedTyping?.Invoke();
            }
            else
                nextCharTimer.StartWithInterval(currentCharDelay);
        }
    }

    public bool IsFinished() => active = false || charIndex >= toType.text.Length;
}
