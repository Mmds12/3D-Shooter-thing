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
    private moneyManager moneyManager;

    private GameObject chestGun;
    public GameObject pistol;

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

    public bool reloading = false;

    private void Start()
    {
        journeyLength = Vector3.Distance(spawnPoint.position, endPoint.position);

        moneyManager = GameObject.Find("MoneyManager").GetComponent<moneyManager>();

        weaponManager.gun2 = pistol;

        weaponManager.firstWeapon = true;
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

            moneyManager.extractMoney(chestCost);
        }

        // Take the weapon
        if(Input.GetKeyDown(KeyCode.E) && inRange && isThereWeapon && !reloading && isOpen)
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

        if (weaponManager.firstWeapon)
        {
            weaponManager.switchSlots(1);
            weaponManager.hasWeapon = true;
            weaponManager.firstWeapon = false;
        }
        else
        {

            if (weaponManager.selectedSlot == 1 && weaponManager.gun1 != null)
                Destroy(weaponManager.gun1);

            else if (weaponManager.selectedSlot == 2 && weaponManager.gun2 != null)
                Destroy(weaponManager.gun2);

        }

        int s = weaponManager.selectedSlot;

        if (s == 1 && weaponManager.t1 != null)
            Destroy(weaponManager.t1);
        else if (s == 2 && weaponManager.t2 != null)
            Destroy(weaponManager.t2);

        if (s == 2 && pistolImage != null)
            Destroy(pistolImage);

        string GunName = chestGun.transform.name;

        switch (GunName)
        {
            case "UMP-45(Clone)":
                InstantiateWeapon(weaponManager.guns[0], gunIcons[3], s);
                break;
            case "Ak-47 (1)(Clone)":
                InstantiateWeapon(weaponManager.guns[1], gunIcons[0], s);
                break;
            case "M4A1 Sopmod(Clone)":
                InstantiateWeapon(weaponManager.guns[2], gunIcons[1], s);
                break;
            case "Shotgun M401(Clone)":
                InstantiateWeapon(weaponManager.guns[3], gunIcons[2], s);
                break;
            default:
                Debug.LogWarning("Unknown chest gun name: " + GunName);
                break;
        }
        

        isThereWeapon = false;
        isOpen = false;
        isGun = false;

        Destroy(chestGun);
    }


    void InstantiateWeapon(GameObject weaponPrefab, GameObject iconPrefab, int slot)
    {
        if (slot == 1)
        {
            weaponManager.gun1 = Instantiate(weaponPrefab, weaponManager.weaponSlot1.transform);
            weaponManager.t1 = Instantiate(iconPrefab, image1.transform);
        }
        else if (slot == 2)
        {
            weaponManager.gun2 = Instantiate(weaponPrefab, weaponManager.weaponSlot2.transform);
            weaponManager.t2 = Instantiate(iconPrefab, image2.transform);
        }
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
