using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class WavesGenerator : MonoBehaviour
{
     Mesh mesh;

     public InputField amplitude_IF;
     public InputField frequency_IF;
     public InputField lengthX_IF;
     public InputField lengthZ_IF;
     public Slider waterLevelSlider;
     public Dropdown presetDropdown;

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

     private bool manualUIEditing;
     

     // Use this for initialization
     void Start()
     {
          Generate();
          baseHeight = mesh.vertices;
          zeroWaterLevel = transform.position.y;

          vertices = new Vector3[baseHeight.Length];

          manualUIEditing = false;
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


          Vector3 mesh_location = GetComponent<Transform>().position;
          GetComponent<Transform>().position = new Vector3(mesh_location.x, meshHeight+3, mesh_location.z);// position.y = meshHeight + 3;
          
     }


     public void UpdateWave()
     {
          if (!manualUIEditing)
          {
               if (amplitude_IF.text != "")
                    amplitude = float.Parse(amplitude_IF.text);
               if (frequency_IF.text != "")
                    speed = float.Parse(frequency_IF.text);
               if (lengthX_IF.text != "")
                    lengthX = float.Parse(lengthX_IF.text);
               if (lengthZ_IF.text != "")
                    lengthZ = float.Parse(lengthZ_IF.text);

               meshHeight = waterLevelSlider.value;
          }
     }

     public void UsePreset()
     {
          switch (presetDropdown.value)
          {
               case 0:
                    {
                         amplitude = 10f;
                         speed = 2f;
                         lengthX = 0.01f;
                         lengthZ = 0.03f;
                         meshHeight = 27f;
                         break;
                    }
               case 1:
                    {
                         amplitude = 60f;
                         speed = 1.5f;
                         lengthX = 0.005f;
                         lengthZ = 0.01f;
                         meshHeight = 71f;
                         break;
                    }
               case 2:
                    {
                         amplitude = 15f;
                         speed = 2f;
                         lengthX = 0.6f;
                         lengthZ = 0f;
                         meshHeight = 40f;
                         break;
                    }
               case 3:
                    {
                         amplitude = 3f;
                         speed = 10f;
                         lengthX = 15f;
                         lengthZ = 6f;
                         meshHeight = 30f;
                         break;
                    }
          }

          manualUIEditing = true;

          amplitude_IF.text = amplitude.ToString();
          frequency_IF.text = speed.ToString();
          lengthX_IF.text = lengthX.ToString();
          lengthZ_IF.text = lengthZ.ToString();
          waterLevelSlider.value = meshHeight;

          manualUIEditing = false;
     }
}
