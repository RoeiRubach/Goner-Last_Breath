using UnityEngine;

public enum EnemyTransitionParameters
{
    _isAtThrowingSpot,
    _isAtAttackingPosition,
    _isDead
}

public abstract class EnemyBase : MonoBehaviour
{
    protected Transform _playerPosition;
    [SerializeField] protected float _enemyHealth;
    [SerializeField] protected float _runningSpeed, _turningSpeed;

    [SerializeField] private float _animationTransitionTime;

    protected bool _isAllowToStart;

    protected Animator _enemyAnimator;
    
    protected virtual void Awake()
    {
        _enemyAnimator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        Invoke("ActivateUpdateMethod", _animationTransitionTime);
        _playerPosition = GameObject.FindWithTag("Player").transform;
        transform.LookAt(_playerPosition);
    }

    protected virtual void Update()
    {
        float _distance = Vector3.Distance(transform.localPosition, _playerPosition.position);
        
        if (_distance <= 1.5f)
        {
            _enemyAnimator.SetBool(EnemyTransitionParameters._isAtAttackingPosition.ToString(), true);
            return;
        }

        if (_isAllowToStart)
            transform.localPosition += (transform.forward * (Time.deltaTime * _runningSpeed));

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

    private void ActivateUpdateMethod()
    {
        _isAllowToStart = true;
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
