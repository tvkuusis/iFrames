using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingLight : MonoBehaviour {

    public float speed;
    public float amplitude;

    MeshRenderer rend;
    Color color;
    float origAlpha;
    float newAlpha;

	// Use this for initialization
	void Start () {
        rend = GetComponent<MeshRenderer>();
        color = rend.material.color;
        origAlpha = color.a;
	}
	
	void Update () {
        newAlpha = Mathf.Sin(Time.time * speed) * amplitude;
        color = new Color(color.r, color.g, color.b, origAlpha + newAlpha);
        rend.material.color = color;
        print(color + " " + origAlpha);
	}
}
