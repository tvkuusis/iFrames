using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockStopper : MonoBehaviour {

    public float freezeTimer = 2f;
    float timer;

    Rigidbody2D rb;
    float magnitude;
    bool active = true;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        ResetTimer();
	}
	
	void Update () {
        if (active) {
            magnitude = rb.velocity.magnitude;
            //print(magnitude);

            if (magnitude <= 0.01f) {
                timer -= Time.deltaTime;
            }
            else {
                ResetTimer();
            }

            if (timer < 0) {
                FreezeObject();
            }
        }
	}

    void ResetTimer()
    {
        timer = freezeTimer;
    }

    void FreezeObject(){
        rb.isKinematic = true;
        active = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    void UnfreezeObject(){
        ResetTimer();
        GetComponent<BoxCollider2D>().enabled = false;
        rb.isKinematic = false;
        active = true;
    }

    //public override void Reset()
    //{
    //    print("unfreezing");
    //    UnfreezeObject();
    //}
}
