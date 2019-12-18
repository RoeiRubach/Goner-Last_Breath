using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField]
    private Transform playerLocation;

    [SerializeField]
    private float runningSpeed;

    private void Update()
    {
        transform.LookAt(playerLocation);

        transform.position = transform.position + transform.forward * Time.deltaTime * runningSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
