using System.Collections;
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

    [SerializeField] private LineRenderer lineA;
    [SerializeField] private LineRenderer lineB;
    private float lineWidth = 0.002f;

    [SerializeField] private Transform eyeL;
    [SerializeField] private Transform eyeR;

    [SerializeField] private Transform neck;
    [SerializeField] private Transform head;
    [SerializeField] private LayerMask mask;

    [SerializeField] private Animator animator;
    [SerializeField] private Animator vfxAnimator;
    [SerializeField] private MultiAimConstraint neckConstraint;

    private bool canAttack = true;
    private bool lowerNeckWeight;
    private bool drawLines = false;
    private GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lineA.widthMultiplier = lineWidth;
        lineA.positionCount = 2;

        lineB.widthMultiplier = lineWidth;
        lineB.positionCount = 2;

    }

    private void UpdateState(RobotState state)
    {
        currentState = state;
    }

    private void Update()
    {
        if (drawLines)
        {
            lineA.widthMultiplier += Time.deltaTime * 0.01f;
            lineA.SetPosition(0, nuzzleL.position);
            lineA.SetPosition(1, player.transform.position);

            lineB.widthMultiplier += Time.deltaTime * 0.01f;
            lineB.SetPosition(0, nuzzleR.position);
            lineB.SetPosition(1, player.transform.position);
        }
        else
        {
            lineA.widthMultiplier = lineWidth;
            lineB.widthMultiplier = lineWidth;

            lineA.SetPosition(0, nuzzleL.position);
            lineA.SetPosition(1, nuzzleL.position);

            lineB.SetPosition(0, nuzzleR.position);
            lineB.SetPosition(1, nuzzleR.position);
        }

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
        if (currentState != RobotState.Attack && canAttack)
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

    private void ScanPlayer(Vector3 endPos)
    {
        if(Physics.Linecast(head.transform.position, endPos, mask))
        {
            player.GetComponent<Player>().Respawn();
        }
    }

    private void OnDisable()
    {
        drawLines = false;
        canAttack = true;
        lowerNeckWeight = true;
        UpdateState(RobotState.Idle);
        animator.SetBool("Shoot", false);
        vfxAnimator.SetBool("Shoot", false);
    }

    private IEnumerator Shoot()
    {
        canAttack = false;
        neckConstraint.weight = 1;
        animator.SetBool("Shoot", true);
        vfxAnimator.SetBool("Shoot", true);
        yield return new WaitForSeconds(0.5f);
        drawLines = true;
        yield return new WaitForSeconds(1.9f);
        drawLines = false;
        Vector3 storedPos = player.transform.position;
        yield return new WaitForSeconds(0.1f);
        ScanPlayer(storedPos);
        animator.SetBool("Shoot", false);
        vfxAnimator.SetBool("Shoot", false);
        lowerNeckWeight = true;
        UpdateState(RobotState.Idle);
        yield return new WaitForSeconds(2.5f);
        canAttack = true;
    }
}
