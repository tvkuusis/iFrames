using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlayer : MonoBehaviour {

    private void OnCollisionStay2D(Collision2D col){
        //print("Collided with something");
        PlayerScript ps = col.gameObject.GetComponent<PlayerScript>();
        if (ps) {
            //print("Hit player");
            //ps.DestroyPlayer(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //print("Collided with something");
        PlayerScript ps = col.gameObject.GetComponent<PlayerScript>();
        if (ps) {
            //print("Hit player");
            //ps.DestroyPlayer(false);
        }
    }

    private void Update(){
        
    }
}
