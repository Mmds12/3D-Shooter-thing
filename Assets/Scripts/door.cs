using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class door : MonoBehaviour
{
    public TextMeshProUGUI costText;
    public Animation anim;
    public Animation anim2;

    private EnemySpawn EnemySpawn;

    private moneyManager moneyManager;
    public float cost = 1000f;

    private bool inRange = false;
    private bool opened = false;

    private void Start()
    {
        EnemySpawn = GameObject.Find("EnemySpawner").GetComponent<EnemySpawn>();

        moneyManager = GameObject.Find("MoneyManager").GetComponent<moneyManager>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && inRange && !opened && moneyManager.money >= cost)
        {
            costText.gameObject.SetActive(false);
            anim.Play();
            anim2.Play();
            opened = true;
            EnemySpawn.gateOpened = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !opened)
        {
            inRange = true;
            costText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !opened)
        {
            inRange = false;
            costText.gameObject.SetActive(false);
        }
    }
}
