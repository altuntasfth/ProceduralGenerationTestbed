using System;
using UnityEngine;

[ExecuteInEditMode]
public class PerlinGrapher3D : MonoBehaviour
{
    public float heightScale = 2f;
    [Range(0f, 1f)]
    public float scale = 0.5f;
    public int octaves = 1;
    public float heightOffset;
    [Range(0f, 10f)]
    public float drawCutOff;
    
    private Vector3 dimensions = new Vector3(10, 10, 10);

    private void OnValidate()
    {
        Graph();
    }

    private void CreateCubes()
    {
        for (var z = 0; z < dimensions.z; z++)
        {
            for (var y = 0; y < dimensions.y; y++)
            {
                for (var x = 0; x < dimensions.z; x++)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.name = "perlin_cube";
                    cube.transform.parent = this.transform;
                    cube.transform.position = new Vector3(x, y, z);
                }
            }
        }
    }

    private void Graph()
    {
        MeshRenderer[] cubes = GetComponentsInChildren<MeshRenderer>();

        if (cubes.Length == 0)
        {
            CreateCubes();
        }

        if (cubes.Length == 0)
        {
            return;
        }
        
        for (var z = 0; z < dimensions.z; z++)
        {
            for (var y = 0; y < dimensions.y; y++)
            {
                for (var x = 0; x < dimensions.z; x++)
                {
                    float p3d = MeshUtils.FractalBrownianMotion3D(x, y, z, octaves, scale, heightScale, heightOffset);

                    if (p3d < drawCutOff)
                    {
                        cubes[x + (int)dimensions.x * (y + (int)dimensions.z * z)].enabled = false;
                    }
                    else
                    {
                        cubes[x + (int)dimensions.x * (y + (int)dimensions.z * z)].enabled = true;
                    }
                }
            }
        }
    }
}