using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class houseDoor : MonoBehaviour
{
    private moneyManager moneyManager;

    public Animator animator;
    public GameObject theText;

    private bool isPlayer = false;
    private float cost = 300f;
    private bool isOpened = false;

    private void Start()
    {
        moneyManager = GameObject.Find("MoneyManager").GetComponent<moneyManager>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && isPlayer && moneyManager.money >= cost && !isOpened)
        {
            moneyManager.extractMoney(cost);

            animator.SetTrigger("Start");
            isOpened = true;
            Destroy(theText);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isPlayer = true;

            if(theText != null)
                theText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayer = false;

            if(theText != null)
                theText.SetActive(false);
        }
    }
}
