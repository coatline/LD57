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
        player = FindFirstObjectByType<PlayerController>().transform.GetChild(0);
    }

    void Update()
    {
        if (timer.DecrementIfRunning(Time.deltaTime))
        {
            SoundPlayer.I.PlaySound(scarySound, player.transform.position + new Vector3(Random.Range(-15f, 15f), Random.Range(-15f, 15f)));
            timer.StartWithInterval(Random.Range(minInterval, maxInterval));
        }
    }
}
