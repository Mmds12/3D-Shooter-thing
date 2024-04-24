using System.Collections;
using UnityEngine;

public class Katana : MonoBehaviour
{
    public Animator gunHolder;

    public GameObject katanaPrefab1;
    public GameObject katanaPrefab2;
    public GameObject katanaSpawnPoint;
    //public GameObject lightningBolt;

    private GameObject katana;
    //private GameObject l;
    //private bool lightning = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(Attack());
        }

        //if (lightning)
        //{
        //    l.transform.position = new Vector3(katana.transform.position.x, katana.transform.position.y, katana.transform.position.z);
        //}
    }

    IEnumerator Attack()
    {
        int temp = Random.Range(1, 11);

        gunHolder.SetBool("KatanaAttack", true);

        yield return new WaitForSeconds(.2f);

        if (temp >= 5)
            katana = Instantiate(katanaPrefab1, katanaSpawnPoint.transform);
        else
            katana = Instantiate(katanaPrefab2, gameObject.transform);

        //l = Instantiate(lightningBolt);
        //lightning = true;

        yield return new WaitForSeconds(.18f);

        //lightning = false;
        gunHolder.SetBool("KatanaAttack", false);
        Destroy(katana);
        //Destroy(l);
    }
}
