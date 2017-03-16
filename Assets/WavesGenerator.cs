using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesGenerator : MonoBehaviour {
     Mesh mesh;
     private Vector3 []vertices;
     private int sizeX;
     private int sizeZ;
     private float t;
	// Use this for initialization
	void Start () {
          sizeX = 100;
          sizeZ = 100;
          t = 0;
          Generate();
     }
	
     private void Generate()
     {
          mesh = GetComponent<MeshFilter>().mesh;
          vertices = new Vector3[sizeX * sizeZ];

          for (int i = 0; i!=sizeX; i++)
               for (int j = 0; j!=sizeZ; j++)
               {
                    vertices[i + j * sizeZ] = new Vector3(i * sizeX, 0, j * sizeZ);
               }
          mesh.vertices = vertices;
     }


	// Update is called once per frame
	void Update () {
		
	}

     private void FixedUpdate()
     {
          for (int i = 0; i != sizeX; i++)
               for (int j = 0; j != sizeZ; j++)
               {
                    vertices[i + j * sizeZ].y += Mathf.Sin(t);
               }
          mesh.vertices = vertices;
          t += 0.01f;
     }
}
