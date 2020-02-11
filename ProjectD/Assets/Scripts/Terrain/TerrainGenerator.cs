using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour
{

    const float viewerMoveThresholdForChunkUpdate = 25f;
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;


    public int colliderLODIndex;
    public LODInfo[] detailLevels;

    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;
    public TextureData textureSettings;

    public Transform viewer;
    public Material mapMaterial;

    Vector2 viewerPosition;
    Vector2 viewerPositionOld;

    float meshWorldSize;
    int chunksVisibleInViewDst;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();

    float maxHeight;
    float minHeight;

    void Start()
    {

        textureSettings.ApplyToMaterial(mapMaterial);
        textureSettings.UpdateMeshHeights(mapMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);

        float maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
        meshWorldSize = meshSettings.meshWorldSize;
        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / meshWorldSize);

        UpdateVisibleChunks();
    }

    void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);

        if (viewerPosition != viewerPositionOld)
        {
            foreach (TerrainChunk chunk in visibleTerrainChunks)
            {
                chunk.UpdateCollisionMesh();
            }
        }

        if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
        {
            viewerPositionOld = viewerPosition;
            UpdateVisibleChunks();
        }
    }

    void UpdateVisibleChunks()
    {
        HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2>();
        for (int i = visibleTerrainChunks.Count - 1; i >= 0; i--)
        {
            alreadyUpdatedChunkCoords.Add(visibleTerrainChunks[i].coord);
            visibleTerrainChunks[i].UpdateTerrainChunk();
        }

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / meshWorldSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / meshWorldSize);

        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
                if (!alreadyUpdatedChunkCoords.Contains(viewedChunkCoord))
                {
                    if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                    {
                        terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
                    }
                    else
                    {
                        TerrainChunk newChunk = new TerrainChunk(viewedChunkCoord, heightMapSettings, meshSettings, detailLevels, colliderLODIndex, transform, viewer, mapMaterial);
                        terrainChunkDictionary.Add(viewedChunkCoord, newChunk);
                        newChunk.onVisibilityChanged += OnTerrainChunkVisibilityChanged;
                        newChunk.onGenerate += AttemptPopulateChunk;

                        if (newChunk.heightMap.minValue < minHeight)
                        {
                            minHeight = newChunk.heightMap.minValue;
                        }

                        if (newChunk.heightMap.maxValue < maxHeight)
                        {
                            maxHeight = newChunk.heightMap.maxValue;
                        }

                        newChunk.Load();

                    }
                }

            }
        }
    }

    public float townHeightRange = 10f;
    public float waterHeight = 10f;
    public float beachHeight = 15f;
    void AttemptPopulateChunk(TerrainChunk chunk)
    {
        chunk.meshObject.name = chunk.averageHeight.ToString();

        float baseAverage = chunk.averageHeight;

        bool mayContainTown = true;


        //check for chokepoint on land v water, check until water, find total distance from water to water.
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2 viewedChunkCoord = new Vector2(chunk.coord.x + x, chunk.coord.y + y);
                if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    TerrainChunk checkingChunk = terrainChunkDictionary[viewedChunkCoord];

                    if (checkingChunk.averageHeight != 0)//verify chunk has been initialized
                    {
                        if (checkingChunk.averageHeight > baseAverage - townHeightRange && checkingChunk.averageHeight < baseAverage + townHeightRange)
                        {

                        }
                        else
                        {
                            mayContainTown = false;
                        }
                    }

                }
            }
        }

        if (baseAverage < waterHeight)
        {
            mayContainTown = false;
            chunk.meshObject.name += "DROWNED";
            chunk.tags.Add(TerrainChunk.Tags.UNDERWATER);
        }
        else if (baseAverage < beachHeight)
        {
            chunk.meshObject.name += "Beach";
            chunk.tags.Add(TerrainChunk.Tags.BEACH);
        }

        if (mayContainTown)
        {
            chunk.meshObject.name += "TOWN";

            chunk.tags.Add(TerrainChunk.Tags.TOWN);

            UpdateTownChunk(chunk);
        }
    }

    void UpdateTownChunk(TerrainChunk chunk)
    {
        chunk.hasBeenIndexed = true;
        Color randomColor = Random.ColorHSV();

        Debug.Log("ETAET");
        if (chunk.tags.Contains(TerrainChunk.Tags.TOWN))
        {
            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);

            temp.transform.SetParent(chunk.meshObject.transform);
            temp.transform.localPosition = new Vector3(0, chunk.averageHeight, 0);
            temp.transform.localScale = new Vector3(32, 32, 32);
            temp.GetComponent<Renderer>().material.color = randomColor;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector2 viewedChunkCoord = new Vector2(chunk.coord.x + x, chunk.coord.y + y);

                    if (!terrainChunkDictionary.ContainsKey(viewedChunkCoord))//if chunk has not been initialized, initialize it. needed to complete town generation as a whole unit
                    {
                        TerrainChunk newChunk = new TerrainChunk(viewedChunkCoord, heightMapSettings, meshSettings, detailLevels, colliderLODIndex, transform, viewer, mapMaterial);
                        terrainChunkDictionary.Add(viewedChunkCoord, newChunk);
                        newChunk.onVisibilityChanged += OnTerrainChunkVisibilityChanged;
                        newChunk.onGenerate += AttemptPopulateChunk;
                        newChunk.Load();
                    }
                }
            }
        }
    }

    void OnTerrainChunkVisibilityChanged(TerrainChunk chunk, bool isVisible)
    {
        if (isVisible)
        {
            visibleTerrainChunks.Add(chunk);
        }
        else
        {
            visibleTerrainChunks.Remove(chunk);
        }
    }

}

[System.Serializable]
public struct LODInfo
{
    [Range(0, MeshSettings.numSupportedLODs - 1)]
    public int lod;
    public float visibleDstThreshold;


    public float sqrVisibleDstThreshold
    {
        get
        {
            return visibleDstThreshold * visibleDstThreshold;
        }
    }
}
