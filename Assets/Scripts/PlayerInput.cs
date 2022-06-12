using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerScript))]
public class PlayerInput : MonoBehaviour {

    PlayerScript player;
    float timer = 0;
    bool finished;
    int direction;
    Vector2 directionalInput;

	void Start () {
        player = GetComponent<PlayerScript>();
	}
	
	void Update () {
        timer -= Time.deltaTime;
        if (timer < 0) {
            directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            player.SetDirectionalInput(directionalInput);

            if (Input.GetButtonDown("Jump")) {
                player.OnJumpInputDown();
            }
            if (Input.GetButtonUp("Jump")) {
                player.OnJumpInputUp();
            }
        }
        else {
            player.SetDirectionalInput(Vector2.zero);
        }

        if (finished) {
            player.SetDirectionalInput(Vector2.right * 0.2f * direction);
        }
    }

    public void DisablePlayerInput(float time = Mathf.Infinity){
        timer = time;
    }

    void MovePlayerAfterFinish(int dir = 1){
        finished = true;
        direction = dir;
    }

    public void MoveWithButton(int dir){
        if(dir == -1) {
            player.SetDirectionalInput(Vector2.left);
        }else if(dir == 1) {
            player.SetDirectionalInput(Vector2.right);
        }
    }

    public void StopMovingButton()
    {
        player.SetDirectionalInput(Vector2.zero);
    }

    public void StartJumpingWithButton(){
        player.OnJumpInputDown();
    }

    public void StopJumpingWithButton()
    {
        player.OnJumpInputUp();
    }
}