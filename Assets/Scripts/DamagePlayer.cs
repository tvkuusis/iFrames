using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour {

    public int damageDone = 1;

    private void OnTriggerStay2D(Collider2D col){
        //print("Hit stay");
        PlayerScript ps = col.GetComponent<PlayerScript>();
        if (ps) {
            //print("Hit player");
            ps.PlayerHit(damageDone);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //print("Hit enter");
        PlayerScript ps = col.GetComponent<PlayerScript>();
        if (ps) {
            //print("Hit player");
            ps.PlayerHit(damageDone);
        }
    }
}
