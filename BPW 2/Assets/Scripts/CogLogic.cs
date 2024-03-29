using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogLogic : MonoBehaviour
{
    [SerializeField] private Animator anim1;
    [SerializeField] private Animator anim2;

    bool on = false;
    public void SwitchState()
    {
        on = !on;
        if(anim1 != null)
        {
            anim1.SetBool("On", on);
        }

        if(anim2 != null)
        {
            anim2.SetBool("On", on);
        }
    }
}
