using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Katana : MonoBehaviour
{
    public Animator gunHolder;

    public GameObject katanaSpawnPoint;

    public GameObject katanaPrefab1;
    public GameObject particle1;

    public GameObject katanaPrefab2;
    public GameObject particle2;

    public GameObject cam;
    public LayerMask enemyLayer;

    private GameObject katana;
    private playerManager playerManager;

    private float attackRange = 3.8f;
    private bool attacking = false;


    private void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<playerManager>();
    }

    void Update()
    {
        if((Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.F)) && !attacking && !playerManager.weaponReloading)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        int temp = Random.Range(1, 11);
        attacking = true;

        gunHolder.SetBool("KatanaAttack", true);
        GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(.2f);

        GameObject p;
        if (temp >= 5)
        {
            katana = Instantiate(katanaPrefab1, katanaSpawnPoint.transform);
            p = Instantiate(particle2, gameObject.transform);
        }
        else
        {
            katana = Instantiate(katanaPrefab2, gameObject.transform);
            p = Instantiate(particle1, gameObject.transform);
        }

        yield return new WaitForSeconds(.18f);

        // Enemy Damage
        enemyDamage();

        attacking = false;
        gunHolder.SetBool("KatanaAttack", false);
        Destroy(katana);
        Destroy(p);
    }

    void enemyDamage()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackRange, enemyLayer))
        {
            hit.collider.gameObject.GetComponent<EnemyAI>().Damage(65, 25f);
        }
    }
}