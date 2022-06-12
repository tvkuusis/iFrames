using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class Gradient : MonoBehaviour {

    public Color colorTop = Color.white;
    public Color colorBottom = Color.black;
    MeshFilter meshFilter;
    Mesh meshCopy;

    private void Start()
    {
        RefreshGradient();
    }

    public void RefreshGradient()
    {
        GetComponent<MeshRenderer>().enabled = true;
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Color[] colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            colors[i] = Color.Lerp(colorBottom, colorTop, vertices[i].y);
        }

        mesh.colors = new Color[] { colorBottom, colorTop, colorBottom, colorTop };

        //meshFilter = GetComponent<MeshFilter>();
        //meshFilter.mesh.colors = new Color[] { colorBottom, colorTop, colorBottom, colorTop };
    }

    void Update(){

    }
}
