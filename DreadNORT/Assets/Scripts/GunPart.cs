using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunPart
{
    public string name;
    public static GunPart GetRandom(float cost)
    {
        return GetRandom((Gun.GunPartType)Random.Range(0, (int)Gun.GunPartType.COUNT), Mathf.Sqrt(cost));
    }

    public static GunPart GetRandom(Gun.GunPartType _GunPartType, float cost)
    {
        GunPart returnPart = null;
        switch (_GunPartType)
        {
            case Gun.GunPartType.BODY:
                returnPart = GetRandomBody(cost);
                break;
            case Gun.GunPartType.BODYMOD:
                returnPart = GetRandomBodyMod(cost); break;
            case Gun.GunPartType.BARREL:
                returnPart = GetRandomBarrel(cost); break;
            case Gun.GunPartType.BARRELMOD:
                returnPart = GetRandomBarrelMod(cost); break;
            case Gun.GunPartType.STOCK:
                returnPart = GetRandomStock(cost); break;
            case Gun.GunPartType.SIGHT:
                returnPart = GetRandomSight(cost); break;
            case Gun.GunPartType.SIGHTMOD:
                returnPart = GetRandomSightMod(cost); break;
            case Gun.GunPartType.CAPACITOR:
                returnPart = GetRandomCapacitor(cost); break;
        }

        returnPart.name = _GunPartType.ToString();
        returnPart.GunPartType = _GunPartType; 

        return returnPart;
    }

    private void GenericRandomize(float cost)
    {

    }

    private static GunPart GetRandomBody(float cost)
    {
        //Determines Projectile speed
        //Determines Projectile prefab
        //Determines Recoil (positive)
        //Determines Damage (positive)
        //Determines Projectile Size
        //Determines Projectile Gravity

        GunPart returnPart = new GunPart();

        //Approach zero as cost reaches infinity => 1/x

        int bodyType = Random.Range(0, 3);

        returnPart.baseProjectileSpeed = Mathf.Clamp(Random.Range((cost / 100) * 4, (cost / 100) * 8),2,128);
        //returnPart.projectile = 
        returnPart.baseDamagePerBullet = Random.Range((cost / 100) * 1, (cost / 50) * 2);
        returnPart.baseRecoil = new Vector3(Random.Range(0, 5 * returnPart.baseDamagePerBullet), Random.Range(0, 5 * returnPart.baseDamagePerBullet), 0);
        returnPart.baseProjectileGravity = (Random.Range(-5 * returnPart.baseDamagePerBullet, 5 * returnPart.baseDamagePerBullet));
        returnPart.baseProjectileSize =  Random.Range(1 + (returnPart.baseDamagePerBullet / 200), 2 + (returnPart.baseDamagePerBullet / 200));

        returnPart.GenericRandomize(cost);

        return returnPart;

    }

    private static GunPart GetRandomBodyMod(float cost)
    {
        //Determines a specialtychance and specialtytype [usually empty]
        GunPart returnPart = new GunPart();

        returnPart.baseSpecialtyIndex = (Specialty)Random.Range(0, (int)Specialty.COUNT);
        returnPart.baseSpecialtyChance = Random.Range(1, cost / 250);

        returnPart.GenericRandomize(cost);

        return returnPart;
    }

    private static GunPart GetRandomBarrel(float cost)
    {
        //Determines Bullets per shot
        //Determines Cost per bullet
        //Determines ShotsPerSecond
        //Determines BaseBulletSpread
        //Determines Recoil (positive)
        //Determines Damage (negative)

        GunPart returnPart = new GunPart();

        returnPart.baseBulletsPerShot = Random.Range(1, 4);

        if(returnPart.baseBulletsPerShot == 1)
        {
            returnPart.baseBulletsPerShot = (int)Random.Range( 1 + (cost/250), Mathf.Sqrt(cost / 150));
        }
        else if (returnPart.baseBulletsPerShot == 2)
        {
            returnPart.baseBulletsPerShot = 1;
        }
        else if (returnPart.baseBulletsPerShot == 3)
        {
            returnPart.baseBulletsPerShot = (int)Random.Range(4 * cost / 150, 8 * cost / 100);
        }

        returnPart.baseShotsPerSecond = Random.Range(1 * (cost / 50), 5 * (cost / 150)) / (returnPart.baseBulletsPerShot);



        returnPart.baseBulletSpread = Random.Range(0, 5 * (returnPart.baseBulletsPerShot * returnPart.baseShotsPerSecond));

        returnPart.baseRecoil = new Vector3(Random.Range(0, 5 * (returnPart.baseBulletsPerShot * returnPart.baseShotsPerSecond)), Random.Range(0, 5 * (returnPart.baseBulletsPerShot * returnPart.baseShotsPerSecond)));

        returnPart.baseDamagePerBullet = -Random.Range(0, 2 * returnPart.baseBulletsPerShot * returnPart.baseShotsPerSecond);

        if (returnPart.baseBulletsPerShot == 1)
        {
            returnPart.baseDamagePerBullet = -Random.Range(25 * (cost/100), 50 * (cost/50));

            returnPart.baseShotsPerSecond /= 15;

        }

        returnPart.GenericRandomize(cost);

        return returnPart;
    }

    private static GunPart GetRandomBarrelMod(float cost)
    {
        //Determines a specialtychance and specialtytype [usually empty]
        GunPart returnPart = new GunPart();

        returnPart.baseSpecialtyIndex = (Specialty)Random.Range(0, (int)Specialty.COUNT);
        returnPart.baseSpecialtyChance = Random.Range(1, cost / 200);

        returnPart.GenericRandomize(cost);

        return returnPart;
    }

    private static GunPart GetRandomStock(float cost)
    {
        //Determines Recoil (negative)
        //Determines BaseBulletSpread (negative)
        GunPart returnPart = new GunPart();

        returnPart.baseRecoil = new Vector3(Random.Range(2, 5 * (cost / 100)), Random.Range(2, 5 * (cost / 100)));
        returnPart.baseBulletSpread = -Random.Range(2, 5 * (cost / 100));

        returnPart.GenericRandomize(cost);

        return returnPart;
    }

    private static GunPart GetRandomSight(float cost)
    {
        //Determines zoom type
        //Determines zoom amount
        GunPart returnPart = new GunPart();

        returnPart.zoomType = (ZoomType)((int)ZoomType.COUNT - Mathf.FloorToInt(Mathf.Sqrt(Random.Range((cost/350), ((int)ZoomType.COUNT + 1) * ((int)ZoomType.COUNT + 1)) - 1)));

        returnPart.zoomMultiplier = Random.Range(1f, 2.5f) * (int)returnPart.zoomType;

        returnPart.GenericRandomize(cost);

        return returnPart;
    }

    private static GunPart GetRandomSightMod(float cost)
    {
        //Determines what variables are seen on hud
        //Determines a specialtychance and specialtytype [usually empty] when scoped
        GunPart returnPart = new GunPart();

        returnPart.baseSpecialtyIndex = (Specialty)Random.Range(0, (int)Specialty.COUNT);
        returnPart.baseSpecialtyChance = Random.Range(1, cost / 250);

        returnPart.GenericRandomize(cost);

        return returnPart;
    }

    private static GunPart GetRandomCapacitor(float cost)
    {
        //Determines clip/magazine size
        //Determines Reload Speed

        GunPart returnPart = new GunPart();

        returnPart.baseClipSize = (int)Random.Range(8, 15 * Mathf.Sqrt(cost / 50));
        returnPart.baseReloadSpeed = Random.Range(1, 5) * Mathf.Sqrt(returnPart.baseClipSize / (cost / 100));
        returnPart.GenericRandomize(cost);

        return returnPart;

    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Customized,
        Rare,
        Specialty,
        Legendary,
        Badass,
    }

    public enum Specialty
    {
        RICOCHET,
        PIERCING,
        CHAINING,
        HOMING,
        LIFESTEAL,

        COUNT
    }


    public enum ZoomType
    {
        NAKED,
        IRONSIGHTS,
        IRONSIGHTMARKER,
        REDDOT,
        HUD,

        COUNT
    }
    [Header("Instantiation")]
    public Gun.GunPartType GunPartType;
    public GameObject partPrefab;
    public int baseCost;
    public Rarity rarity;

    [Header("Ammo Capacity")]
    public int baseClipSize;
    public int baseAmmoSize;

    [Header("Shots")]
    public int baseBulletsPerShot;
    public float baseCostPerShot;
    public float baseShotsPerSecond;

    [Header("Specialties")]
    public float baseSpecialtyChance;
    public Specialty baseSpecialtyIndex;
    public AudioClip specialActivatedSound;

    [Header("Misc")]
    public Vector3 baseRecoil;
    public float baseBulletSpread;
    public float baseReloadSpeed;
    public float baseDamagePerBullet;

    [Header("Zoom")]
    public ZoomType zoomType;
    public float zoomMultiplier;

    [Header("Projectile Variables")]
    public float baseProjectileSpeed;
    public float baseProjectileSize;
    public float baseProjectileGravity;

    [Header("References")]
    public GameObject projectile;
    public GameObject muzzleFlashPrefab;
    public AudioClip fireBulletSound;
    public AudioClip hitBulletSound;
}
