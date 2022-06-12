using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundObject : MonoBehaviour {

    public float moveSpeed;

    public GameObject cam;
    Vector2 origpos;

	void Start () {
        origpos = new Vector2(transform.position.x, transform.position.y);
	}
	
	void Update () {
        transform.position = new Vector2(origpos.x + cam.transform.position.x, origpos.y + cam.transform.position.y);
	}
}
