using System.Collections;
using TMPro;
using UnityEngine;

public class Rounds : MonoBehaviour
{
    private Animator animator;
    private TextMeshProUGUI roundsCounter;

    public bool waiting = false;
    public float roundEnemy = 20f;
    public float enemySpawned = 0f;
    public float enemyDead = 0f;

    private float waitingTime = 8f;
    private float round = 0f;
    private bool nextRound_ = false;

    
    private void Start()
    {
        animator = GetComponent<Animator>();
        roundsCounter = GameObject.Find("RoundsCounterText").GetComponent<TextMeshProUGUI>();

        StartCoroutine(nextRound());
    }

    void Update()
    {
        if(enemySpawned >= roundEnemy)
        {
            waiting = true;
            if(enemyDead >= roundEnemy && !nextRound_)
            {
                StartCoroutine(nextRound());
            }
        }
    }

    IEnumerator nextRound()
    {
        round++;
        nextRound_ = true;

        gameObject.GetComponent<TextMeshProUGUI>().text = "Round " + round;
        roundsCounter.text = "R: " + round;
        animator.SetTrigger("Start");

        enemySpawned = 0f;
        enemyDead = 0f;

        yield return new WaitForSeconds(waitingTime);

        animator.SetTrigger("End");
        waiting = false;
        nextRound_ = false;

        roundEnemy += 3f;
    }
}
