using System.Collections;
using UnityEngine;
using TMPro;

public class WeaponScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public int maxAmmo;
    public bool isReloading = false;
    public int CurrentAmmo1;
    public int CurrentAmmo2;
    
    public WeaponSwap ws;
    
    public Camera fpsCam;
    public ParticleSystem muzzleFlashSMG;
    public GameObject impactEffect;
    public TextMeshProUGUI ammoText;
    public Animator animatorPistol;

    void Start()
    {
        CurrentAmmo1 = 12;
        CurrentAmmo2 = 30;
        animatorPistol = GetComponent<Animator>();
    }
    void Update()
    {
        Debug.Log(ws.currentWeaponIndex);
        
        if (ws.currentWeaponIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.R) && CurrentAmmo1 != 12) 
            {
                StartCoroutine(Reload());
            }
            ammoText.text = "Ammo: " + CurrentAmmo1.ToString();
        }
        
        if (ws.currentWeaponIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.R) && CurrentAmmo2 != 30) 
            {
                StartCoroutine(Reload());
            }
            ammoText.text = "Ammo: " + CurrentAmmo2.ToString();
        }
        
        if (isReloading) return;
        
        if (Input.GetButtonDown("Fire1"))
        {
            if (CurrentAmmo1 > 0 && CurrentAmmo2 > 0)
            {
                Shoot();
            }
            else
            {
                StartCoroutine(Reload());
            }
        }
    }

    void Shoot()
    {
        if (muzzleFlashSMG != null)
        {
            muzzleFlashSMG.Play();
        }

        if (ws.currentWeaponIndex == 0)
        {
            CurrentAmmo1 -= 1;
            
            if (CurrentAmmo1 <= 0)
            {
                StartCoroutine(Reload());
            }
        }

        if (ws.currentWeaponIndex == 1)
        {
            CurrentAmmo2 -= 1;
            
            if (CurrentAmmo2 <= 0)
            {
                StartCoroutine(Reload());
            }
        }
        
        

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (impactEffect != null)
            {
                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 2f);
            }
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        if (ws.currentWeaponIndex == 0) // Pistol
        {
            yield return new WaitForSeconds(1f); 
            CurrentAmmo1 = 12;
        }
        else if (ws.currentWeaponIndex == 1) // SMG
        {
            yield return new WaitForSeconds(1f);
            CurrentAmmo2 = 30;
        }

        isReloading = false;
    }
}
