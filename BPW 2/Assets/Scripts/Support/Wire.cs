using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> renderers;
    [SerializeField] private Material poweredMat;
    [SerializeField] private Material unpoweredMat;

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
        foreach (MeshRenderer r in renderers)
        {
            Material[] storedMats = r.materials;

            if (powered == false)
            storedMats[1] = unpoweredMat;

            else if (powered)
                storedMats[1] = poweredMat;

            r.materials = storedMats;
        }
    }
}
