using UnityEngine;

public enum GameLevels
{
    FirstLevel = 1,
    SecondLevel,
    ThirdLevel,
    GameEnded
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _leftHandRef;
    [SerializeField] private GameObject _clockTimerRef, _soldierSpawnersRef, _commanderSpawnersRef, bossSpawnerRef;
    [SerializeField] private SceneController _sceneController;

    private GameLevels _gameLevels = GameLevels.FirstLevel;

    private void Start()
    {
        StartFirstLevel();
    }

    public void StartFirstLevel()
    {

    }

    public void StartSecondLevel()
    {

    }

    public void StartThirdLevel()
    {

    }

    public void LevelSuccessfullyFinished()
    {
        EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();

        foreach (EnemyBase enemiesToKill in enemies)
        {
            enemiesToKill.EnemyBeenKilled();
        }

        _gameLevels++;

        switch (_gameLevels)
        {
            case GameLevels.SecondLevel:
                StartSecondLevel();
                break;
            case GameLevels.ThirdLevel:
                break;
            case GameLevels.GameEnded:
                break;
        }
    }

    public Transform GetLeftHandRef()
    {
        return _leftHandRef;
    }
}
