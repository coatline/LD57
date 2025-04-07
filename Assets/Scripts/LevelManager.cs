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

        Vector3 wellivatorEuler = _levelGenerator.Wellivator.transform.eulerAngles;


        //PlayerController.transform.GetChild(0).rotation = Quaternion.LookRotation(_levelGenerator.Wellivator.transform.position + _levelGenerator.Wellivator.transform.forward, Vector3.up);
        //PlayerController.transform.GetChild(0).rotation = Quaternion.Euler(0, PlayerController.transform.GetChild(0).eulerAngles.y, 0);
        PlayerController.transform.GetChild(0).LookAt(new Vector3(_levelGenerator.Wellivator.transform.position.x, transform.position.y, _levelGenerator.Wellivator.transform.position.z) + _levelGenerator.Wellivator.transform.forward);


        print($"Well y = {_levelGenerator.Wellivator.transform.eulerAngles.y}, player y is now = {PlayerController.transform.GetChild(0).eulerAngles.y}");
    }

    private void Start()
    {
        PlayerController.GetComponent<CutscenePlayer>().PlayCutscene(_levelGenerator.Wellivator.GetComponent<LevelIntroCutscene>());
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