using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform target;
    public float followDistance = 5f;
    public float followSpeed = 1f;
    private void Update()
    {
        Vector3 targetPosition = target.position + target.up;
        if (Vector3.Distance(transform.position, targetPosition) > followDistance) {
            Vector3 velocity = targetPosition - transform.position;
            velocity *= Time.deltaTime * followSpeed;
            transform.Translate(velocity, Space.World);
        }
    }
}
