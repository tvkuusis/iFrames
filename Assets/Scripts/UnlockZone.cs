using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;
using System.Linq;

public class UnlockZone : MonoBehaviour {

    public float destroyTime = 3f;
    float timeStep;
    public Key.KeyType correctKey;
    Key thisKey;
    //public UnityEvent onActivation;
    SortedDictionary<float, GameObject> sortedBlocks = new SortedDictionary<float, GameObject>();

    public GameObject[] blocks;
    public GameObject smokeEffect;

    PlayerScript player;

    void Start() {
        player = GameObject.Find("Player").GetComponent<PlayerScript>();
        RearrangeBlocks();
        timeStep = destroyTime / blocks.Length;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            StartCoroutine(DestroyBlocks());
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        thisKey = col.gameObject.GetComponent<Key>();
        if (thisKey != null && thisKey.keyType.Equals(correctKey)){
            //print("Correct key!");
            col.gameObject.SetActive(false);
            player.RemovePickUp();
            //onActivation.Invoke();
            gameObject.GetComponent<SpriteRenderer>().color = new Color(.8f, .8f, .8f, .3f);
            StartCoroutine(DestroyBlocks());
        }
        else {
           // print("Incorrect object");
        }
    }

    void RearrangeBlocks(){
        for (int i = 0; i < blocks.Length; i++) {
            sortedBlocks.Add(CalculateDistance(gameObject, blocks[i]), blocks[i]);
        }
        blocks = sortedBlocks.Values.ToArray();
    }

    float CalculateDistance(GameObject g1, GameObject g2){
        float dist = Vector2.Distance(g1.transform.position, g2.transform.position);
        return dist;
    }

    IEnumerator DestroyBlocks(){
        for (int i = 0; i < blocks.Length; i++) {
            blocks[i].SetActive(false);
            var smoke = Instantiate(smokeEffect, blocks[i].transform.position, Quaternion.identity);
            Destroy(smoke, 1f);
            yield return new WaitForSeconds(timeStep);
        }
    }
}