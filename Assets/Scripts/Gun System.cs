using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class GunSystem : MonoBehaviour
{
    // Assignable
    public GameObject firePoint;
    public GameObject fpsCam;
    public LayerMask whatIsEnemy;

    // Bullet
    public RaycastHit rayHit;
    public TextMeshProUGUI magazineText;
    public TextMeshProUGUI fullMagazineText;

    public ParticleSystem muzzleFlash;
    private bool outOfAmmo = false;

    // Impact Effect
    public GameObject impactEffect_metal;
    public GameObject impactEffect_dirt;
    public GameObject impactEffect_wood;

    // Animation
    public Animator animator;

    // Bullet
    public GameObject bullet;
    public float bulletSpeed = 100f;

    // Gun Stats
    public int damage;
    public float spread, magazine, fullMagazine, timeBetweenShots, reloadTime, range, bulletsPerTap;
    public bool isAllowedToHold;

    // Reloading
    private float currentBullets;
    private bool Shooting, readyToShoot;
    public bool Reloading = false;

    // Scripts
    private Recoil recoil_script;
    private EnemyAI Enemy_script;

    // Aiming
    public static bool isaAiming = false;
    public GameObject crosshair;

    [Header("Recoil")]
    // Recoil
    [Header("Normal")]
    public float recoilX;
    public float recoilY;
    public float recoilZ;

    [Header("Aim")]
    public float aimRecoilX;
    public float aimRecoilY;
    public float aimRecoilZ;

    private Chest chest1;
    private Chest chest2;

    // Sound Effect
    public AudioSource Sound_Fire;
    public AudioSource Sound_Shell;
    public AudioSource Sound_Reload;

    // Others
    private playerManager playerManager;
    public float weaponUpgrade = 0f;

    private void Start()
    {
        currentBullets = magazine;
        readyToShoot = true;
        recoil_script = GameObject.Find("Player/CameraRecoil").GetComponent<Recoil>();
        fpsCam = GameObject.Find("Main Camera");

        // Bullet things
        GameObject mt = GameObject.Find("MagazineText");
        magazineText = mt.GetComponent<TextMeshProUGUI>();

        GameObject fmt = GameObject.Find("FullMagazineText");
        fullMagazineText = fmt.GetComponent<TextMeshProUGUI>();
        

        crosshair = GameObject.Find("Crosshair");

        chest1 = GameObject.Find("Chest1").GetComponent<Chest>();
        chest2 = GameObject.Find("Chest2").GetComponent<Chest>();

        playerManager = GameObject.Find("Player").GetComponent<playerManager>();
    }

    private void Update()
    {
        myInput();

        if(playerManager != null)
            playerManager.weaponReloading = Reloading;
    }

    void myInput()
    {
        if(isAllowedToHold)
            Shooting = Input.GetKey(KeyCode.Mouse0);
        else
            Shooting = Input.GetKeyDown(KeyCode.Mouse0);

        // Aiming
        if (Input.GetKey(KeyCode.Mouse1) && !outOfAmmo)
            AimStart();
        else
            AimEnd();

        // Shooting
        if (Shooting && readyToShoot && !Reloading && currentBullets > 0)
            Shoot();

        // Reload
        if (currentBullets <= 0 && fullMagazine > 0 || Input.GetKeyDown(KeyCode.R) && currentBullets < magazine)
            Reload();

        // Bullet text
        magazineText.text = currentBullets.ToString();
        fullMagazineText.text = fullMagazine.ToString();
    }

    void Shoot()
    {
        // Recoil
        recoil_script.RecoilStart();
        // Muzzle Flash
        muzzleFlash.Play();
        // Sound Effect
        StartCoroutine(soundEffect());

        // Spawn Bullet
        if(!isaAiming)
        {
            GameObject b = Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
            b.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
            Destroy(b, 3f);
        }

        readyToShoot = false;
        currentBullets--;

        if (currentBullets <= 0 && fullMagazine <= 0)
            outOfAmmo = true;


        for(int i = 0; i < bulletsPerTap; i++)
        {

            // Spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);
            Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

            // Raycast
            if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range))
            {
                // Damage
                if(!rayHit.transform.CompareTag("Player") && whatIsEnemy == (whatIsEnemy | (1 << rayHit.transform.gameObject.layer)))
                {
                    Enemy_script = rayHit.transform.GetComponent<EnemyAI>();
                    Enemy_script.Damage(damage, 14f);
                }

                // Impact Effect
                GameObject effect;

                if (rayHit.transform.CompareTag("MetalSurface")) effect = Instantiate(impactEffect_metal, rayHit.point, Quaternion.LookRotation(rayHit.normal));
                else if (rayHit.transform.CompareTag("WoodSurface")) effect = Instantiate(impactEffect_wood, rayHit.point, Quaternion.LookRotation(rayHit.normal));
                else if (!rayHit.transform.CompareTag("Player")) effect = Instantiate(impactEffect_dirt, rayHit.point, Quaternion.LookRotation(rayHit.normal));
                else effect = null;

                if(effect != null)
                    Destroy(effect, 2f);
            }
             
            Invoke("resetShooting", timeBetweenShots);

        }

    }

    public void Reload()
    {
        Reloading = true;
        chest1.reloading = true;
        chest2.reloading = true;
        Sound_Reload.Play();

        animator.SetTrigger("ReloadStart");
        animator.SetTrigger("ReloadEnd");

        Invoke("resetReload", reloadTime);

        float temp = magazine - currentBullets;
        if (fullMagazine - temp >= 0)
        {
            currentBullets = magazine;
            fullMagazine -= temp;
        }
        else
        {
            currentBullets += fullMagazine;
            fullMagazine = 0;
        }

        outOfAmmo = false;
    }

    void AimStart()
    {
        isaAiming = true;
        animator.SetBool("isAiming", true);
        crosshair.SetActive(false);
    }

    void AimEnd()
    {
        crosshair.SetActive(true);
        isaAiming = false;
        animator.SetBool("isAiming", false);
    }

    void resetReload()
    {
        Reloading = false;
        chest1.reloading = false;
        chest2.reloading = false;
    }

    void resetShooting()
    {
        readyToShoot = true;
    }

    IEnumerator soundEffect()
    {
        Sound_Fire.Play();
        yield return new WaitForSeconds(.2f);
        if (Sound_Shell != null)
            Sound_Shell.Play();
    }
}
