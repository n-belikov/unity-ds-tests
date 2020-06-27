using System;
using System.Collections;
using System.Collections.Generic;
using CryoDI;
using Inputs;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveToPoint : MonoBehaviour
{
    [Dependency] public IInputHandler handler { get; set; }

    public Transform player;

    public Transform[] points;

    public Vector3 offset = Vector3.up;

    public float speed = 2f;


    private bool clicked = false;

    private void Update()
    {
        if (handler.Sprint && !clicked) {
            StartCoroutine(StartMovingCoroutine());
            clicked = true;
        }

        if (!handler.Sprint && clicked) {
            clicked = false;
        }
    }

    public IEnumerator StartMovingCoroutine()
    {
        Vector3 currentPosition = player.position;
        float time = 0;
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerManager>().DisableGravity = true;
        while (true) {
            time += Time.deltaTime / speed;
            if (time < 1f) {
                player.position = Vector3.Lerp(
                    currentPosition,
                    currentPosition + offset,
                    time
                );
            }
            else {
                Vector3 position = points[Random.Range(0, points.Length)].position;
                if (Physics.Raycast(position, Vector3.down, out RaycastHit hit)) {
                    position = hit.point + Vector3.up * 0.1f;
                }

                player.position = position;
                player.GetComponent<CharacterController>().enabled = true;
                player.GetComponent<PlayerManager>().DisableGravity = false;
                yield break;
            }

            yield return null;
        }
    }
}