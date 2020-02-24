using UnityEngine;
using System.Collections.Generic;

public class EnemyCommander : EnemyBase
{
    [SerializeField] private AudioClip _soldierStep;
    [SerializeField] private List<Rigidbody> _ragdollKinematic = new List<Rigidbody>();

    protected override void Start()
    {
        base.Start();
        SetRagdollOFF();

        InvokeRepeating("PlayCommanderStep", 2f, 10f);
    }

    private void SetRagdollOFF()
    {
        Rigidbody[] rigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody c in rigidbodies)
        {
            _ragdollKinematic.Add(c);
            c.isKinematic = true;
        }
    }

    public void SetRagdollON()
    {
        _enemyAnimator.enabled = false;

        foreach (Rigidbody c in _ragdollKinematic)
        {
            c.isKinematic = false;
        }
    }

    protected override void FixedUpdate()
    {
        if (_distance <= _distanceToAttack)
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            transform.LookAt(_playerPosition);
            _distanceToAttack = 1.7f;
        }

        base.FixedUpdate();
    }

    public override void EnemyBeenKilled()
    {
        SetRagdollON();
        base.EnemyBeenKilled();
    }

    public void PlayCommanderStep()
    {
        _enemyAudioSource.volume = 0.45f;
        _enemyAudioSource.PlayOneShot(_soldierStep);
    }
}
