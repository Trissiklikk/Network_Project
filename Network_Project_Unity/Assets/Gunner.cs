using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : MonoBehaviour
{
    public Transform aimPosition;
    public GameObject currentGun;
    GameObject currentTarget;
    public float distance = 200f;

    bool isAiming;
    void Start()
    {
        currentGun.transform.position = aimPosition.position;
    }
    void CheckTarget()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position,transform.forward,out hit, distance))
        {
            if (!isAiming)
            {
                Debug.Log("Target Found");
                currentTarget = hit.transform.gameObject;
                isAiming = true; 
            }
            else
            {
                currentTarget = null;
                isAiming = false;
                Debug.Log("Target Not found");
            }
        }
    }
    void AimBot()
    {
        currentGun.transform.LookAt(currentGun.transform);
        Debug.DrawRay(transform.position, currentGun.transform.forward, Color.green);
    }

    // Update is called once per frame
    void Update()
    {
        CheckTarget();
        if(!isAiming)
        AimBot();
    }
}
