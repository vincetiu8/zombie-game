using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentGun : MonoBehaviour
{
    //Ideally all weapons in the game should inherit from this, the script is designed to be as vauge as possible so things like how a weapon should fire is intentially not detailed here.


    [HideInInspector] public float currentDamage, currentMaxAmmo, currentMagSize, currentFireRate, currentReload, totalAmmoLeft, leftInMag;
    private bool canShoot;

    [SerializeField] protected string gunName; // What name shows up when you see it in game and hold it
    [SerializeField] protected string gunDescription;// Description you read before/after you buy the gun
    [SerializeField] protected float bulletVelocity; //Could be bunched with the other weapon attributes but i've never seen any gun that has it's bullet speed change when upgraded

    private int currentTier = 0;

    public string[] effects; // is a list so that the number of effects can be infinitely large if need be
    // example effects: fire, electrify, freeze, penetrate, water, knockback

    public class WeaponAttributes
    {
        public float damage, ammo, magazineSize, fireRate, reload;
        /* Just for clarification:
         * damage: the amount of damage your weapon deals per shot
         * ammo: the TOTAL amount of ammo weapon has, ie. if it's 0 your gun can shoot no more bullets
         * magazineSize: how many rounds weapon has before having to reload
         * fireRate: how frequently weapon is allowed to fire
         * reload: how many seconds it takes to reload weapon
         
         */


        /*public void OnHit(float _damage, Vector2 _knockback, List<string> _effects) //This method would ideally be in the health controller but i want to see if you would agree with how i want to do things first
        {
            Should be universal for both enemies for player when theyr'e hit with something
            
            -   The knoback is there because different gun/zombies will have different amounts of knock back, plus every form of attack in this game should apply knockback anyways
                is a vector 2 so both direction and magnitude can easily be set, (some zombies may pull player in toawrds them instead of knocking them away)
        
            -   The list is there becasue in the game theres a lot of special status effects that can be applied,
                for example, some zombies light the player on fire
                player can also buy multiple perks that influence this such as, flame bullets and frozen bullets, both of which applies special effects on hit, so i chose a list becuase it is easily extendable
        }*/



        public WeaponAttributes(float dmg, float amo, float magsiz, float frate, float re)
        {
            damage = dmg;
            ammo = amo;
            magazineSize = magsiz;
            fireRate = frate;
            reload = re;
        }

        public void Display()
        {
            //float[] yee = {damage, ammo, clipAmmo, fireRate, reload};
            string result = "Damage: " + damage.ToString() + ", Ammo: " + ammo.ToString() + ", Mag Size: " + ammo.ToString() + ", Fire Rate: " + fireRate.ToString() + ", Reload Time: " + reload.ToString();
            /*foreach (var item in yee)
            {
                result += item.ToString() + ", ";
            }*/
            Debug.Log(result);
        }
    }

    public List<WeaponAttributes> WeaponTiers = new List<WeaponAttributes>();

    protected virtual void GunStats() // use this to define your weapon's base and upgraded attributes, override when writing your own weapon scripts
    {
        AddWeaponTier(1, 2, 3, 4, 5); // This will be the base weapon attribute
        AddWeaponTier(2, 4, 6, 8, 10); // This will be ultimate weapon I
        AddWeaponTier(4, 8, 12, 16, 20);// This will be ultimate weapon II

        currentDamage = WeaponTiers[0].damage;
        currentMaxAmmo = WeaponTiers[0].ammo;
        currentMagSize = WeaponTiers[0].magazineSize;
        currentFireRate = WeaponTiers[0].fireRate;
        currentReload = WeaponTiers[0].reload;

        //Debug.Log(currentReload);
        Debug.Log(WeaponTiers.Count);


    }
    //the way damage is delt this game universally, should be damage(damageDelt, effects)
    void Start()
    {
        GunStats();
        WeaponTiers[0].Display();

        UpgrageWeapon(0, 0, 0, 0, 0);
    }

    void AddWeaponTier(float damage, float ammo, float magazineSize, float fireRate, float reload)
    {

        //float[] newTier = {damage, ammo, clipAmmo, fireRate, reload};
        WeaponTiers.Add(new WeaponAttributes(damage, ammo, magazineSize, fireRate, reload));
        //Debug.Log("yee" + WeaponTiers[0].clipAmmo);
        //WeaponTiers[0].Display();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpgrageWeapon(float damage, float ammo, float clipAmmo, float fireRate, float reload)
    {
        if (currentTier >= (WeaponTiers.Count - 1))
        {
            Debug.Log("over");
            return;
        }

        currentTier = currentTier + 1;

        gunName = gunName + " I";
        currentDamage = WeaponTiers[currentTier].damage;
        currentMaxAmmo = WeaponTiers[currentTier].ammo;
        currentMagSize = WeaponTiers[currentTier].magazineSize;
        currentFireRate = WeaponTiers[currentTier].fireRate;
        currentReload = WeaponTiers[currentTier].reload;

        MaxAmmo();

        WeaponTiers[currentTier].Display();


    }



    public void Reload()
    {
        if (leftInMag == currentMagSize) return;
        //play animation
        ShootDelay(currentReload);
        totalAmmoLeft = totalAmmoLeft - leftInMag;
        leftInMag = currentMagSize;

    }

    protected virtual void Fire()
    {
        if (leftInMag == 0)
        {
            Debug.Log("No ammo");
            // play no ammo sound
            return;
        }

        leftInMag = leftInMag - 1;
        // need to be affected by quickfire perk
        ShootDelay(currentFireRate);
        // damamge should be (damage, knockback, Send effects list)
    }

    private IEnumerator ShootDelay(float waitTime)
    {
        canShoot = false;
        yield return new WaitForSeconds(waitTime);
        canShoot = true; // reloading between shots might cause a bug in this

    }

    //Perks activation section
    public void MaxAmmo()
    {
        totalAmmoLeft = currentMaxAmmo;
        leftInMag = currentMagSize;
    }


}
