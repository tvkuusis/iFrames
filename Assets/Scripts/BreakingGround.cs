using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingGround : ResetObject
{

    public float breakTime;
    float timer;
    float interval;
    int i = 0;

    public Sprite[] sprites;
    public GameObject dust;

    SpriteRenderer rend;
    bool breaking;

    void Start(){
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = sprites[0];
        interval = breakTime / sprites.Length;
    }


    void Update()
    {
        if (breaking) {
            timer += Time.deltaTime;
            if(timer > interval) {
                i++;
                if (i >= sprites.Length) {
                    var d = Instantiate(dust, transform.position, Quaternion.identity);
                    Destroy(d, 1);
                    breaking = false;
                    rend.enabled = false;
                    GetComponent<BoxCollider2D>().enabled = false;
                }
                else {
                    rend.sprite = sprites[i];
                    timer = 0;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        StartBreaking();
    }

    void StartBreaking()
    {
        breaking = true;
    }

    public override void Reset()
    {
        ResetObject();
    }

    void ResetObject()
    {
        timer = 0;
        i = 0;
        GetComponent<BoxCollider2D>().enabled = true;
        rend.enabled = true;
        breaking = false;
        rend.sprite = sprites[0];
    }
}
