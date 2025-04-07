using UnityEngine;

public class ScarySoundPlayer : MonoBehaviour
{
    [SerializeField] float minInterval;
    [SerializeField] float maxInterval;
    [SerializeField] SoundType scarySound;

    IntervalTimer timer;
    Transform player;

    private void Awake()
    {
        timer.StartWithInterval(Random.Range(minInterval, maxInterval));
    }

    private void Start()
    {
        player = LevelManager.I.PlayerController.transform.GetChild(0);
    }

    void Update()
    {
        if (timer.DecrementIfRunning(Time.deltaTime))
        {
            SoundPlayer.I.PlaySound(scarySound, player.transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)));
            timer.StartWithInterval(Random.Range(minInterval, maxInterval));
        }
    }
}
