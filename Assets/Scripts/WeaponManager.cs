using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // Assignable
    public GameObject weaponSlot1;
    public GameObject weaponSlot2;

    public GameObject[] guns;
    public Recoil Recoil;

    public int selectedSlot = 2;
    
    public bool hasWeapon = false;


    private void Update()
    {
        MyInput();
        recoil();
    }

    void MyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && hasWeapon && !Recoil.GunSystem.Reloading)
            switchSlots(1);

        if (Input.GetKeyDown(KeyCode.Alpha2) && !Recoil.GunSystem.Reloading)
            switchSlots(2);
    }

    public void switchSlots(int s)
    {
        if (s == 1)
        {
            selectedSlot = 1;
            weaponSlot1.SetActive(true);
            weaponSlot2.SetActive(false);
        }
        else if(s == 2)
        {
            selectedSlot = 2;
            weaponSlot1.SetActive(false);
            weaponSlot2.SetActive(true);
        }
    }

    void recoil()
    {
        if (selectedSlot == 1)
        {
            Recoil.GunSystem = weaponSlot1.GetComponentInChildren<GunSystem>();
        }

        if (selectedSlot == 2)
        {
            Recoil.GunSystem = weaponSlot2.GetComponentInChildren<GunSystem>();
        }
    }
}
