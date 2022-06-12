using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {
    public float xSpeed;
    public float ySpeed;
    Renderer rend;
    Vector2 oldPos;
    Vector2 newPos;
    Vector2 orig;
    Camera cam;

    void Start() {
        rend = GetComponent<Renderer>();
        cam = Camera.main;
        orig = rend.material.mainTextureOffset;
        rend.sortingLayerName = "Background";
    }

    void Update() {
        newPos = cam.transform.position;
        float x = Mathf.Repeat(newPos.x * xSpeed, 1f);
        float y = Mathf.Repeat(newPos.y * ySpeed, 1f);
        Vector2 offset = new Vector2(x, y);
        rend.material.mainTextureOffset = orig - offset;
    }
}
