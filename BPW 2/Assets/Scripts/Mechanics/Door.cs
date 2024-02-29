using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool doorUnlocked;
    [SerializeField] private float playerRange = 5f;
    private bool doorOpen;
    [SerializeField] private CoilElectrified electricitySource;
    private Animator animator;

    private Transform playerTransform;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if(electricitySource != null)
        {
            doorUnlocked = electricitySource.GetElectrified();
        }
        if (doorUnlocked)
        {
            CheckPlayer();
        }

        try
        {
            animator.SetBool("Unlocked", doorUnlocked);
            animator.SetBool("Open", doorOpen);
        }
        catch { Debug.LogWarning("Door with name " + transform.name + " has no animator attached to it, and if it has, it simply isn't loaded yet."); }
    }

    private void CheckPlayer()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < playerRange)
            doorOpen = true;
        else
            doorOpen = false;
    }

}
