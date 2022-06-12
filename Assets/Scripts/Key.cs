using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : ResetObject {
    public enum KeyType{RedKey, YellowKey, BlueKey, GreenKey}
    public KeyType keyType;
    Vector3 origPos;
    Quaternion origRot;

    private void Start(){
        origPos = transform.position;
        origRot = transform.rotation;
    }

    public override void Reset(){
        transform.position = origPos;
        transform.rotation = origRot;
        transform.SetParent(null);
    }
}
