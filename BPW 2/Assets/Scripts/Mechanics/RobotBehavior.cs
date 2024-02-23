using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RobotBehavior : MonoBehaviour
{
    public enum RobotState
    {
        Idle,
        Patrol,
        Attack
    }

    [SerializeField] private bool debugMode;

    [SerializeField] private RobotState currentState;
    [SerializeField] private float scanDistance;
    [SerializeField] private Transform nuzzleL;
    [SerializeField] private Transform nuzzleR;
    [SerializeField] private Transform eyeL;
    [SerializeField] private Transform eyeR;
    [SerializeField] private Transform neck;
    [SerializeField] private LayerMask mask;

    [SerializeField] private Animator animator;
    [SerializeField] private Animator vfxAnimator;
    [SerializeField] private MultiAimConstraint neckConstraint;

    private bool canAttack = true;
    private bool lowerNeckWeight;

    private void UpdateState(RobotState state)
    {
        currentState = state;
    }

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

        if (lowerNeckWeight) LowerWeight();
    }

    private void LowerWeight()
    {
        neckConstraint.weight -= Time.deltaTime * 3;
        if (neckConstraint.weight <= 0)
            lowerNeckWeight = false;
    }

    private void ScanPlayer()
    {
        if(currentState != RobotState.Attack && canAttack)
        {
            if (debugMode)
            {
                Debug.DrawRay(eyeL.position, eyeL.forward * scanDistance, Color.magenta);
                Debug.DrawRay(eyeR.position, eyeR.forward * scanDistance, Color.magenta);
            }

            if (Physics.Raycast(eyeL.position, eyeL.forward, scanDistance, mask))
            {
                StartCoroutine(Shoot());
            }
            else if (Physics.Raycast(eyeR.position, eyeR.forward, scanDistance, mask))
            {
                StartCoroutine(Shoot());
            }
        }
        else
        {
            if (debugMode)
            {
                Debug.DrawRay(eyeL.position, eyeL.forward * scanDistance, Color.white);
                Debug.DrawRay(eyeR.position, eyeR.forward * scanDistance, Color.white);
            }
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
        canAttack = false;
        neck.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
        neckConstraint.weight = 1;
        animator.SetBool("Shoot", true);
        vfxAnimator.SetBool("Shoot", true);
        yield return new WaitForSeconds(2.8f);
        animator.SetBool("Shoot", false);
        vfxAnimator.SetBool("Shoot", false);
        lowerNeckWeight = true;
        UpdateState(RobotState.Idle);
        yield return new WaitForSeconds(2.5f);
        canAttack = true;
    }
}
