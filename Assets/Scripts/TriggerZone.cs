using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : ResetObject {

    public bool disableOnEnter = true;
    public UnityEvent onTrigger;
    public UnityEvent onExit;

    bool disabled;

	void Start () {
		
	}
	
	void LateUpdate () {
        disabled = false;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!disabled) {
            onTrigger.Invoke();
            if (disableOnEnter) {
                GetComponent<BoxCollider2D>().enabled = false;
            }
            disabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        onExit.Invoke();
    }

    public override void Reset()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
