using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Wire : MonoBehaviour
{
    [SerializeField] private GameObject poweredObj;
    [SerializeField] private GameObject unpoweredObj;

    [SerializeField] private CoilElectrified parent;

    private bool powered;
    private bool poweredStored;

    private void Update()
    {
        powered = parent.GetElectrified(); 
        if(powered !=  poweredStored)
        {
            UpdateMaterials();
        }
    }

    public bool IsPowered()
    {
        return powered;
    }

    private void UpdateMaterials()
    {
        poweredStored = powered;
        if (powered == false)
        {
            poweredObj.SetActive(false);
            unpoweredObj.SetActive(true);
        }

        else if (powered)
        {
            poweredObj.SetActive(true);
            unpoweredObj.SetActive(false);
        }
    }
}
