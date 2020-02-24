using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public enum GunPartType
    {
        BODY,
        //Determines Projectile speed
        //Determines Projectile prefab
        //Determines Recoil (positive)
        //Determines Damage (positive)
        //Determines Projectile Size
        //Determines Projectile Gravity

        BODYMOD,
        //Determines a specialtychance and specialtytype [usually empty]

        BARREL,
        //Determines Bullets per shot
        //Determines Cost per bullet
        //Determines ShotsPerSecond
        //Determines BaseBulletSpread
        //Determines Recoil (positive)
        //Determines Damage (negative)

        BARRELMOD,
        //Determines a specialtychance and specialtytype [usually empty]

        STOCK,
        //Determines Recoil (negative)
        //Determines BaseBulletSpread

        SIGHT,
        //Determines zoom type
        //Determines zoom amount

        SIGHTMOD,
        //Determines what variables are seen on hud
        //Determines a specialtychance and specialtytype [usually empty] when scoped

        CAPACITOR,
        //Determines clip/magazine size

        COUNT//Var for array initialization
    }

    public GunPart[] gunParts = new GunPart[(int)GunPartType.COUNT];

    public GunPart.Rarity rarity { get { int returnValue = 0; foreach (GunPart part in gunParts) { returnValue += (int)part.rarity; } returnValue /= gunParts.Length; return (GunPart.Rarity)returnValue; } }
    public int ClipSize { get { int returnValue = 0; foreach (GunPart part in gunParts) { returnValue += part.baseClipSize; } return returnValue; } }
    public int AmmoSize { get { int returnValue = 0; foreach (GunPart part in gunParts) { returnValue += part.baseAmmoSize; } return returnValue; } }
    public int BulletsPerShot { get { int returnValue = 0; foreach (GunPart part in gunParts) { returnValue += part.baseBulletsPerShot; } return returnValue; } }
    public float CostPerShot { get { float returnValue = 0; foreach (GunPart part in gunParts) { returnValue += part.baseCostPerShot; } return returnValue; } }
    public float ShotsPerSecond { get { float returnValue = 0; foreach (GunPart part in gunParts) { returnValue += part.baseShotsPerSecond; } return returnValue; } }
    public List<float> SpecialtyChance { get { List<float> returnValue = new List<float>(); foreach (GunPart part in gunParts) { returnValue.Add(part.baseSpecialtyChance); } return returnValue; } }
    public List<GunPart.Specialty> SpecialtyIndicies { get { List<GunPart.Specialty> returnValue = new List<GunPart.Specialty>(); foreach (GunPart part in gunParts) { returnValue.Add(part.baseSpecialtyIndex); } return returnValue; } }
    public Vector3 Recoil { get { Vector3 returnValue = Vector3.zero; foreach (GunPart part in gunParts) { returnValue += part.baseRecoil; } return returnValue; } }
    public float BulletSpread { get { float returnValue = 0; foreach (GunPart part in gunParts) { returnValue += part.baseBulletSpread; } return returnValue; } }
    public float ReloadSpeed { get { float returnValue = 0; foreach (GunPart part in gunParts) { returnValue += part.baseReloadSpeed; } return returnValue; } }
    public float DamagePerBullet { get { float returnValue = 0; foreach (GunPart part in gunParts) { returnValue += part.baseDamagePerBullet; } return returnValue; } }
    public float ProjectileSpeed { get { float returnValue = 0; foreach (GunPart part in gunParts) { returnValue += part.baseProjectileSpeed; } return returnValue; } }
    public float ProjectileSize { get { float returnValue = 1; foreach (GunPart part in gunParts) { returnValue += part.baseProjectileSize; } return returnValue; } }
    public float ProjectileGravity { get { float returnValue = 0; foreach (GunPart part in gunParts) { returnValue += part.baseProjectileGravity; } return returnValue; } }
    public GameObject Projectile { get { return gunParts.FirstOrDefault((part) => (part.projectile != null)).projectile; } }

    public int bulletsInClip = 0;

    public float cooldownTimer;
    public bool isReloading;


    public void FixedUpdate()
    {
        cooldownTimer += Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            if(bulletsInClip > 0)
            {
                AttemptFireGun();
            }
            else
            {
                AttemptReloadGun();
            }
        }
    }

    public void AttemptFireGun()
    {
        if (cooldownTimer > 1 / ShotsPerSecond)
        {
            for (int i = 0; i < Mathf.Min(bulletsInClip,BulletsPerShot); i++)
            {
                FireGun();
            }

            bulletsInClip -= Mathf.Min(bulletsInClip, BulletsPerShot);

            cooldownTimer = 0;
        }
    }
    public void AttemptReloadGun()
    {
        if(!isReloading)
        {
            isReloading = true;
            cooldownTimer = 0;
        }
        else
        {
            if(cooldownTimer > ReloadSpeed)
            {
                isReloading = false;
                bulletsInClip = ClipSize;
            }
        }
    }

    public void FireGun()
    {

        GameObject child = Instantiate<GameObject>(Projectile);
        child.transform.position = transform.position + transform.forward;
        child.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * ProjectileSpeed * 100);
        child.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread, BulletSpread)));

        foreach (var item in gunParts)
        {
            if(item.muzzleFlashPrefab)
            {
                Instantiate(item.muzzleFlashPrefab, transform);
            }
        }
    }
}
