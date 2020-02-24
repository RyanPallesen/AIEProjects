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
            //chunk.meshObject.name += "DROWNED";
            chunk.tags.Add(TerrainChunk.Tags.UNDERWATER);
        }
        else if (baseAverage < beachHeight)
        {
            //chunk.meshObject.name += "Beach";
            chunk.tags.Add(TerrainChunk.Tags.BEACH);
        }

        if (mayContainTown)
        {
            //chunk.meshObject.name += "TOWN";

            chunk.tags.Add(TerrainChunk.Tags.TOWN);

            UpdateTownChunk(chunk);
        }
    }

    int numTowns = 0;

    void UpdateTownChunk(TerrainChunk chunk)
    {
        chunk.meshObject.name = " ";
        Color randomColor = Random.ColorHSV();

        if (chunk.tags.Contains(TerrainChunk.Tags.TOWN))
        {
            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);

            temp.transform.SetParent(chunk.meshObject.transform);
            temp.transform.localPosition = new Vector3(0, chunk.averageHeight, 0);
            temp.transform.localScale = new Vector3(32, 32, 32);



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

                    if(terrainChunkDictionary[viewedChunkCoord].townchunk != null)//neighbour has a town
                    {
                        if (chunk.townchunk == null)//this chunk does not have a town
                        {
                            chunk.townchunk = terrainChunkDictionary[viewedChunkCoord].townchunk;
                            randomColor = chunk.townchunk.townColor;
                            chunk.meshObject.name = terrainChunkDictionary[viewedChunkCoord].meshObject.name;
                        }
                        else if (terrainChunkDictionary[viewedChunkCoord].townchunk != chunk.townchunk)//neighbouring chunk has a different town.
                        {
                            terrainChunkDictionary[viewedChunkCoord].townchunk = chunk.townchunk;
                            terrainChunkDictionary[viewedChunkCoord].meshObject.transform.GetChild(0).GetComponent<Renderer>().material.color = chunk.townchunk.townColor;
                           terrainChunkDictionary[viewedChunkCoord].meshObject.name = chunk.meshObject.name;

                            UpdateTownChunk(terrainChunkDictionary[viewedChunkCoord]);

                            chunk.meshObject.transform.Translate(new Vector3(0, -10, 0));
                        }

                    }
                }
            }

            if (chunk.townchunk == null)//no neighbours contained town
            {
                chunk.townchunk = new TownChunk();
                chunk.townchunk.townColor = randomColor;
                temp.GetComponent<Renderer>().material.color = randomColor;

                chunk.townchunk.terrainChunks.Add(chunk);
                chunk.meshObject.name = numTowns.ToString();
                numTowns += 1;
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
