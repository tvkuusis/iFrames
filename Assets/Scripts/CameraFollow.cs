using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : ResetObject {

    public Controller2DPlayer target;
    public float verticalOffset;
    float origVertOffset;

    public float lookAheadDstX;
    public float lookSmoothTimeX;
    public float verticalSmoothTime;
    public Vector2 focusAreaSize;
    float origFocusVert;

    FocusArea focusArea;
    float currentLookAheadX;
    float targetLookAheadX;
    float lookAheadDirX;
    float smoothLookVelocityX;
    float smoothVelocityY;

    bool lookAheadStopped;

    float origLookAheadX;
    float origLookAheadDir;

    float timer;
    public float fadeoutSpeed = 0.6f;
    Camera cam;
    float origSize;
    public float zoomSize = 3f;
    bool zooming;
    bool following = true;
    bool movingToPlayer;
    float moveTime = 0.5f;
    float t = 0;

    private void Start(){
        cam = Camera.main;
        origSize = cam.orthographicSize;
        focusArea = new FocusArea(target.collider.bounds, focusAreaSize);
        origLookAheadX = currentLookAheadX;
        origLookAheadDir = lookAheadDirX;
        origVertOffset = verticalOffset;
        origFocusVert = focusAreaSize.y;
    }

    private void LateUpdate(){
        if (following) {

            focusArea.Update(target.collider.bounds);

            Vector2 focusPosition = focusArea.centre + Vector2.up * verticalOffset;

            if (focusArea.velocity.x != 0) {
                lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
                if (Mathf.Sign(target.playerInput.x) == Mathf.Sign(focusArea.velocity.x) && target.playerInput.x != 0) {
                    lookAheadStopped = false;
                    targetLookAheadX = lookAheadDirX * lookAheadDstX;
                } else {
                    if (!lookAheadStopped) {
                        lookAheadStopped = true;
                        targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX) / 4f;
                    }
                }
            }

            currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

            focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);

            focusPosition += Vector2.right * currentLookAheadX;
            transform.position = (Vector3)focusPosition + Vector3.forward * -10;
        }else if (movingToPlayer) {
            t += Time.deltaTime;
            if(t > moveTime) {
                t = moveTime;
            }
            float percentage = t / moveTime;
            Vector2 newPos = Vector2.Lerp(transform.position, target.transform.position, percentage);
            transform.position = newPos;
            transform.position += Vector3.forward * -10;
            if (percentage == 1) {
                movingToPlayer = false;
                ZoomAtPlayer();
            }
        }else if (zooming) {
            timer += Time.deltaTime * fadeoutSpeed;
            cam.orthographicSize = Mathf.Lerp(origSize, zoomSize, timer);
            if (timer > 1) {
                timer = 0;
                zooming = false;
                this.enabled = false;
            }
        }
    }

    private void OnDrawGizmos(){
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(focusArea.centre, focusAreaSize);
    }

    struct FocusArea{
        public Vector2 centre;
        public Vector2 velocity;
        float left, right;
        float top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds){
            float shiftX = 0;
            if(targetBounds.min.x < left) {
                shiftX = targetBounds.min.x - left;
            }else if(targetBounds.max.x > right) {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom) {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top) {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }

    public override void Reset(){
        lookAheadDirX = origLookAheadDir;
        currentLookAheadX = origLookAheadX;
        ResetVerticalOffset();
    }

    public void ZoomAtPlayer(){
        zooming = true;
    }

    public void ChangeVerticalOffset(float offset) {
        verticalOffset = offset;
    }

    public void ResetVerticalOffset() {
        verticalOffset = origVertOffset;
    }

    public void MoveToPlayer(){
        following = false;
        zooming = false;
        movingToPlayer = true;
    }
}
