using UnityEngine;

public class GameData : Singleton<GameData>
{
    [SerializeField] LevelSO[] levels;

    int level;

    public void LevelWon()
    {
        level++;

        if (level == 3)
        {
            SceneFader.I.LoadNewScene("Game Over");
            GameOver();
        }
        else
            SceneFader.I.LoadNewScene("Game");
    }

    public void GameOver()
    {
        level = 0;
    }

    public LevelSO GetCurrentLevel()
    {
        if (level >= levels.Length)
            level = levels.Length - 1;

        return levels[level];
    }
}