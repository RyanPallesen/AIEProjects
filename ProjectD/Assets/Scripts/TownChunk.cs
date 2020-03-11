using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TownChunk
{
    [Header("Data")]
    [HideInInspector] public List<TerrainChunk> terrainChunks = new List<TerrainChunk>();
    public List<Vector3> wallPositions = new List<Vector3>();

    [Header("Temporary")]
    List<GameObject> gameObjects = new List<GameObject>();
    public LineRenderer lineRenderer;

    [Header("Town Stats")]
    public Color townColor = Random.ColorHSV();
    public int TownSize = 0;
    public string TownName = "";
    public WallType wallType;

    public GameObject self = new GameObject();

    public enum WallType
    {
        NONE = 1,
        SOMEFENCE,
        FENCED,
        ROCKFENCE,
        WOODWALL,
        STONEWALL,
        GREATWALL,
        METALWALL
    }

    public TownChunk()
    {
        TownName = Random.Range(0, float.PositiveInfinity).ToString();
    }

    public void AddNewChunk(TerrainChunk chunkToAdd)
    {
        TownSize++;
        wallType = (WallType)Mathf.FloorToInt(Mathf.Sqrt(TownSize));

        if (!lineRenderer)
        {
            lineRenderer = self.AddComponent<LineRenderer>();
        }

        chunkToAdd.meshObject.transform.SetParent(self.transform);

        terrainChunks.Add(chunkToAdd);

        for (int i = -1; i <= 1; i += 2)
        {
            wallPositions.Add(chunkToAdd.meshObject.transform.position + new Vector3(0, 64, (i * 64)));
            wallPositions.Add(chunkToAdd.meshObject.transform.position + new Vector3((i * 64), 64, 0));
        }

        List<Vector3> hull = JarvisMarchConvexHull.GetHull(wallPositions.ToArray()).ToList();



        for (var i = 0; i < gameObjects.Count; i++)
        {
            GameObject.Destroy(gameObjects[i]);
        }

        foreach (Vector3 point in hull)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = point;
            go.transform.localScale = new Vector3(16, 16, 16);
            go.transform.SetParent(chunkToAdd.meshObject.transform);
            gameObjects.Add(go);
        }

        // Set up game object with mesh;
        MeshRenderer mr = self.GetComponent<MeshRenderer>();
        if (!mr)
            mr = self.AddComponent<MeshRenderer>();


        MeshFilter mf = self.GetComponent<MeshFilter>();
        if (!mf)
            mf = self.AddComponent<MeshFilter>();

        mf.mesh = HullMesher.BuildPolygon(hull.ToArray());
    }

    public void MergeTownChunks(TownChunk otherChunk)
    {
        townColor = Color.Lerp(townColor, otherChunk.townColor, 0.5f);

        for (int i = 0; i < otherChunk.terrainChunks.Count; i++)
        {
            terrainChunks.Add(otherChunk.terrainChunks[i]);
            otherChunk.terrainChunks[i].townchunk = this;

        }

        otherChunk = null;

        Debug.LogError("Merged two chunks");
    }

}
