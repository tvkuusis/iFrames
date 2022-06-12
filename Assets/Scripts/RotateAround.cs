using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : ResetObject {

    public float speed = 1f;
    float t = 0;
    bool rotating = true;


	void Start () {
		
	}
	
	void Update () {
        if (rotating) {
            t = Time.deltaTime;
            transform.Rotate(Vector3.up, t * speed);
        }
	}

    public void StopRotating()
    {
        rotating = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public override void Reset()
    {
        t = 0;
        rotating = true;
    }
}
