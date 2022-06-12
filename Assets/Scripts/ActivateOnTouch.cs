using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnTouch : ResetObject
{

    public bool movingAtStart = false;
    bool activated;

    private void Start()
    {
        if (movingAtStart){
            ActivatePlatform();
        }else{
            DeactivatePlatform();
        }
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (!activated) {
            if (c.transform.tag == ("Player")) {
                ActivatePlatform();
            }
        }
    }

    public override void Reset()
    {
        //ActivatePlatform();
        GetComponent<PlatformController>().ResetThis();

        if (movingAtStart){
            ActivatePlatform();
        }else{
            DeactivatePlatform();
        }
    }

    void ActivatePlatform(){
        activated = true;
        GetComponent<PlatformController>().StartMoving();
    }

    void DeactivatePlatform()
    {
        activated = false;
        GetComponent<PlatformController>().StopMoving();
    }
}
