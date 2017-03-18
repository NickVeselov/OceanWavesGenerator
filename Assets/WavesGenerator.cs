using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WavesGenerator : MonoBehaviour
{
     Mesh mesh;

     private Vector3[] vertices;
     private Vector3[] baseHeight;
     private bool[] isEdgePoint;
     private float zeroWaterLevel;

     public int sizeX;
     public int sizeZ;

     public float interval_sizeX;
     public float interval_sizeZ;

     public float amplitude;
     public float speed;

     public float lengthX;
     public float lengthZ;

     public float meshHeight;

     private Vector2 offset;

     // Use this for initialization
     void Start()
     {
          Generate();
          baseHeight = mesh.vertices;
          zeroWaterLevel = transform.position.y;

          vertices = new Vector3[baseHeight.Length];
     }

     private void Generate()
     {
          mesh = GetComponent<MeshFilter>().mesh;

          vertices = new Vector3[(sizeX + 1) * (sizeZ + 1)];
          isEdgePoint = new bool[(sizeX + 1) * (sizeZ + 1)];

          //Calculate vertices
          for (int i = 0, x = -sizeX / 2; x <= sizeX / 2; x++)
               for (int z = -sizeZ / 2; z <= sizeZ / 2; z++, i++)
               {
                    vertices[i] = new Vector3(x * interval_sizeX, 0, z * interval_sizeZ);
                    if (x == -sizeX / 2 || x == sizeX / 2 || z == -sizeZ / 2 || z == sizeZ / 2)
                         isEdgePoint[i] = true;
                    else
                         isEdgePoint[i] = false;
               }

          //Calculate triangles
          int[] triangles = new int[sizeX * sizeZ * 6];
          for (int z = 0, tr_ind = 0, vert_ind = 0;
               z < sizeZ;
               z++, vert_ind++)
               for (int x = 0; x < sizeX;
                    x++, tr_ind += 6, vert_ind++)
                    {
                         triangles[tr_ind + 0] = vert_ind;
                         triangles[tr_ind + 1] = vert_ind + sizeX + 1;
                         triangles[tr_ind + 2] = vert_ind + 1;
                         triangles[tr_ind + 3] = vert_ind + 1;
                         triangles[tr_ind + 4] = vert_ind + sizeX + 1;
                         triangles[tr_ind + 5] = vert_ind + sizeX + 2;
                    }

          Vector2[] uvs = new Vector2[vertices.Length];

          for (int i = 0; i < uvs.Length; i++)
          {
               uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
          }

          mesh.vertices = vertices;
          mesh.triangles = triangles;
          mesh.RecalculateNormals();
          mesh.uv = uvs;
          GetComponent<MeshRenderer>().material.SetColor(0, Color.black);
     }


     // Update is called once per frame
     void Update()
     {
          Vector3 vertex;
          Vector3 globalVertex;

          for (int i = 0; i < vertices.Length; i++)
          {
               vertex = baseHeight[i];
               globalVertex = transform.TransformPoint(vertex);

               if (isEdgePoint[i])
                    vertex.y = amplitude + meshHeight;
               else
               {
                    float phase = Mathf.Sin(Time.time * speed + globalVertex.x * lengthX + globalVertex.z * lengthZ);
                    vertex.y = -amplitude * phase;

               }
               
               vertices[i] = vertex;
          }


          mesh.vertices = vertices;
          mesh.RecalculateNormals();
          
     }
}
