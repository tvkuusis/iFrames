using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootObjects : ResetObject {

    public GameObject whatToShoot;
    public float interval = 1f;
    public float speed = 1f;
    public float destroyTime = 10f;
    public float timeOffset = 0;
    float origTimeOffset;
    float timer;
    public bool activated = true;
    bool origState;
    IEnumerator coroutine;

    SpriteRenderer rend;
    public Sprite[] sprites;
    Sprite fullSprite;
    Sprite emptySprite;

    bool shooting;

    void Start() {
        origTimeOffset = timeOffset;
        fullSprite = sprites[0];
        emptySprite = sprites[1];
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = emptySprite;
        origState = activated;
        //coroutine = StartShooting();
        //if (activated) {
            //Activate();
        //}
	}
	

	void Update () {
        if (Input.GetKeyDown(KeyCode.B)) {
            if (activated) {
                Deactivate();
            } else {
                Activate();
            }
        }

        if(timeOffset > 0 && activated && !shooting) {
            timeOffset -= Time.deltaTime;
            if(timeOffset < 0) {
                shooting = true;
            }
        }else if(timeOffset <= 0 && activated && !shooting) {
            shooting = true;
        }


        if (shooting) {
            timer += Time.deltaTime;
            if(timer > interval - 0.3f) {
                rend.sprite = fullSprite;
            }
            while(timer > interval) {
                var x = Instantiate(whatToShoot, transform.position + transform.up * 0.1f, transform.rotation);
                x.GetComponent<Rigidbody2D>().AddForce(transform.up * speed, ForceMode2D.Impulse);
                Destroy(x, destroyTime);
                timer -= interval;
                rend.sprite = emptySprite;
            }


            //rend.sprite = emptySprite;
            //rend.sprite = fullSprite;
        }
	}

    public void Activate()
    {
        activated = true;
        timer = 0;
        timeOffset = origTimeOffset;
    }

    public void Deactivate()
    {
        activated = false;
        shooting = false;
    }

    //IEnumerator StartShooting(){
    //    while (true) {
    //        var x = Instantiate(whatToShoot, transform.position + transform.up * 0.1f, transform.rotation);
    //        x.GetComponent<Rigidbody2D>().AddForce(transform.up * speed, ForceMode2D.Impulse);
    //        Destroy(x, destroyTime);
    //        rend.sprite = emptySprite;
    //        yield return new WaitForSeconds((interval + timeOffset) * 0.5f);
    //        rend.sprite = fullSprite;
    //        yield return new WaitForSeconds((interval + timeOffset) * 0.5f);
    //    }
    //}

    public override void Reset(){
        //Deactivate();
        if (!origState) {
            activated = origState;
            shooting = false;
            timer = 0;
            timeOffset = origTimeOffset;
            rend.sprite = emptySprite;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
}
