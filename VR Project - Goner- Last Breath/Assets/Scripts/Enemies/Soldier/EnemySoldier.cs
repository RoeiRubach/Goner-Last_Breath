using UnityEngine;

public class EnemySoldier : EnemyBase
{
    [SerializeField] private AudioClip _soldierStep;

    protected override void Start()
    {
        base.Start();
        transform.LookAt(_playerPosition);
    }

    public void PlaySoldierStep()
    {
        _enemyAudioSource.volume = 0.45f;
        _enemyAudioSource.PlayOneShot(_soldierStep);
    }
}
