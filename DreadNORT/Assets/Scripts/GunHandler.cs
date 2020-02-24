using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    public Gun gun;

    public int AvailableCost;


    private static GunHandler _instance;

    public static GunHandler Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gun = GenerateNewGun(AvailableCost);

        FindObjectOfType<PlayerHUD>().gun = gun;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(gun);
            gun = GenerateNewGun(AvailableCost);

            FindObjectOfType<PlayerHUD>().gun = gun;
        }
    }
    Gun GenerateNewGun(float numToSpend)
    {
        List<Gun.GunPartType> gunPartTypes = new List<Gun.GunPartType>();

        for (int i = 0; i < (int)Gun.GunPartType.COUNT; i++)
        {
            gunPartTypes.Add((Gun.GunPartType)i);
        }


        Gun.GunPartType[] MyArray = gunPartTypes.ToArray();

        System.Random rnd = new System.Random();

        Gun.GunPartType[] MyRandomArray = MyArray.OrderBy(x => rnd.Next()).ToArray();

        Gun returnGun = gameObject.AddComponent<Gun>();

        for (int i = 0; i < (int)Gun.GunPartType.COUNT; i++)
        {

            returnGun.gunParts[(int)MyRandomArray[i]] = GunPart.GetRandom(MyRandomArray[i], numToSpend);
        }

        return returnGun;
    }




    // Update is called once per frame
    void Update()
    {

    }
}
