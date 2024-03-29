using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverControls : MonoBehaviour
{
    private bool inRange;
    Transform player;

    [SerializeField] private CogLogic logic;
    [SerializeField] private ElectricityNeighbors electricity;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    bool on;
    private void Update()
    {
        GetComponent<Animator>().SetBool("On", on);
        if (Vector3.Distance(transform.position, player.position) < 3f)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if (on)
                    on = false;
                else on = true;

                if(logic != null) logic.SwitchState();
                if (electricity != null) electricity.SetElectricity(on);

            }
        }
    }
}
