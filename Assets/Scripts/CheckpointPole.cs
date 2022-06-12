using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointPole : MonoBehaviour
{

    public GameObject flagResting;
    public GameObject flagWaving;
    public Transform checkpointPosition;
    Vector2 thisPosition;
    public GameObject particles;
    PlayerScript ps;

    void Start(){
        thisPosition = new Vector2(checkpointPosition.position.x, checkpointPosition.position.y);
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();
        flagResting.SetActive(true);
        flagWaving.SetActive(false);
    }

    void Update(){

    }

    private void OnTriggerEnter2D(Collider2D player){
        //GetComponent<BoxCollider2D>().enabled = false;
        flagResting.SetActive(false);
        flagWaving.SetActive(true);
        if (ps.origPos != thisPosition) { // Don't activate same checkpoint twice in a row
            ps.AddCheckpoint(checkpointPosition.position);
            var p = Instantiate(particles, transform.position + Vector3.up * 2, Quaternion.identity);
            Destroy(p, 3f);
        }
    }
}

