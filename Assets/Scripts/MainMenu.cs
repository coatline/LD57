using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] SoundType playSound;

    public void Play()
    {
        SceneFader.I.LoadNewScene("Backstory", 1f);
        SoundPlayer.I.PlaySound(playSound, transform.position, 1, 0);
    }
}
