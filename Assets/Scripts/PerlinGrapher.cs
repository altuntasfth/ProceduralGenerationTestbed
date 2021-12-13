using System;
using UnityEngine;

[ExecuteInEditMode]
public class PerlinGrapher : MonoBehaviour
{
    public LineRenderer lr;
    public float heightScale = 2f;
    public float scale = 0.5f;
    public int octaves = 1;
    public float heightOffset;

    private void Start()
    {
        Graph();
    }

    

    private void Graph()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 100;
        
        int z = 11;
        Vector3[] positions = new Vector3[lr.positionCount];
        for (int x = 0; x < lr.positionCount; x++)
        {
            float y = MeshUtils.FractalBrownianMotion(x, z, octaves, scale, heightScale, heightOffset);
            positions[x] = new Vector3(x, y, z);
        }
        lr.SetPositions(positions);
    }

    private void OnValidate()
    {
        Graph();
    }
}