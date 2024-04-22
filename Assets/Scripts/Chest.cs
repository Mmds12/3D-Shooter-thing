using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    // Assignable
    public Animation Animation;
    public Transform spawnPoint;
    public Transform endPoint;
    public GameObject[] chestGuns;
    public WeaponManager weaponManager;
    public TextMeshProUGUI buyWeaponText;

    public GameObject[] gunIcons;
    public GameObject image1;
    public GameObject image2;
    public GameObject pistolImage;
    private GameObject t1;
    private GameObject t2;
    private moneyManager moneyManager;

    private GameObject chestGun;
    public GameObject gun1;
    public GameObject gun2;

    public string weaponName;
    public float chestCost = 950f;

    public float moveSpeed = 5f;
    private float startTime;
    private float journeyLength;

    private bool inRange = false;
    private bool isGun = false;
    private bool isOpen = false;
    private bool opening = false;
    private bool isThereWeapon = false;
    private bool firstWeapon = true;

    public bool reloading = false;

    // Weapons Name
    string UMP = "UMP-45(Clone)";
    string AK = "Ak-47 (1)(Clone)";
    string M4A1 = "M4A1 Sopmod(Clone)";
    string Shotgun_M401 = "Shotgun M401(Clone)";

    private void Start()
    {
        journeyLength = Vector3.Distance(spawnPoint.position, endPoint.position);

        moneyManager = GameObject.Find("MoneyManager").GetComponent<moneyManager>();
    }

    private void Update()
    {
        myInput();

        if(isGun)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;

            float fractionOfJourney = distanceCovered / journeyLength;

            chestGun.transform.position = Vector3.Lerp(spawnPoint.position, endPoint.position, fractionOfJourney);
        }
    }

    void myInput()
    {
        // Open the chest
        if (Input.GetKeyDown(KeyCode.E) && inRange && !isOpen && moneyManager.money >= chestCost)
        {
            isOpen = true;
            Animation.Play("ChestAnim");
            StartCoroutine(spawnWeapon());
        }

        // Take the weapon
        if(Input.GetKeyDown(KeyCode.E) && inRange && isThereWeapon && !reloading)
        {
            takeWeapon();
            opening = false;
        }
    }

    IEnumerator spawnWeapon()
    {
        buyWeaponText.gameObject.SetActive(false);
        opening = true;

        yield return new WaitForSeconds(2.2f);
        isGun = true;
        int s = Random.Range(0, chestGuns.Length);
        chestGun = Instantiate(chestGuns[s], spawnPoint.transform.position, spawnPoint.transform.rotation);
        yield return new WaitForSeconds(1f);
        isThereWeapon = true;
    }

    void takeWeapon()
    {
        Animation.Play("ChestAnimClose");

        if(firstWeapon)
        {
            weaponManager.switchSlots(1);
            weaponManager.hasWeapon = true;
            firstWeapon = false;
        }
        else
        {

            if (weaponManager.selectedSlot == 1)
                Destroy(gun1);

            else if (weaponManager.selectedSlot == 2)
                Destroy(gun2);

        }

        int s = weaponManager.selectedSlot;

        if (s == 1 && t1 != null)
            Destroy(t1);
        else if (s == 2 && t2 != null)
            Destroy(t2);

        if (s == 2 && pistolImage != null)
            Destroy(pistolImage);

        if (chestGun.transform.name == UMP)
        {
            if(s == 1)
            {
                gun1 = Instantiate(weaponManager.guns[0], weaponManager.weaponSlot1.transform);
                t1 = Instantiate(gunIcons[3], image1.transform);
            }
            if (s == 2)
            {
                gun2 = Instantiate(weaponManager.guns[0], weaponManager.weaponSlot2.transform);
                t2 = Instantiate(gunIcons[3], image2.transform);
            }
        }

        else if (chestGun.transform.name == AK)
        {
            if (s == 1)
            {
                gun1 = Instantiate(weaponManager.guns[1], weaponManager.weaponSlot1.transform);
                t1 = Instantiate(gunIcons[0], image1.transform);
            }
            if (s == 2)
            {
                gun2 = Instantiate(weaponManager.guns[1], weaponManager.weaponSlot2.transform);
                t2 = Instantiate(gunIcons[0], image2.transform);
            }
        }

        else if (chestGun.transform.name == M4A1)
        {
            if (s == 1)
            {
                gun1 = Instantiate(weaponManager.guns[2], weaponManager.weaponSlot1.transform);
                t1 = Instantiate(gunIcons[1], image1.transform);
            }
            if (s == 2)
            {
                gun2 = Instantiate(weaponManager.guns[2], weaponManager.weaponSlot2.transform);
                t2 = Instantiate(gunIcons[1], image2.transform);
            }
        }

        else if (chestGun.transform.name == Shotgun_M401)
        {
            if (s == 1)
            {
                gun1 = Instantiate(weaponManager.guns[3], weaponManager.weaponSlot1.transform);
                t1 = Instantiate(gunIcons[2], image1.transform);
            }
            if (s == 2)
            {
                gun2 = Instantiate(weaponManager.guns[3], weaponManager.weaponSlot2.transform);
                t2 = Instantiate(gunIcons[2], image2.transform);
            }
        }

        isThereWeapon = false;
        isOpen = false;
        isGun = false;
        Destroy(chestGun);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            if(!opening)
                buyWeaponText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            buyWeaponText.gameObject.SetActive(false);
        }
    }
}
