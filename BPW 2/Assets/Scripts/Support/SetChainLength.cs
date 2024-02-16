using System.Collections.Generic;
using UnityEngine;

public class SetChainLength : MonoBehaviour
{
    [Header("General values")]
    [Range(1, 20)]
    [SerializeField] private int chainLength;

    [SerializeField] private Transform linkStart;
    [SerializeField] private Transform linkEnd;

    [Header("Standard values")]
    [SerializeField] private GameObject chain;
    [SerializeField] private float offsetChain = 0.2f;
    [SerializeField] private float offsetStartLink = 0.15f;
    [SerializeField] private float offsetEndLink = 0.4f;

    [SerializeField] private int swingLimitStart = 50;

    private List<GameObject> chains = new List<GameObject>();

    int storedLength;

    private void Start()
    {
        UpdateChain();
    }

    private void Update()
    {
        if (storedLength != chainLength)
            UpdateChain();
    }

    private void UpdateChain()
    {
        if(chains != null)
        {
            foreach(GameObject go in chains)
            {
                Destroy(go);
            }
        }

        storedLength = chainLength;

        chains = new List<GameObject>();
        float offset = offsetStartLink + (offsetChain * chainLength) + offsetEndLink;
        Vector3 localPos = new();

        localPos.y = -offset;

        linkEnd.transform.localPosition = localPos;

        Vector3 storedRot = linkEnd.transform.localRotation.eulerAngles;
        Vector3 newRot = storedRot;

        HingeJoint joint = null;

        if (chainLength % 2 == 0)
        {
            newRot.y -= 90;
            linkEnd.transform.localRotation = Quaternion.Euler(newRot);
        }
        else
        {
            linkEnd.transform.localRotation = Quaternion.Euler(newRot);
        }


        for (int i = 0; i < chainLength; i++)
        {
            offset = offsetStartLink + (offsetChain * i);

            GameObject c = Instantiate(chain, transform);
            chains.Add(c);

            if (i % 2 != 0)
            {
                storedRot = c.transform.localRotation.eulerAngles;
                newRot = storedRot;
                newRot.y -= 90;
                c.transform.localRotation = Quaternion.Euler(newRot);
            }

            localPos = new();
            localPos.y = -offset;

            c.transform.localPosition = localPos;

            joint = c.GetComponent<HingeJoint>();

            if (i == 0)
            {
                joint.connectedBody = linkStart.GetComponent<Rigidbody>();
            }
            else
            {
                joint.connectedBody = chains[i - 1].GetComponent<Rigidbody>();
            }

            float iF = chainLength - i;
            float cl = chainLength;
            float multiplication = iF / cl;

            JointLimits limits = joint.limits;
            limits.max = swingLimitStart * multiplication;
            limits.min = -swingLimitStart * multiplication;
            joint.limits = limits;


            linkEnd.gameObject.GetComponent<HingeJoint>().connectedBody = c.GetComponent<Rigidbody>();
        }

    }
}
