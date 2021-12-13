using System;
using System.Collections;
using UnityEngine;

public class World : MonoBehaviour
{
    public GameObject chunkPrefab;
    public static Vector3 worldDimensions = new Vector3(10, 10, 10);
    public static Vector3 chunkDimensions = new Vector3(10, 10, 10);

    private void Start()
    {
        StartCoroutine(BuildWorld());
    }

    IEnumerator BuildWorld()
    {
        for (var z = 0; z < worldDimensions.z; z++)
        {
            for (var y = 0; y < worldDimensions.y; y++)
            {
                for (var x = 0; x < worldDimensions.x; x++)
                {
                    GameObject chunk = Instantiate(chunkPrefab);
                    Vector3 position = new Vector3(x * chunkDimensions.x, y * chunkDimensions.y, z * chunkDimensions.z);
                    chunk.GetComponent<Chunk>().CreateChunk(chunkDimensions, position);
                    yield return null; 
                }
            }
        }
    }
}