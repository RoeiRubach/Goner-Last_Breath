using UnityEngine;

public class EnemyBoss : EnemyBase
{
    [SerializeField] private float _walkingSpeed;

    private bool _isAbleToTakeDamage = true;

    protected override void Start()
    {
        base.Start();
        transform.LookAt(_playerPosition);
    }

    protected override void Update()
    {
        if (_isAllowToStart)
        {
            transform.localPosition += (transform.forward * (Time.deltaTime * _walkingSpeed));
        }
    }

    public override void TakeDamage(float _damageAmount)
    {
        if (_enemyHealth <= (_enemyMaxHealth / 2) &&
            !_enemyAnimator.GetBool(EnemyTransitionParameters._isAtThrowingSpot.ToString()))
        {
            _enemyAnimator.SetBool(EnemyTransitionParameters._isAtThrowingSpot.ToString(), true);
            _isAbleToTakeDamage = false;
            Invoke("SetEnableDamageTrue", 6f);
        }

        if (!_isAbleToTakeDamage)
            return;

        base.TakeDamage(_damageAmount);
    }

    private void SetEnableDamageTrue()
    {
        _isAbleToTakeDamage = true;
    }
}
