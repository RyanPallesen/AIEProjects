using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour
{

    public static TerrainGenerator instance;
    public LibNoise.Voronoi voronoi = new LibNoise.Voronoi();

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

    public List<TownChunk> existingTownChunks = new List<TownChunk>();

    float maxHeight;
    float minHeight;

    void Start()
    {
        instance = this;
        voronoi.Seed = heightMapSettings.noiseSettings.seed;

        textureSettings.ApplyToMaterial(mapMaterial);
        textureSettings.UpdateMeshHeights(mapMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);

        float maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
        meshWorldSize = meshSettings.meshWorldSize;
        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / meshWorldSize);

        StartCoroutine(UpdateVisibleChunks());
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
            StartCoroutine(UpdateVisibleChunks());
        }


    }

    IEnumerator UpdateVisibleChunks()
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
                    yield return new WaitForEndOfFrame();
                    if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                    {
                        terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
                    }
                    else
                    {
                        GenerateChunkAt(viewedChunkCoord);

                    }
                }

            }
        }

    }

    public float townHeightRange = 10f;
    public float waterHeight = 10f;
    public float beachHeight = 15f;

    int numTowns = 0;

    void AttemptPopulateChunk(TerrainChunk chunk)
    {
        if (!chunk.hasGenerated)
        {
            chunk.meshObject.name = "";

            float baseAverage = chunk.averageHeight;

            bool mayContainTown = false;




            if (chunk.heightMap.maxValue - chunk.heightMap.minValue < townHeightRange)
            {
                mayContainTown = true;
            }

            if (baseAverage < waterHeight)
            {
                mayContainTown = false;
                chunk.meshObject.name += " DROWNED";
                chunk.tags.Add(TerrainChunk.Tags.UNDERWATER);
            }
            else if (baseAverage < beachHeight)
            {
                chunk.meshObject.name += " BEACH";
                chunk.tags.Add(TerrainChunk.Tags.BEACH);
            }

            if (mayContainTown)
            {
                chunk.meshObject.name += " TOWN";

                chunk.tags.Add(TerrainChunk.Tags.TOWN);
                UpdateSurroundingTown(chunk);
            }

            chunk.hasGenerated = true;

        }
    }



    TerrainChunk GenerateChunkAt(Vector2 viewedChunkCoords, TownChunk chunkParent = null)
    {
        TerrainChunk newChunk = new TerrainChunk(viewedChunkCoords, heightMapSettings, meshSettings, detailLevels, colliderLODIndex, transform, viewer, mapMaterial);
        
        if(chunkParent != null)
        {
            newChunk.townchunk = chunkParent;
            chunkParent.terrainChunks.Add(newChunk);

        }

        terrainChunkDictionary.Add(viewedChunkCoords, newChunk);
        newChunk.onVisibilityChanged += OnTerrainChunkVisibilityChanged;

        newChunk.onGenerate += AttemptPopulateChunk;

        newChunk.Load();

        return newChunk;
    }



    void UpdateSurroundingTown(TerrainChunk chunk)
    {
        if (!chunk.hasBeenTownIndexed)
        {
            chunk.hasBeenTownIndexed = true;

            if (chunk.townchunk == null)
            {
                numTowns++;

                chunk.townchunk = new TownChunk();
                existingTownChunks.Add(chunk.townchunk);

                chunk.townchunk.townColor = Random.ColorHSV();

                chunk.meshObject.name = "Origin " + numTowns;
            }

            chunk.townchunk.AddNewChunk(chunk);

            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);

            temp.transform.SetParent(chunk.meshObject.transform);
            temp.transform.localPosition = new Vector3(0, chunk.averageHeight, 0);
            temp.transform.localScale = new Vector3(32, 32, 32);
            temp.GetComponent<Renderer>().material.color = chunk.townchunk.townColor;

            //Instantiate neighbours to any town chunk
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector2 viewedChunkCoords = new Vector2(chunk.coord.x + x, chunk.coord.y + y);

                    if (!terrainChunkDictionary.ContainsKey(viewedChunkCoords))
                    {
                        TerrainChunk generatedChunk = GenerateChunkAt(viewedChunkCoords, chunk.townchunk);
                        generatedChunk.townchunk = chunk.townchunk;
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
