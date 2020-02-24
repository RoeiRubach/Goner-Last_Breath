using UnityEngine;

public class EnemySoldier : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        transform.LookAt(_playerPosition);
    }
}
