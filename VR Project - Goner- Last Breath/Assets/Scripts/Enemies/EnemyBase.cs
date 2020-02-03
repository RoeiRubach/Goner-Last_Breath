using UnityEngine;

public enum EnemyTransitionParameters
{
    _isAtThrowingSpot,
    _isDead
}

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float _enemyHealth;
    [SerializeField] protected float _walkingSpeed, _runningSpeed, _turningSpeed;

    protected Animator _enemyAnimator;

    protected virtual void Awake()
    {
        _enemyAnimator = GetComponent<Animator>();
    }

    public virtual void TakeDamage(float _damageAmount)
    {
        _enemyHealth -= _damageAmount;

        if (_enemyHealth <= 0f)
        {
            EnemyBeenKilled();
        }
    }

    protected virtual void EnemyBeenKilled()
    {
        _enemyAnimator.SetBool(EnemyTransitionParameters._isDead.ToString(), true);

        //_enemyAnimator.enabled = false;

        // Destroy(gameObject);
    }

#if UNITY_EDITOR
    [ContextMenu("Kill this enemy - PLAYMODE ONLY!")]
    public void KillEnemy()
    {
        _enemyAnimator.SetBool(EnemyTransitionParameters._isAtThrowingSpot.ToString(), true);
        EnemyBeenKilled();
    }
#endif
}
