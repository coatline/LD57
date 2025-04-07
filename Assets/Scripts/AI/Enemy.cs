using UnityEngine;

public class Enemy : MonoBehaviour
{
    PlayerController player;
    bool following;

    void Start()
    {
        player = LevelManager.I.PlayerController;
    }

    void Update()
    {
        if (following)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.GetChild(0).position, Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, player.transform.GetChild(0).position) < 5f)
        {
            following = true;
        }
    }
}
