using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehavior : MonoBehaviour
{
    public enum RobotState
    {
        Idle,
        Patrol,
        Aim,
        Shoot
    }

    [SerializeField] private bool debugMode;

    [SerializeField] private RobotState currentState;
    [SerializeField] private float scanDistance;
    [SerializeField] private Transform nuzzleL;
    [SerializeField] private Transform nuzzleR;
    [SerializeField] private Transform eyeL;
    [SerializeField] private Transform eyeR;
    [SerializeField] private Transform neck;

    [SerializeField] private Animator animator;
    

    private void Update()
    {
        ScanPlayer();

        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                StartCoroutine(Shoot());
            }
        }
    }

    private void ScanPlayer()
    {
        if (debugMode)
        {
            Debug.DrawRay(eyeL.position, eyeL.forward * scanDistance, Color.magenta);
            Debug.DrawRay(eyeR.position, eyeR.forward * scanDistance, Color.magenta);
        }
    }

    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, scanDistance);
        }
    }

    private IEnumerator Shoot()
    {
        neck.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
        animator.SetBool("Shoot", true);
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("Shoot", false);
    }
}
