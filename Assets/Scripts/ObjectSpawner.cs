using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

    public Transform start;
    public Transform end;

    public int minObjects = 1;
    public int maxObjects = 3;

    public GameObject[] objects;
    GameObject birds;

	void Awake () {
        var b = GameObject.Find("Birds");
        if (b == null) {
            birds = new GameObject("Birds");
        } else {
            birds = b;
        }
        var n = Random.Range(minObjects, maxObjects + 1);
		for(int i = 0; i < n; i++) {
            var thisBird = Instantiate(objects[Random.Range(0, objects.Length)], new Vector2(Random.Range(start.position.x, end.position.x), Random.Range(start.position.y, end.position.y)), Quaternion.identity);
            thisBird.transform.parent = birds.transform;
        }
	}
	
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0.5f, 0.9f);
        Gizmos.DrawLine(start.position, new Vector2(end.position.x, start.position.y));
        Gizmos.DrawLine(start.position, new Vector2(start.position.x, end.position.y));

        Gizmos.DrawLine(end.position, new Vector2(start.position.x, end.position.y));
        Gizmos.DrawLine(end.position, new Vector2(end.position.x, start.position.y));
    }
}
