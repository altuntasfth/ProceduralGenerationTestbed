using System;
using System.Collections;
using System.Collections.Generic;
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
    public static Vector3Int worldDimensions = new Vector3Int(4, 4, 4);
    public static Vector3Int chunkDimensions = new Vector3Int(10, 10, 10);

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
    
    public static PerlinSettings caveSettings;
    public PerlinGrapher3D caves;

    private HashSet<Vector3Int> chunckChecker = new HashSet<Vector3Int>();
    private HashSet<Vector2Int> chunckColumns = new HashSet<Vector2Int>();
    private Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();
    private Vector3Int lastBuildPosition;

    private void Start()
    {
        loadingBar.maxValue = worldDimensions.x * worldDimensions.z;

        surfaceSettings = new PerlinSettings(surface.heightScale, surface.scale, surface.octaves, surface.heightOffset, surface.probability);
        stoneSettings = new PerlinSettings(stone.heightScale, stone.scale, stone.octaves, stone.heightOffset, stone.probability);
        diamondTopSettings = new PerlinSettings(diamondTop.heightScale, diamondTop.scale, diamondTop.octaves, diamondTop.heightOffset, diamondTop.probability);
        diamondBottomSettings = new PerlinSettings(diamondBottom.heightScale, diamondBottom.scale, diamondBottom.octaves, diamondBottom.heightOffset, diamondBottom.probability);
        caveSettings = new PerlinSettings(caves.heightScale, caves.scale, caves.octaves, caves.heightOffset, caves.drawCutOff);
        
        StartCoroutine(BuildWorld());
    }

    private void BuildChunkColumn(int x, int z)
    {
        for (int y = 0; y < worldDimensions.y; y++)
        {
            Vector3Int position = new Vector3Int(x, y * chunkDimensions.y, z);
            
            if (!chunckChecker.Contains(position))
            {
                GameObject chunk = Instantiate(chunkPrefab);
                chunk.name = "Chunk_" + position.x + "_" + position.y + "_" + position.z;
                Chunk c = chunk.GetComponent<Chunk>();
                c.CreateChunk(chunkDimensions, position);
                chunckChecker.Add(position);
                chunks.Add(position, c);
            }
            else
            {
                chunks[position].meshRenderer.enabled = false;
            }
        }
    }

    IEnumerator BuildWorld()
    {
        for (var z = 0; z < worldDimensions.z; z++)
        {
            for (var x = 0; x < worldDimensions.x; x++)
            {
                BuildChunkColumn(x * chunkDimensions.x, z * chunkDimensions.z);
                loadingBar.value++;
                yield return null; 
            }
        }
        
        mainCamera.SetActive(false);
        int xPos = worldDimensions.x * chunkDimensions.x / 2;
        int zPos = worldDimensions.z * chunkDimensions.z / 2;
        int yPos = (int)MeshUtils.FractalBrownianMotion(xPos, zPos, surface.octaves, surface.scale, surface.heightScale, surface.heightOffset) + 10;
        fpc.transform.position = new Vector3Int(xPos, yPos, zPos);
        loadingBar.gameObject.SetActive(false);
        fpc.SetActive(true);
        lastBuildPosition = Vector3Int.CeilToInt(fpc.transform.position);
    }
}