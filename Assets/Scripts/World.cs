using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class World : MonoBehaviour
{
    public static Vector3 worldDimensions = new Vector3(3, 3, 3);
    public static Vector3 chunkDimensions = new Vector3(10, 10, 10);

    public GameObject chunkPrefab;
    public GameObject mainCamera;
    public GameObject fpc;
    public Slider loadingBar;

    private void Start()
    {
        loadingBar.maxValue = worldDimensions.x * worldDimensions.y * worldDimensions.z;
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
                    loadingBar.value++;
                    yield return null; 
                }
            }
        }
        
        mainCamera.SetActive(false);
        float xPos = worldDimensions.x * chunkDimensions.x / 2f;
        float zPos = worldDimensions.z * chunkDimensions.z / 2f;
        Chunk c = chunkPrefab.GetComponent<Chunk>();
        float yPos = MeshUtils.FractalBrownianMotion(xPos, zPos, c.octaves, c.scale, c.heightScale, c.heightOffset) + 10f;
        fpc.transform.position = new Vector3(xPos, yPos, zPos);
        loadingBar.gameObject.SetActive(false);
        fpc.SetActive(true);
    }
}