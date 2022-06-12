
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class RotateObject : MonoBehaviour
{


    // Rotates the selected Game Object +45 degrees if the user presses 'g'
    // or -45 degrees if the user presses 'Shift + g'
    // If no object is selected, the Menus are grayed out.

    public static GameObject activeGameObject;

    //[@MenuItem("Rotate/Rotate +90 _g")]
    //static void RotateGreenPlus45()
    //{
    //    var obj = Selection.activeGameObject;
    //    obj.transform.Rotate(Vector3.back * 90);
    //}

    //[@MenuItem("Example/Rotate Green +45 _g", true)]
    //static function ValidatePlus45()
    //{
    //    return Selection.activeGameObject != null;
    //}


    [@MenuItem("Rotate/Rotate 90 %g")]
    static void RotateObject90()
    {
        var obj = Selection.activeGameObject;
        obj.transform.Rotate(Vector3.forward * -90);
    }

    [@MenuItem("Rotate/Flip X %h")]
    static void FlipObject()
    {
        var obj = Selection.activeGameObject;
        obj.transform.localScale = obj.transform.localScale.x == 1 ? new Vector3(-1, 1, 1) : new Vector3(1,1,1);
    }

    //[@MenuItem("Example/Rotate green -45 #g", true)]
    //static function ValidateMinus45()
    //{
    //    return Selection.activeGameObject != null;
    //}
}
#endif