using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPartDisplay : MonoBehaviour
{
    public GunPart gunPart;

    public TextMeshProUGUI textMesh;

    // Start is called before the first frame update
    void Start()
    {
        gunPart = GunPart.GetRandom(500);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
