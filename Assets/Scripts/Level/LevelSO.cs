using UnityEngine;

[CreateAssetMenu()]
public class LevelSO : ScriptableObject
{
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject ceilingPrefab;
    public GameObject torchPrefab;
    public int maxIterations;
    public int maxDistance;
    public int enemies;
    public int size;
}
