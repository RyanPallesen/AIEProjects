using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObjectScript : MonoBehaviour
{
    public Terrain MiniTerrainData;
    public Terrain BigTerrainData;

    public GameObject GameObjectStorage;

    // Start is called before the first frame update
    void Start()
    {

        BigTerrainData.terrainData.size = new Vector3(3, 3, 3);
        //TODO: change texture resolution
        BigTerrainData.transform.localPosition = new Vector3(-1.5f, 0, -1.5f);
        GameObjectStorage.transform.localScale = new Vector3(3, 3, 3);
        
    }

 
}
