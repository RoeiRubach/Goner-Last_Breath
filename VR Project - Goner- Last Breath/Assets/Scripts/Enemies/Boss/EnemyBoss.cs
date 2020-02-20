using UnityEngine;

public class EnemyBoss : EnemyBase
{
    [SerializeField] private float _walkingSpeed;

    protected override void Update()
    {
        if (_isAllowToStart)
        {
            transform.localPosition += (transform.forward * (Time.deltaTime * _walkingSpeed));
        }
    }
}
