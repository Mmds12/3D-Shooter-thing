using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Machine : MonoBehaviour
{
    public GameObject theText;

    public float machine;
    public float cost;

    private moneyManager moneyManager;

    private bool isPlayer = false;

    // Weapon Upgrade
    public GameObject waeponSlot1;
    public GameObject waeponSlot2;
    public GameObject gunHolder;

    private GunSystem gunSystem;
    private WeaponManager weaponManager;
    private Renderer rend;
    private Material mat;

    private void Start()
    {
        moneyManager = GameObject.Find("MoneyManager").GetComponent<moneyManager>();
        weaponManager = gunHolder.GetComponent<WeaponManager>();

        if(machine == 3)
        {
            theText.GetComponent<TextMeshProUGUI>().text = "   Press E to Buy\nWeapon Upgrade  \n   $" + cost.ToString();
        }
    }

    void Update()
    {
        if(isPlayer)
        {
            // Take the Gun Stats
            if (weaponManager.selectedSlot == 1)
                gunSystem = waeponSlot1.GetComponentInChildren<GunSystem>();
            else if (weaponManager.selectedSlot == 2)
                gunSystem = waeponSlot2.GetComponentInChildren<GunSystem>();
        }


        if(Input.GetKeyDown(KeyCode.E) && isPlayer && moneyManager.money >= cost)
        {
            if (machine == 1)
                healFaster();

            else if (machine == 2)
                movementSpeed();

            else if (machine == 3 && gunSystem.weaponUpgrade < 3)
                weaponUpgrade();


            if(machine != 3)
                Destroy(theText);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayer = true;

            if (theText != null && gunSystem.weaponUpgrade < 3)
                theText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayer = false;

            if (theText != null)
                theText.SetActive(false);
        }
    }


    void healFaster()
    {
        moneyManager.extractMoney(cost);

        GameObject.Find("Player").GetComponent<playerManager>().healLimit += 20f;
        GameObject.Find("Player").GetComponent<playerManager>().healCouldown = 5f;
        GameObject.Find("Player").GetComponent<playerManager>().heal = 8f;
    }
    void movementSpeed()
    {
        moneyManager.extractMoney(cost);

        GameObject.Find("Player").GetComponent<PlayerMovement>().moveSpeed = 15.3f;
    }

    void weaponUpgrade()
    {
        moneyManager.extractMoney(cost);

        if(gunSystem.timeBetweenShots > .04f)
            gunSystem.timeBetweenShots -= .025f;

        gunSystem.damage += 5;
        gunSystem.magazine += 5f;
        gunSystem.recoilX += .3f;
        gunSystem.recoilY -= .1f;

        gunSystem.weaponUpgrade++;

        if (gunSystem.weaponUpgrade == 0) cost = 1000f;
        else if (gunSystem.weaponUpgrade == 1) cost = 2500f;
        else if (gunSystem.weaponUpgrade == 2) cost = 3500f;

        theText.GetComponent<TextMeshProUGUI>().text = "   Press E to Buy\nWeapon Upgrade  \n   $" + cost.ToString();

        // Change the color
        if(gunSystem.gameObject.GetComponent<Renderer>())
            rend = gunSystem.gameObject.GetComponent<Renderer>();
        else
            rend = gunSystem.gameObject.GetComponentInChildren<Renderer>();

        mat = rend.material;

        Color newColor;

        if (gunSystem.weaponUpgrade == 1)
            newColor = Color.HSVToRGB(8f / 360f, 65f / 100f, 100f / 100f);
        else if (gunSystem.weaponUpgrade == 2)
            newColor = Color.HSVToRGB(228f / 360f, 70f / 100f, 100f / 100f);
        else
            newColor = Color.yellow;

        mat.color = newColor;

        gunSystem.Reload();
    }
}
