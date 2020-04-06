using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class LocalManager : NetworkBehaviour
{
    public Player player;

    public List<ScriptableObject> Primary;
    public List<ScriptableObject> Secondary;
    public List<ScriptableObject> Movement;
    public List<ScriptableObject> Ultimate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
