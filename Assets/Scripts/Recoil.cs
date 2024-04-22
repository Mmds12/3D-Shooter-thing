using Unity.VisualScripting;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    public GunSystem GunSystem;

    private Vector3 currentRot;
    private Vector3 targetRot;

    [Header("Settings")]
    public float snapping;
    public float returnSpeed;

    private void Start()
    {
        GunSystem = GameObject.FindGameObjectWithTag("Weapon").GetComponent<GunSystem>();
    }

    void Update()
    {
        targetRot = Vector3.Lerp(targetRot, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRot = Vector3.Slerp(currentRot, targetRot, snapping * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRot);
    }

    public void RecoilStart()
    {
        if(GunSystem.isaAiming || PlayerMovement.isCrouching)
            targetRot += new Vector3(GunSystem.aimRecoilX, Random.Range(-GunSystem.aimRecoilY, GunSystem.aimRecoilY), Random.Range(-GunSystem.aimRecoilZ, GunSystem.aimRecoilZ));
        else
            targetRot += new Vector3(GunSystem.recoilX, Random.Range(-GunSystem.recoilY, GunSystem.recoilY), Random.Range(-GunSystem.recoilZ, GunSystem.recoilZ));
    }

}
