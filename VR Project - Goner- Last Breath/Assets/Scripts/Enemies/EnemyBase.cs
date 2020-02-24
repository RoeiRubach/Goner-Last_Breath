using UnityEngine;

public enum EnemyTransitionParameters
{
    _isAtThrowingSpot,
    _isAtAttackingPosition,
    _isDead
}

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float _enemyMaxHealth;
    [SerializeField] protected float _runningSpeed;

    [SerializeField] protected float _animatorOffTime, _updateActivationTime, _distanceToAttack;

    protected Transform _playerPosition;
    protected float _enemyHealth, _distance;
    protected bool _isAllowToStart;

    protected Animator _enemyAnimator;

    private SimpleShoot _simpleShoot;

    protected virtual void Awake()
    {
        _enemyAnimator = GetComponent<Animator>();
        _enemyHealth = _enemyMaxHealth;
    }

    protected virtual void Start()
    {
        _playerPosition = GameObject.FindWithTag("Player").transform;
        Invoke("ActivateUpdateMethod", _updateActivationTime);
        _simpleShoot = FindObjectOfType<SimpleShoot>();
        Debug.Assert(_simpleShoot, "its null buddy");
        _distance = Vector3.Distance(transform.localPosition, _playerPosition.position);
    }

    protected virtual void Update()
    {
        _distance = Vector3.Distance(transform.localPosition, _playerPosition.position);

        if (_distance <= _distanceToAttack)
        {
            GetComponent<AudioSource>().Play();
            _enemyAnimator.SetBool(EnemyTransitionParameters._isAtAttackingPosition.ToString(), true);
            return;
        }

        if (_isAllowToStart && !_enemyAnimator.GetBool(EnemyTransitionParameters._isDead.ToString()))
            transform.localPosition += (transform.forward * (Time.deltaTime * _runningSpeed));

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
            TakeDamage(_simpleShoot._bulletDamage);
    }

    public virtual void TakeDamage(float _damageAmount)
    {
        Debug.Log("Damage EnemyBase "+ _damageAmount);
        _enemyHealth -= _damageAmount;
        
        if (_enemyHealth <= 0f)
            EnemyBeenKilled();
    }

    public virtual void EnemyBeenKilled()
    {
        GetComponent<BoxCollider>().enabled = false;
        _enemyAnimator.SetBool(EnemyTransitionParameters._isDead.ToString(), true);
        Invoke("SetAnimatorOffAndDestroy", _animatorOffTime);
    }

    private void ActivateUpdateMethod()
    {
        _isAllowToStart = true;
    }

    private void SetAnimatorOffAndDestroy()
    {
        _enemyAnimator.enabled = false;

        Destroy(gameObject);
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
