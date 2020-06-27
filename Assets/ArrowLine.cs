using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLine : MonoBehaviour
{
    public LineRenderer LineRenderer;

    public Transform endLinePoint;

    private void Update()
    {
        LineRenderer.positionCount = 2;
        LineRenderer.SetPosition(1, transform.position);
        LineRenderer.SetPosition(0, endLinePoint.position);
    }
}
