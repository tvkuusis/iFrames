using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRefill : MonoBehaviour {

    GameControllerScript gc;
    bool active = true;
    Animator anim;


	void Start () {
        anim = GetComponent<Animator>();
        gc = GameObject.Find("GameController").GetComponent<GameControllerScript>();
	}
	
	void Update () {

	}

    private void OnTriggerEnter2D(Collider2D col){
        if (gc) {
            if (gc.currentHealth < gc.maxHealth) {
                gc.RefillHealth();
                anim.Play("Heart_refill");
                Disable();
            }
        }
    }

    void Disable(){
        GetComponent<PolygonCollider2D>().enabled = false;
        active = false;
    }

    void Enable(){
        GetComponent<PolygonCollider2D>().enabled = true;
        active = true;
    }
}