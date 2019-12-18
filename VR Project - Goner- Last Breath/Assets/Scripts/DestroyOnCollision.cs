﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    private float counter = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        counter -= Time.deltaTime;

        if (counter <= 0)
        {
            counter = 10f;
            Destroy(gameObject);
        }
    }
}