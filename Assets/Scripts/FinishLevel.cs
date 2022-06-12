using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour {

    PlayerScript player;    

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player").GetComponent<PlayerScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D col){
        GetComponent<BoxCollider2D>().enabled = false;
        player.FinishLevel();
    }
}
