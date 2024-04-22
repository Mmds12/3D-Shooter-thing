using System.Collections;
using TMPro;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI gameOver;

    public GameObject gun;
    public GameObject camera_;

    private float health = 100;
    private float heal = 5;
    private float healCounter = 0f;
    private float healCouldown = 8f;

    public bool isDead = false;
    private bool healing = false;

    private void Update()
    {
        healCounter += Time.deltaTime;
        if(healCounter >= healCouldown)
        {
            if(health < 100 && !healing)
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
    }

    private void death()
    {
        gameOver.gameObject.SetActive(true);

        gameObject.GetComponent<PlayerMovement>().enabled = false;
        camera_.GetComponent<PlayerLook>().enabled = false;
        gun.SetActive(false);
    }

    IEnumerator Heal()
    {
        healing = true;

        if (health + heal >= 100)
            health = 100;
        else
            health += heal;

        yield return new WaitForSeconds(.8f);
        healing = false;
    }
}
