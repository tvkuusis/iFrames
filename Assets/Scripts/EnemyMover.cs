using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : ResetObject {

    public Vector3[] localWaypoints;
    Vector3[] globalWaypoints;

    public float speed;
    public bool cyclic;
    public float waitTime;
    [Range(0, 2)]
    public float easeAmount;

    int fromWaypointIndex;
    int toWaypointIndex;
    float percentBetweenWaypoints;
    float nextMoveTime;
    float distanceBetweenWaypoints;
    bool lastFramefacingLeft;
    bool origFacingLeft;
    Vector2 origPos;
    Vector3 newPos;
    SpriteRenderer rend;
    Vector3[] origWaypoints;
    Vector3[] resetWaypoints;
    float timeAtReset = 0;

    public bool moving = true;
    bool movingOrig;

    void Start(){
        rend = GetComponentInChildren<SpriteRenderer>();
        origPos = transform.position;
        movingOrig = moving;
        Initialize();
    }

    void Initialize() {
        transform.position = origPos;
        globalWaypoints = new Vector3[localWaypoints.Length];
        origWaypoints = globalWaypoints;
        if (localWaypoints.Length > 1) {
            SetupWaypoints();
            origFacingLeft = lastFramefacingLeft;
        } else {
            moving = false;
        }
        resetWaypoints = globalWaypoints;
        CheckFacing();

    }

    void SetupWaypoints()
    {
        for (int i = 0; i < localWaypoints.Length; i++) {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
    }

    void Update() {
        if (moving) {
            Vector3 velocity = CalculateEnemyMovement();
            transform.Translate(velocity);
        }
    }

    float Ease(float x){
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    Vector3 CalculateEnemyMovement(){

        if ((Time.time - timeAtReset) < nextMoveTime) {
            return Vector3.zero;
        }

        fromWaypointIndex %= globalWaypoints.Length;
        toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

        newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

        if (percentBetweenWaypoints >= 1) {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;
            if (!cyclic) {
                if (fromWaypointIndex >= globalWaypoints.Length - 1) {
                    fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }
            CheckFacing();
            RotateSprite(globalWaypoints[fromWaypointIndex + 1] - globalWaypoints[fromWaypointIndex]);
            nextMoveTime = Time.time - timeAtReset + waitTime;
        }
        return newPos - transform.position;
    }

    void RotateSprite(Vector2 velocity)
    {
        //print(velocity);
        if (velocity.x < 0 && !lastFramefacingLeft) {
            //rend.flipX = false;
            transform.localScale = new Vector2(1, 1);

            lastFramefacingLeft = true;
        }
        else if (velocity.x > 0 && lastFramefacingLeft) {
            //rend.flipX = true;
            lastFramefacingLeft = false;
            transform.localScale = new Vector2(-1, 1);
        }
    }

    private void OnDrawGizmos(){
        if (localWaypoints != null) {
            Gizmos.color = Color.red;
            float size = .3f;

            for (int i = 0; i < localWaypoints.Length; i++) {
                Vector3 globalWaypointPos = Application.isPlaying ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }

    public void StartMoving()
    {
        moving = true;
    }

    void CheckFacing(){ // Flipx = false -> enemy facing left
        if (localWaypoints.Length > 1) {
            if (transform.position.x > globalWaypoints[1].x) {
                //rend.flipX = false;
                transform.localScale = new Vector2(1, 1);
                lastFramefacingLeft = true;
            } else {
                //rend.flipX = true;
                transform.localScale = new Vector2(-1, 1);

                lastFramefacingLeft = false;
            }
        }
    }

    public override void Reset()
    {
        moving = movingOrig;
        Initialize();
        if (localWaypoints.Length > 1) {
            globalWaypoints = origWaypoints;
            //origWaypoints = globalWaypoints;
            //if (globalWaypoints.Length > 1) {
            //    SetupWaypoints();
            //}
            percentBetweenWaypoints = 0;
            fromWaypointIndex = 0;
            //rend.flipX = false;
            transform.localScale = new Vector2(1, 1);
            //lastFramefacingLeft = origFacingLeft;
            CheckFacing();
            toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
            distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
            nextMoveTime = 0;
            lastFramefacingLeft = false;
            timeAtReset = Time.time;
        }
    }
}
