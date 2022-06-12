using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Controller2DPlayer))]
public class PlayerScript : ResetObject
{

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;
    public float moveSpeed = 8;
    public float boostedSpeed = 10;
    public float carrySpeed = 4;
    float currentSpeed;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public Vector2 knockBackForce;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2DPlayer playerController;

    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;

    SpriteRenderer rend;
    bool facingLeft;
    bool lastFrameFacingLeft;

    bool invincible;
    bool invincibleToSpikes;
    bool invincibilityDisabled;
    bool carrying;
    GameObject carriedObject;

    Animator anim;
    PlayerInput pi;
    public float invincibilityTime = 1.5f;
    float invincibilityTimer = 0;
    public Sprite[] sprites;
    Sprite idleSprite;
    Sprite hurtSprite;
    Sprite carrySprite;

    GameControllerScript gc;

    [HideInInspector]
    public Vector2 origPos;
    Quaternion origRot;

    public int invincibilityFrameCount = 5;
    public float jumpInAirTime = 0.2f;
    int iFramesLeft;
    bool levelEnded;

    string prevAnim;
    public GameObject playerBlock;
    public List<GameObject> blocks = new List<GameObject>();
    public int maxNumberOfBlocks = 5;
    public GameObject smoke;
    public GameObject sweatParticles;
    public GameObject speedboostParticles;
    ParticleSystem.EmissionModule speedboostParticleEmission;
    bool speedboostParticlesEnabled = false;
    public float speedBoostParticleRate = 10f;
    bool speedBoosted;
    int speedBoostDir;

    float timeSinceGroundTouch = 0;
    bool jumpedInAir;
    Camera cam;

    void Start()
    {
        playerController = GetComponent<Controller2DPlayer>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        pi = GetComponent<PlayerInput>();
        gc = GameObject.Find("GameController").GetComponent<GameControllerScript>();
        cam = Camera.main;

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        origPos = transform.position;
        origRot = transform.rotation;
        prevAnim = "player_idle";
        currentSpeed = moveSpeed;
        sweatParticles.SetActive(false);
        speedboostParticleEmission = speedboostParticles.GetComponent<ParticleSystem>().emission;
        speedboostParticleEmission.rateOverTime = 0f;
        speedboostParticlesEnabled = false;
    }

    void Update()
    {
        if(speedBoosted) { // Disable speedboost if player doesn't keep moving towards the boosted direction or if the player hits a wall.
            if (directionalInput.x != speedBoostDir) {
                DisableSpeedBoost();
            }
            if ((speedBoostDir == -1 && playerController.collisions.left) || (speedBoostDir == 1 && playerController.collisions.right)) {
                DisableSpeedBoost();
            }
        }

        CalculateVelocity();
        HandleWallSliding();

        // Small leeway to jumping so that the player can still jump a few frames after leaving a platform.
        if (playerController.collisions.below) {
            timeSinceGroundTouch = 0;
            jumpedInAir = false;
        } else {
            timeSinceGroundTouch += Time.deltaTime;
        }

        // Invincibility after hitting an enemy
        if (invincible) {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer < 0f) {
                invincible = false;
                invincibilityDisabled = true;
                iFramesLeft = invincibilityFrameCount;
                if (!levelEnded) { // Otherwise the previous animation overrides the finish animation
                    ReturnToPreviousAnimation();
                }
            }
        }

        playerController.Move(velocity * Time.deltaTime, directionalInput);

        // Handle sliding down a steep slope.
        if (playerController.collisions.above || playerController.collisions.below) {
            if (playerController.collisions.slidingDownMaxSlope) {
                velocity.y += playerController.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else {
                velocity.y = 0;
            }
        }

        if (playerController.lethal) {
            DestroyPlayer(false);
        }

        // Enable particle effect during speedboost while touching the ground.
        if (speedBoosted) {
            if (playerController.collisions.below && !speedboostParticlesEnabled) {
                speedboostParticleEmission.rateOverTime = speedBoostParticleRate;
                speedboostParticlesEnabled = true;
            } else if(!playerController.collisions.below && speedboostParticlesEnabled) {
                speedboostParticleEmission.rateOverTime = 0f;
                speedboostParticlesEnabled = false;
            }
        }
    }

    private void FixedUpdate()
    {
        // This code ensures that when the player loses invincibility while touching both an enemy and a spike at the same time, the player hits the enemy first and becomes invincible again instead of hitting the spike and dying immediately.
        if (invincibilityDisabled) {
            iFramesLeft--;
            if (iFramesLeft < 0) {
                invincibilityDisabled = false;
                invincibleToSpikes = false;
                iFramesLeft = invincibilityFrameCount;
            }
        }
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
        CheckPlayerFaceDirection(input.x);
    }

    void CheckPlayerFaceDirection(float x){
        // Switch sprite facing direction if player turns
        if (x != 0) {
            if (x < 0 && !lastFrameFacingLeft) {
                // Turn player left
                rend.flipX = true;
                lastFrameFacingLeft = true;
            }
            else if (x > 0 && lastFrameFacingLeft) {
                // Turn player right
                rend.flipX = false;
                lastFrameFacingLeft = false;
            }
        }
    }

    // Not implemented (yet)
    //IEnumerator TakeMultipleHits()
    //{
    //    rend.color = Color.green;
    //    PlayerHit(1);
    //    yield return new WaitForSeconds(3);
    //    PlayerHit(1);
    //    yield return new WaitForSeconds(3);
    //    PlayerHit(1);
    //}

    public void PlayerHit(int amount){
        if (!invincible && !levelEnded) {
            //print("Current health: " + gc.currentHealth + ", damage: " + amount);
            invincible = true;
            invincibleToSpikes = true;
            invincibilityDisabled = false;
            invincibilityTimer = invincibilityTime;
            if(gc.currentHealth > amount) { // Knockback in case the player doesn't die to the hit
                KnockBack(knockBackForce);
                anim.Play("Player_hit");
            }
            gc.ReduceHealth(amount);
            rend.color = Color.white;
            DisableSpeedBoost();
        }
    }

    public void DestroyPlayer(bool instaDeath){ // Instadeath: true only if the player should die regardless of current invincibilty (e.g. falling into a pit)
        if((!invincibleToSpikes || instaDeath) && !levelEnded) {
            playerController.lethal = false;

            // Spawn a 'dead' player
            var newBlock = Instantiate(playerBlock, transform.position, transform.rotation);
            if (blocks.Count >= maxNumberOfBlocks) {
                Destroy(blocks[0]);
                blocks.RemoveAt(0);
            }
            blocks.Add(newBlock);
            var playerPoof = Instantiate(smoke, transform.position, Quaternion.identity);
            Destroy(playerPoof, 1f);
            gameObject.SetActive(false);
            gc.ResetLevel();
        }
    }

    public void KnockBack(Vector2 force){
        pi.DisablePlayerInput(0.2f);
        velocity.y = force.y;
    }



    public void OnJumpInputDown()
    {
        // Handle different wallsliding jumps
        if (wallSliding) {
            if (wallDirX == directionalInput.x) { // Wallclimb
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0) { // Drop off
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else { // Wall leap (enables speedboost)
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                    EnableSpeedBoost(-wallDirX);
                }
            }
        if (playerController.collisions.below || (timeSinceGroundTouch <= jumpInAirTime && !jumpedInAir)) {
            if (playerController.collisions.slidingDownMaxSlope) {
                if (directionalInput.x != -Mathf.Sign(playerController.collisions.slopeNormal.x)) { // not jumping against max slope
                    velocity.y = maxJumpVelocity * playerController.collisions.slopeNormal.y;
                    velocity.x = maxJumpVelocity * playerController.collisions.slopeNormal.x;
                }
            }
            else {
                velocity.y = maxJumpVelocity;
            }
                if (!jumpedInAir) {
                jumpedInAir = true;
                }
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity) {
            velocity.y = minJumpVelocity;
        }
    }


    void HandleWallSliding()
    {
        wallDirX = (playerController.collisions.left) ? -1 : 1;
        wallSliding = false;

        if (directionalInput.y > -0.5f && (playerController.collisions.left || playerController.collisions.right) && !playerController.collisions.below && velocity.y != 0) {
            wallSliding = true;

            if (velocity.y < 0) {
                if (velocity.y < -wallSlideSpeedMax) {
                    velocity.y = -wallSlideSpeedMax;
                }

                if (timeToWallUnstick > 0) {
                    velocityXSmoothing = 0;
                    velocity.x = 0;
                    //print("Dir input x: " + directionalInput.x + ", walldirx: " + wallDirX + ", unstick " + timeToWallUnstick + " Collision left: " + playerController.collisions.left);
                    if (directionalInput.x != wallDirX && directionalInput.x != 0) {
                        velocity.y = -wallSlideSpeedMax * 0.5f;
                        //timeToWallUnstick -= Time.deltaTime;
                    }
                    else {
                        timeToWallUnstick = wallStickTime;
                    }
                }
                else {
                    timeToWallUnstick = wallStickTime;
                }
            }
        }
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * currentSpeed;
        if(targetVelocityX == 0 && playerController.collisions.below) {
            velocity.x = 0;
        }
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (playerController.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Pick up a pickable object if the player is not carrying anything yet.
        if (col.CompareTag("Pickable") && !carrying) {
            PickUp(col.gameObject);
            carrying = true;
        }
    }

    void PickUp(GameObject thisPickUp){
        if(invincible) {
            prevAnim = "Player_carry";
        }else {
            anim.Play("Player_carry");
            prevAnim = "Player_carry";
        }

        sweatParticles.SetActive(true);
        thisPickUp.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, thisPickUp.transform.position.z) + Vector3.up * 1.2f;
        thisPickUp.transform.localRotation = Quaternion.Euler(0, 0, 180f);
        thisPickUp.transform.parent = transform;
        //var p = thisPickUp.GetComponent<RotateAround>();
        //if (p) {
        //    p.StopRotating();
        //}
        DecreaseSpeed();
    }

    public void RemovePickUp(){
        anim.Play("player_idle");
        prevAnim = "player_idle";
        carrying = false;
        sweatParticles.SetActive(false);
        IncreaseSpeed(); // Return to normal speed
    }

    void DecreaseSpeed(){
        currentSpeed = carrySpeed;
    }

    void IncreaseSpeed(){
        currentSpeed = moveSpeed;
    }

    void ReturnToPreviousAnimation()
    {
        anim.Play(prevAnim);
    }

    public void AddCheckpoint(Vector2 newPosition)
    {
        origPos = newPosition;
        //print("New pos: " + origPos);
    }

    public void FinishLevel(){
        ReturnToPreviousAnimation();
        cam.GetComponent<CameraFollow>().MoveToPlayer();
        pi.DisablePlayerInput();
        anim.Play("Player_finish");
        levelEnded = true;
    }

    void StartFading()
    {
        cam.GetComponent<ScreenTransitionImageEffect>().StartTransition();
    }

    public override void Reset()
    {
        transform.position = origPos;
        transform.rotation = origRot;
        carrying = false;
        carriedObject = null;
        rend.flipX = false;
        SetDirectionalInput(Vector2.zero);
        velocity = Vector2.zero;
        pi.DisablePlayerInput(0.3f);
        lastFrameFacingLeft = false;
        anim.Play("player_idle");
        prevAnim = "player_idle";
        iFramesLeft = 0;
        currentSpeed = moveSpeed;
        invincibilityDisabled = false;
        invincibleToSpikes = false;
        invincible = false;
        sweatParticles.SetActive(false);
        speedboostParticleEmission.rateOverTime = 0f;
        speedboostParticlesEnabled = false;
        //gameObject.SetActive(true);
    }

    void EnableSpeedBoost(int boostDirection)
    {
        if (!carrying) {
            speedBoostDir = boostDirection;
            speedBoosted = true;
            currentSpeed = boostedSpeed;
        }
    }

    void DisableSpeedBoost(){
        speedBoosted = false;
        if (!carrying) {
            currentSpeed = moveSpeed;
        } else {
            currentSpeed = carrySpeed;
        }
        speedboostParticleEmission.rateOverTime = 0f;
        speedboostParticlesEnabled = false;
    }
}