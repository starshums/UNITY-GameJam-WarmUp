﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public EnemyController enemyController;
    public PlayerController player;
    public ItemDetectionLocation itemDetectionLocation;

    public GameObject scaredFloatingTraps;
    // Start is called before the first frame update
    void Start()
    {
        enemyController = transform.GetComponentInParent<EnemyController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        itemDetectionLocation = player.transform.GetComponentInChildren<ItemDetectionLocation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ActivateScaredFloatingTraps(int trapID)
    {
        scaredFloatingTraps.transform.GetChild(trapID - 1).gameObject.SetActive(true);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Trap"))       //checking it it's a trap or not
        {
            GameObject trap = other.gameObject;
            if(trap.GetComponent<PickUp>().isTrapActive == true)        //checking if the trap is active or not
            {
                //Debug.Log("Yes trap is active");
                //raycast to the trap to see if kid can actually see it or not.
                RaycastHit raycastHit;
                Vector3 direction = trap.transform.position - transform.position;          //direction vector from player to trap
                if (Physics.Raycast(transform.position, direction, out raycastHit))             //checking if the raycast hit something or not
                {
                    //Debug.Log("Raycast is hitting an object");
                    Debug.DrawRay(transform.position, direction, Color.red);
                    if (raycastHit.transform.gameObject.CompareTag("Trap"))         //checking if the object that raycast hitted is a trap or if there's another object covering the obstacle
                    {
                       // Debug.Log("It has hit a trap");
                        float angle = Vector3.Angle(direction, transform.forward);
                        if (angle < 70)
                        {
                            //Debug.Log("Trap is in the field of view");
                            enemyController.isScared = true;
                            trap.GetComponent<PickUp>().isAvailable = false;
                            int trapID = other.gameObject.GetComponent<PickUp>().scareID;
                            Destroy(other.gameObject, 2f); //to destroy the trap after 2 seconds
                            ActivateScaredFloatingTraps(trapID);
                            itemDetectionLocation.CheckIfTrapGameObjectIsDestroyed();
                        }
                        /*
                        else
                        {
                            //Debug.Log("Trap is not in the field of view");
                            enemyController.isScared = false;
                        }*/
                    }
                    /*
                    else
                    {
                        //Debug.Log("did not hit trap");
                        enemyController.isScared = false;
                    }*/
                }
                

                
            }
        }
        //====> REMOVED TO AVOID KIDS GETTING SCARED WITH THE GHOST
        // if (other.CompareTag("Player"))
        // {
        //     if (!player.isInvisible)
        //     {
        //         RaycastHit raycastHit;
        //         Vector3 direction = other.transform.position - this.transform.position;          //direction vector from player to trap
        //         if (Physics.Raycast(transform.position, direction, out raycastHit))             //checking if the raycast hit something or not
        //         {
        //             //Debug.Log("Raycast is hitting an object");

                    
        //             if (raycastHit.transform.gameObject.CompareTag("Player"))         //checking if the object that raycast hitted is a trap or if there's another object covering the obstacle
        //             {
        //                 float angle = Vector3.Angle(direction, transform.forward);

        //                 if (angle < 70f)
        //                 {
        //                     Debug.DrawRay(transform.position, direction, Color.red);
        //                     //Apply some damage to ghost
        //                     //Debug.Log("It has hit the player");
        //                     enemyController.isScared = true;
        //                 }
        //             }
        //         }
        //     }
        // }
    }
}
