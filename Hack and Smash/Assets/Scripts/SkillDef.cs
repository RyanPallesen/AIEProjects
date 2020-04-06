using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillDef", order = 1)]
public class SkillDef : ScriptableObject
{

    public enum skillType
    {
        Primary,
        Utility,
        Ultimate,
        COUNT
    }

    public MonoBehaviour monoBehaviour;
    public string name;
    public string description;
    public Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
