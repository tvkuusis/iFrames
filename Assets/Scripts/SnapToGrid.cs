#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

// This script is executed in the editor
[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour
{
    public bool snapToGrid = true;
    public float snapValue = 0.5f;

    //public bool sizeToGrid = false;
    //public float sizeValue = 1f;

    public bool rotateY;

    // Adjust size and position
    void Update()
    {
        if (snapToGrid) {
            transform.position = RoundTransform(transform.position, snapValue);
        }

        //if (sizeToGrid) {
        //    transform.localScale = RoundTransform(transform.localScale, sizeValue);
        //}
    }
    public void Rot()
    {
        if (rotateY) {
            transform.Rotate(Vector3.up, 90f);
        }
        else {
            transform.Rotate(Vector3.forward, 90f);
        }
    }
    // The snapping code
    private Vector3 RoundTransform(Vector3 v, float snapValue)
    {
        return new Vector3
        (
            snapValue * Mathf.Round(v.x / snapValue),
            snapValue * Mathf.Round(v.y / snapValue),
            snapValue * Mathf.Round(v.z / snapValue)
        //v.z
        );
    }
}
#endif
