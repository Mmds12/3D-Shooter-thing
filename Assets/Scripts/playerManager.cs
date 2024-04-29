using System.Collections;
using TMPro;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI gameOver;

    public GameObject gun;
    public GameObject camera_;

    public float health = 100;
    public float healLimit = 100f;
    public float heal = 5;
    public float healCouldown = 8f;

    private float healCounter = 0f;

    public bool isDead = false;
    private bool healing = false;

    // Damage Overlay
    public GameObject overlaySpawn;
    public GameObject overlay1;
    public GameObject overlay2;
    public GameObject overlay3;
    private GameObject overlay11;
    private GameObject overlay22;
    private GameObject overlay33;
    private bool isOverlay1 = false;
    private bool isOverlay2 = false;
    private bool isOverlay3 = false;

    // Others
    public bool weaponReloading;

    private void Update()
    {
        healCounter += Time.deltaTime;

        if(healCounter >= healCouldown)
        {
            if(health < healLimit && !healing)
            {
                StartCoroutine(Heal());
            }
        }
        healthText.text = "HP: " + health.ToString();
    }


    public void takeDamage(float damage)
    {
        healCounter = 0f;

        if(health - damage <= 0)
        {
            health = 0;
            healthText.text = health.ToString();
            isDead = true;

            death();
            return;
        }

        health -= damage;
        healthText.text = health.ToString();


        // ****** Overlay Management ******

        if (health < healLimit && health >= 80f && !isOverlay1)
            overlay11 = Instantiate(overlay1, overlaySpawn.transform);

        if (health < 70f && health >= 50f && !isOverlay2)
        {
            overlay22 = Instantiate(overlay2, overlaySpawn.transform);
            isOverlay2 = true;
        }

        if (health < 35 && health >= 0f && !isOverlay3)
        {
            overlay33 = Instantiate(overlay3, overlaySpawn.transform);
            isOverlay3 = true;
        }

        isOverlay1 = true;

        // ************************
    }

    private void death()
    {
        gameOver.gameObject.SetActive(true);

        gameObject.GetComponent<PlayerMovement>().enabled = false;
        camera_.GetComponent<PlayerLook>().enabled = false;
        gun.SetActive(false);

        GameObject.Find("GameManager").GetComponent<GameManager>().GameStop();
    }

    IEnumerator Heal()
    {
        healing = true;

        if (health + heal >= healLimit)
        {
            health = healLimit;

            isOverlay1 = false;
            overlay11.gameObject.GetComponent<Animator>().SetTrigger("End");
        }

        else
            health += heal;

        if(health >= 35f)
        {
            overlay22.gameObject.GetComponent<Animator>().SetTrigger("End");
            isOverlay2 = false;
        }

        if (health >= 60f)
        {
            overlay33.gameObject.GetComponent<Animator>().SetTrigger("End");
            isOverlay3 = false;
        }

        yield return new WaitForSeconds(.8f);
        healing = false;
    }
}
