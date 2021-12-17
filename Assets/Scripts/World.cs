using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public struct PerlinSettings
{
    public float heightScale;
    public float scale;
    public int octaves;
    public float heightOffset;
    public float probability;

    public PerlinSettings(float heightScale, float scale, int octaves, float heightOffset, float probability)
    {
        this.heightScale = heightScale;
        this.scale = scale;
        this.octaves = octaves;
        this.heightOffset = heightOffset;
        this.probability = probability;
    }
}

public class World : MonoBehaviour
{
    public static Vector3 worldDimensions = new Vector3(3, 3, 3);
    public static Vector3 chunkDimensions = new Vector3(10, 10, 10);

    public GameObject chunkPrefab;
    public GameObject mainCamera;
    public GameObject fpc;
    public Slider loadingBar;

    public static PerlinSettings surfaceSettings;
    public PerlinGrapher surface;
    
    public static PerlinSettings stoneSettings;
    public PerlinGrapher stone;
    
    public static PerlinSettings diamondTopSettings;
    public PerlinGrapher diamondTop;
    
    public static PerlinSettings diamondBottomSettings;
    public PerlinGrapher diamondBottom;

    private void Start()
    {
        loadingBar.maxValue = worldDimensions.x * worldDimensions.y * worldDimensions.z;

        surfaceSettings = new PerlinSettings(surface.heightScale, surface.scale, surface.octaves, surface.heightOffset, surface.probability);
        stoneSettings = new PerlinSettings(stone.heightScale, stone.scale, stone.octaves, stone.heightOffset, stone.probability);
        diamondTopSettings = new PerlinSettings(diamondTop.heightScale, diamondTop.scale, diamondTop.octaves, diamondTop.heightOffset, diamondTop.probability);
        diamondBottomSettings = new PerlinSettings(diamondBottom.heightScale, diamondBottom.scale, diamondBottom.octaves, diamondBottom.heightOffset, diamondBottom.probability);
        
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
        float yPos = MeshUtils.FractalBrownianMotion(xPos, zPos, surface.octaves, surface.scale, surface.heightScale, surface.heightOffset) + 10f;
        fpc.transform.position = new Vector3(xPos, yPos, zPos);
        loadingBar.gameObject.SetActive(false);
        fpc.SetActive(true);
    }
}