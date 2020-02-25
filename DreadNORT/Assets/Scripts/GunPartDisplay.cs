using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        string output = "";

        output += gunPart.rarity.ToString() + " " + gunPart.GunPartType.ToString() + "\n";

        output += "\n - \n";

        if(gunPart.baseClipSize != 0) {output += "Clip size : " + gunPart.baseClipSize + "\n";}
        if(gunPart.baseBulletsPerShot != 0) {output += "Bullets/Shot : " + gunPart.baseClipSize + "\n";}
        if(gunPart.baseCostPerShot != 0) {output += "Ammo cost/Shot : " + gunPart.baseClipSize + "\n";}
        if(gunPart.baseShotsPerSecond != 0) {output += "Shots/Second : " + gunPart.baseClipSize + "\n";}
           
        if(gunPart.baseSpecialtyIndex != GunPart.Specialty.NONE)
        {
            output += "\n - \n";
            output += "SPECIALTY : " + gunPart.baseSpecialtyChance + "% " + gunPart.baseSpecialtyIndex.ToString() + "\n";
            
        }

        textMesh.text = output;
    }
}
