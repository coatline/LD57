using UnityEngine;

[CreateAssetMenu()]
public class LevelSO : ScriptableObject
{
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject ceilingPrefab;
    public GameObject torchPrefab;
    public GameObject wellFloorPrefab;
    public float minDistanceFromStart;
    public float maxDistanceFromStart;
    public int maxIterations;
    public int enemies;
    public int size;
}
