using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownChunk
{
    public List<TerrainChunk> terrainChunks = new List<TerrainChunk>();
    public Color townColor = Random.ColorHSV();

    public void MergeTownChunks(TownChunk otherChunk)
    {
        townColor = Color.Lerp(townColor, otherChunk.townColor, 0.5f);

        for (int i = 0; i < otherChunk.terrainChunks.Count; i++)
        {
            terrainChunks.Add(otherChunk.terrainChunks[i]);
            otherChunk.terrainChunks[i].townchunk = this;
            
        }

        //for ( int i = 0; i < terrainChunks.Count; i++)
        //{
        //    terrainChunks[i].meshObject.transform.GetChild(0).GetComponent<Renderer>().material.color = townColor;
        //}

        otherChunk = null;

        Debug.LogError("Merged two chunks");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
