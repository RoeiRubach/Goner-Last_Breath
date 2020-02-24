using UnityEngine;

public enum GameLevels
{
    FirstLevel = 1,
    SecondLevel,
    GameEnded
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _leftHandRef;
    [SerializeField] private GameObject _clockTimerRef, _soldierSpawnersRef, _commanderSpawnersRef, bossSpawnerRef;
    [SerializeField] private GameObject _UMP45Ref, _uziRef;
    [SerializeField] private SceneController _sceneController;

    private GameLevels _gameLevels = GameLevels.FirstLevel;

    private void Start()
    {
        Invoke("StartFirstLevel", 6f);
    }

    public void StartFirstLevel()
    {
        SetTimerON();

        if (!_uziRef)
            Instantiate(_uziRef);

        _clockTimerRef.GetComponent<Animator>().speed = 0.5f;
        _soldierSpawnersRef.SetActive(true);
    }

    public void StartSecondLevel()
    {
        SetTimerON();

        if (!_UMP45Ref)
            Instantiate(_UMP45Ref);

        _clockTimerRef.GetComponent<Animator>().speed = 0.25f;
        _soldierSpawnersRef.SetActive(true);
        _commanderSpawnersRef.SetActive(true);
    }

    public void StartThirdLevel()
    {

    }

    public void RestartLevel()
    {
        SceneController.LoadScene(_buildIndex:1);
    }

    public void LevelSuccessfullyFinished()
    {
        SetTimerOFF();
        _soldierSpawnersRef.SetActive(false);
        EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();

        foreach (EnemyBase enemiesToKill in enemies)
        {
            enemiesToKill.EnemyBeenKilled();
        }

        _gameLevels++;

        switch (_gameLevels)
        {
            case GameLevels.SecondLevel:
                Invoke("StartSecondLevel", 6f);
                break;
            case GameLevels.GameEnded:
                SceneController.LoadScene();
                break;
        }
    }

    public Transform GetLeftHandRef()
    {
        return _leftHandRef;
    }

    private void SetTimerON()
    {
        _clockTimerRef.GetComponentInChildren<Light>().enabled = true;
        _clockTimerRef.GetComponent<TimerManager>().enabled = true;
        _clockTimerRef.GetComponent<Animator>().enabled = true;
    }

    private void SetTimerOFF()
    {
        _clockTimerRef.GetComponent<TimerManager>().enabled = false;
        _clockTimerRef.GetComponent<Animator>().enabled = false;
    }
}
