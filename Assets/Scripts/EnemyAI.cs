using UnityEngine.AI;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Animation Animation;
    public GameObject player;

    private EnemySpawn EnemySpawn;
    private playerManager playerManager;
    private moneyManager moneyManager;

    private bool isAttacking = false;
    private bool attacked = false;

    public float spawnTime = 8f;
    public float shootingDistance = 2f;

    private float damageToPlayer = 13f;
    private float health = 100f;
    private bool isDead = false;

    private float Distance;
    private float timeBetweenAttack = 1.2f;

    private void Start()
    {
        EnemySpawn = GameObject.Find("EnemySpawner").GetComponent<EnemySpawn>();
        player = GameObject.Find("Player");
        playerManager = player.GetComponent<playerManager>();
        moneyManager = GameObject.Find("MoneyManager").GetComponent<moneyManager>();
    }

    private void Update()
    {
        // The distance between player and enemy
        Distance = Vector3.Distance(player.transform.position, gameObject.transform.position);

        if (!isDead)
        {

            // Chase player
            if (EnemySpawn.isRunning)
            {
                Animation.Play("Walk");
                gameObject.GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
                if (Distance <= shootingDistance)
                {
                    EnemySpawn.isRunning = false;
                    isAttacking = true;
                }
            }

            // Attack player
            if (isAttacking)
            {
                timeBetweenAttack -= Time.deltaTime;

                gameObject.GetComponent<NavMeshAgent>().SetDestination(gameObject.transform.position);
                Animation.Play("Attack1");
                gameObject.transform.LookAt(player.transform.position);

                if(!attacked && timeBetweenAttack <= 0)
                    StartCoroutine(playerDamage());

                if (Distance > shootingDistance)
                {
                    isAttacking = false;
                    EnemySpawn.isRunning = true;
                }
            }

        }
    }

    public void Damage(int d, float money)
    {
        health -= d;

        moneyManager.addMoney(money);

        if (health <= 0 && !isDead)
        {
            // Enemy Death
            isDead = true;
            gameObject.GetComponent<NavMeshAgent>().SetDestination(gameObject.transform.position);
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            Animation.Play("Death");
            Destroy(gameObject, 4f);
        }
    }

    IEnumerator playerDamage()
    {
        timeBetweenAttack = 1.2f;
        attacked = true;
        yield return new WaitForSeconds(.5f);

        playerManager.takeDamage(damageToPlayer);

        yield return new WaitForSeconds(1f);
        attacked = false;
    }
}
