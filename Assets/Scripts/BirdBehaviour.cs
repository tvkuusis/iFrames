using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class BirdBehaviour : ResetObject
{

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    public float accelerationTimeAirborne = 0;
    public float accelerationTimeGrounded = 0f;
    public float moveSpeed = 1;

    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    bool moving;
    bool jumping;
    bool facingLeft = true;
    Vector2 input;
    SpriteRenderer rend;
    int sign;

    public Sprite[] sprites;
    Sprite idle;
    Sprite lookDown;
    Sprite peck;
    Sprite fly1;
    Sprite fly2;
    Sprite clean1;
    Sprite clean2;

    bool flying;
    float flyTimer = 0.08f;
    float flyTimerSwitch;
    float flyX = 0;
    float flyY = 0;
    float flyXdecel;
    float flyYaccel;
    float startSpeedX;
    int i;

    public float scareDistance = 3.5f;
    GameObject player;
    bool animationStarted;
    Vector2 origPos;

    void Start()
    {
        player = GameObject.Find("Player");
        controller = GetComponent<Controller2D>();
        rend = GetComponent<SpriteRenderer>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);

        clean1 = sprites[0];
        clean2 = sprites[1];
        fly1 = sprites[2];
        fly2 = sprites[3];
        lookDown = sprites[4];
        peck = sprites[5];
        idle = sprites[6];
        origPos = transform.position;

        rend.sprite = fly1;
        //DoRandomAction();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F)) {
        //    FlyAway();
        //}

        if (!flying) {

            var distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < scareDistance) {
                FlyAway((int)Mathf.Sign(transform.position.x - player.transform.position.x) * 1);
            }

            if (controller.collisions.above || controller.collisions.below) {
                velocity.y = 0;
            }

            if (controller.collisions.below && moving) { // Jump while moving around
                velocity.y = jumpVelocity;
            }

            //input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


            if (controller.collisions.left && input.x < 0) { // Turn the creature if it hits a wall while moving
                input.x = 1;
                //print("Collided left");
            }
            else if (controller.collisions.right & input.x > 0) {
                input.x = -1;
                //print("Collided right");
            }

            float targetVelocityX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

            if (velocity.x > 0 && facingLeft) {
                rend.flipX = true;
                facingLeft = false;
            }
            else if (velocity.x < 0 && !facingLeft) {
                rend.flipX = false;
                facingLeft = true;
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

        }
        else if (flying) {
            var t = Time.deltaTime;
            flyX -= t * flyXdecel;
            flyY += t * flyYaccel;
            if (flyX < 0) flyX = 0;
            transform.Translate(new Vector3(startSpeedX * i - flyX * i, flyY, 0) * t);
            flyTimerSwitch -= t;
            if (flyTimerSwitch < 0) {
                rend.sprite = rend.sprite == fly1 ? fly2 : fly1;
                flyTimerSwitch = flyTimer;
            }
        }

        if (controller.collisions.below && !animationStarted) {
            animationStarted = true;
            rend.sprite = idle;
            DoRandomAction();
        }
    }

    void DoRandomAction()
    {
        int action = (int)Random.Range(0, 3);
        rend.sprite = idle;

        if (action == 0) { // Move left or right
            rend.sprite = idle;
            StartCoroutine(HopAround());
        }
        else if (action == 1) { // Peck ground
            moving = false;
            jumping = false;
            StartCoroutine(PeckGround());
        }
        else if (action == 2) { // Clean feathers
            moving = false;
            jumping = false;
            StartCoroutine(CleanFeathers());
        }
    }

    int RandomSign()
    {
        return Random.value < .5 ? 1 : -1;
    }

    IEnumerator HopAround()
    {
        moving = true;
        jumping = true;
        input.x = RandomSign();
        //rend.sprite = lookDown;
        var t = Random.Range(0.5f, 2f);
        yield return new WaitForSeconds(t);
        input.x = 0;
        moving = false;
        t = Random.Range(0.5f, 1f);
        //rend.sprite = idle;
        yield return new WaitForSeconds(1f);
        DoRandomAction();
    }

    IEnumerator PeckGround()
    {
        rend.sprite = lookDown;
        var pecks = Random.Range(1, 6);
        for (int i = 0; i <= pecks; i++) {
            var t = Random.Range(0.1f, 0.6f);
            yield return new WaitForSeconds(t);
            rend.sprite = peck;
            yield return new WaitForSeconds(0.15f);
            rend.sprite = lookDown;
            yield return new WaitForSeconds(0.2f);
        }
        rend.sprite = idle;
        yield return new WaitForSeconds(1f);
        DoRandomAction();
    }

    IEnumerator CleanFeathers()
    {
        rend.sprite = lookDown;
        var cleancycles = Random.Range(1, 6);
        for (int i = 0; i <= cleancycles; i++) {
            yield return new WaitForSeconds(0.1f);
            rend.sprite = clean1;
            yield return new WaitForSeconds(0.2f);
            rend.sprite = clean2;
            yield return new WaitForSeconds(0.2f);
        }
        rend.sprite = lookDown;
        yield return new WaitForSeconds(0.1f);
        rend.sprite = idle;
        yield return new WaitForSeconds(0.5f);
        DoRandomAction();
    }

    public void FlyAway(int direction)
    {
        StopAllCoroutines();
        flying = true;
        rend.sprite = fly1;
        flyTimerSwitch = flyTimer;
        flyXdecel = Random.Range(1f, 3f);
        flyYaccel = Random.Range(6f, 8f);
        startSpeedX = Random.Range(8f, 12f);
        i = direction;
        rend.flipX = direction < 0 ? false : true;
    }

    public override void Reset()
    {
        StopAllCoroutines();
        flying = false;
        transform.position = origPos;
        flyX = 0;
        flyY = 0;
        input = Vector3.zero;
        velocity = Vector3.zero;
        rend.sprite = idle;
        animationStarted = false;
        rend.sprite = fly1;
        facingLeft = true;
        rend.flipX = false;
    }
}