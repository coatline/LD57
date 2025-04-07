using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] PlayerController playerPrefab;
    [SerializeField] LevelGenerator _levelGenerator;

    public PlayerController PlayerController { get; private set; }
    bool levelCompleted;

    protected override void Awake()
    {
        base.Awake();

        _levelGenerator.GenerateLevel(GameData.I.GetCurrentLevel());

        Vector3 spawnPosition = _levelGenerator.Wellivator.GetBucketPosition() + new Vector3(0, 0.5f, 0);

        PlayerController = Instantiate(playerPrefab);
        PlayerController.transform.GetChild(0).position = spawnPosition;
    }

    private void Start()
    {
        PlayerController.GetComponent<CutscenePlayer>().PlayCutscene(_levelGenerator.Wellivator.GetComponent<LevelIntroCutscene>());
        PlayerController.transform.GetChild(0).rotation = Quaternion.Euler(0, _levelGenerator.Wellivator.transform.eulerAngles.y + 90, 0);
    }

    public void GrabbedGemStone()
    {
        foreach (Light torch in FindObjectsByType<Light>(FindObjectsSortMode.None))
        {
            torch.intensity *= .35f;
        }
    }

    public void CompleteLevel()
    {
        if (levelCompleted)
            return;

        levelCompleted = true;
        GameData.I.LevelWon();
    }
}