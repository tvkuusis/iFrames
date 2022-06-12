using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : ResetObject {

    public float deathWaitTime;
    [HideInInspector]
    public int maxHealth;
    [HideInInspector]
    public int currentHealth;
    public Image[] healthSprites;
    public Sprite healthFull;
    public Sprite healthEmpty;
    public GameObject player;
    PlayerScript playerScript;
    PlayerInput playerInput;
    public ResetObject[] _ResetScripts;
    Camera cam;

    public CheckpointPole[] checkpoints;
    int nextCheckpoint = 0;

    void Start () {
        maxHealth = healthSprites.Length;
        currentHealth = maxHealth;
        for(int i = 0; i < maxHealth; i++) {
            healthSprites[i].sprite = healthFull;
        }
        cam = Camera.main;
        playerScript = player.GetComponent<PlayerScript>();
        playerInput = player.GetComponent<PlayerInput>();
        _ResetScripts = FindObjectsOfType(typeof(ResetObject)) as ResetObject[];
        //checkpoints = FindObjectsOfType(typeof(CheckpointPole)) as CheckpointPole[];
    }

    void Update () {
        if (Input.GetButtonDown("Reset")) {
            ResetLevel();
        }

        if (Input.GetButtonDown("Next")) { // Skip to next checkpoint, used for testing
            var i = checkpoints.Length;
            if (i > 0) {
                playerScript.AddCheckpoint(checkpoints[nextCheckpoint].transform.position);
                if (nextCheckpoint < i - 1) {
                    nextCheckpoint++;
                }
                else {
                    nextCheckpoint = 0;
                }
                StartCoroutine(ResetObjects(0.1f));
            }
        }

        if (Input.GetKeyDown(KeyCode.M)) { // Menu
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.X)) { // Hard reset the scene, used for testing
            ResetScene();
        }
	}

    public void ReduceHealth(int amount){
        currentHealth -= amount;
        UpdateHealthBar(currentHealth);

        if (currentHealth < 1) {
            playerScript.DestroyPlayer(true);
        }
    }

    public void RefillHealth(){
        currentHealth = maxHealth;
        UpdateHealthBar(currentHealth);
    }

    public void UpdateHealthBar(int healthLeft){ // Update the UI health sprites to indicate how much hp the player has.
        for(int i = 0; i < maxHealth; i++) {
            healthSprites[i].sprite = i < healthLeft ? healthFull : healthEmpty;
        }
    }

    public void ResetLevel(){
        StartCoroutine(ResetObjects(deathWaitTime));
    }

    public override void Reset()
    {
        RefillHealth();
    }

    public void GoToMenu(){
        SceneManager.LoadScene(0);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    IEnumerator ResetObjects(float waitTime)
    {
        // All objects that require a reset are handled by this function instead of reseting the whole scene every time the player dies.
        yield return new WaitForSeconds(waitTime);
        player.SetActive(true);
        foreach (ResetObject resetObject in _ResetScripts) {
            resetObject.Reset();
        }
    }

    //public void FinishLevel()
    //{
    //    cam.GetComponent<CameraFollow>().ZoomAtPlayer();
    //    playerInput.DisablePlayerInput();
    //    playerScript.PlayLevelFinishAnimation();
    //}
}