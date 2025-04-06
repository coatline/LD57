using UnityEngine;

public class Wellivator : MonoBehaviour
{
    [SerializeField] PlayerController playerPrefab;
    [SerializeField] float targetBucketY;
    [SerializeField] float animationTime;
    [SerializeField] Transform bucket;

    IntervalTimer animationTimer;
    float bucketStartingY;

    private void Awake()
    {
        bucketStartingY = bucket.transform.position.y;
        animationTimer.StartWithInterval(animationTime);

        PlayerController p = Instantiate(playerPrefab);
        p.transform.GetChild(0).position = new Vector3(bucket.position.x, bucket.position.y + 1, bucket.position.z);
    }

    private void Update()
    {
        if (animationTimer.IsRunning == false)
            return;

        if (animationTimer.DecrementIfRunning(Time.deltaTime))
        {
            animationTimer.Stop();
            bucket.transform.position = new Vector3(transform.position.x, targetBucketY, transform.position.z);
        }
        else
            bucket.transform.position = new Vector3(bucket.transform.position.x, Mathf.MoveTowards(bucketStartingY, targetBucketY, animationTimer.PercentageComplete), bucket.transform.position.z);
    }
}
