using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Player player;


    [HideInInspector] public Animator anim;
    [HideInInspector] public Slot slotEquippedOn;

    public ItemSO weaponData;
    public bool isAutomatic;
    [Space]
    public Transform shootPoint;

    [Header("Aiming")]
    public float aimSpeed = 10;
    public Vector3 hipPos;
    public Vector3 aimPos;
    public bool isAiming;


    [HideInInspector] public bool isReloading;
    [HideInInspector] public bool hasTakenOut;

    private float currentFireRate;
    private float fireRate;


    private void Start()
    {
        player = gameObject.GetComponent<Player>();
        anim = GetComponentInChildren<Animator>();

        transform.localPosition = hipPos;


        fireRate = weaponData.fireRate;

        currentFireRate = weaponData.fireRate;
    }

    private void Update()
    {
        if(weaponData.itemType == ItemSO.ItemType.Weapon)
        {
            UpdateAiming();
        }
        else if (weaponData.itemType == ItemSO.ItemType.Weapon)
        {

        }
    }


    #region Fire Weapon Functions


    public void Shoot()
    {
        if (currentFireRate < fireRate || isReloading || !hasTakenOut || player.running)
            return;

        anim.CrossFadeInFixedTime("Shoot_Base", 0.015f);

        GetComponentInParent<CameraLook>().RecoilCamera(Random.Range(-weaponData.horizontalRecoil,weaponData.horizontalRecoil),Random.Range(weaponData.minVerticalRecoil, weaponData.maxStack));
    }

    public void UpdateAiming()
    {
        if(Input.GetButton("Fire2") && !player.running && player.GetComponent<CharacterController>().isGrounded)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, aimPos, aimSpeed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, hipPos, aimSpeed * Time.deltaTime);
        }
    }


    #endregion



    public void Equip(Slot slot)
    {
        gameObject.SetActive(true);

        slotEquippedOn = slot;

        transform.localPosition = hipPos;
    }

    public void UnEquip()
    {
        gameObject.SetActive(false);

        slotEquippedOn = null;
    }
}
