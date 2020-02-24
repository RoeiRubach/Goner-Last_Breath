using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private AudioClip _timeEnded;
    

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void LevelHasEnded()
    {
        _audioSource.PlayOneShot(_timeEnded);
        _gameManager.LevelSuccessfullyFinished();
    }
}
