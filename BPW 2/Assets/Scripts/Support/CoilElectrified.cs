using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoilElectrified : MonoBehaviour
{
    private bool electrified;

    public void SetElectified(bool value) { electrified = value; }

    public bool GetElectrified() { return electrified; }
}
