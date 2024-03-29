using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverControls : MonoBehaviour
{
    private bool inRange;
    Transform player;

    [SerializeField] private CogLogic logic;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, player.position) < 3f)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                logic.SwitchState();
                GetComponent<Animator>().SetBool("On", !GetComponent<Animator>().GetBool("On"));
            }
        }
    }
}
