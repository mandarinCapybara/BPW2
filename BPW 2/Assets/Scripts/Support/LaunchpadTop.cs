using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchpadTop : MonoBehaviour
{
    private Launchpad launcher;
    private void Start()
    {
        launcher = transform.parent.gameObject.GetComponent<Launchpad>();
    }

    private void OnTriggerEnter(Collider other)
    {
            if (other.GetComponent<Rigidbody>() != null)
                launcher.AddRigidbody(other.GetComponent<Rigidbody>());
    }

    private void OnTriggerExit(Collider other)
    {
            if (other.GetComponent<Rigidbody>() != null)
                launcher.RemoveRigidbody(other.GetComponent<Rigidbody>());
    }
}
