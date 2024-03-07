using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoilElectrified : MonoBehaviour
{
    private bool electrified;

    public CoilElectrified child;

    private void Update()
    {

        if (child != null)
        {
            child.SetElectified(electrified);
        }
    }

    public void SetElectified(bool value) { electrified = value; }

    public bool GetElectrified() { return electrified; }
}
