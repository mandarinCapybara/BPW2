using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ElectricityNeighbors : MonoBehaviour
{
    [SerializeField] private bool debugMode;
    private List<GameObject> neighbors;
    [SerializeField] private int distanceChecked;
    [SerializeField] private float bounceModifier;
    [SerializeField] private GameObject electricityEffect;

    private Vector3 storedPosition;
    private List<GameObject> electricityObjects;

    void Start()
    {
        CalculateElectricity();
    }

    private void Update()
    {
        if (debugMode) // all debug code
        {
            DebugLines();
        }

        if(transform.position != storedPosition)
        {
            CalculateElectricity();
        }
    }

    public void CalculateElectricity()
    {
        storedPosition = transform.position;
        GetNeighbors();

        ClearElectricityObjects();

        foreach (GameObject neighbor in neighbors)
        {
            SetElectricity(neighbor.transform);
        }
    }

    private void ClearElectricityObjects()
    {
        if(electricityObjects != null)
        {
            foreach(GameObject obj in electricityObjects)
            {
                Destroy(obj);
            }
        }
        electricityObjects = new List<GameObject>();
    }

    // Compares every coil to the max distance to connect
    private void GetNeighbors()
    {
        neighbors = new List<GameObject>();

        GameObject[] coils = GameObject.FindGameObjectsWithTag("Coil Alive");

        foreach (GameObject coil in coils)
        {
            if (Vector3.Distance(transform.position, coil.transform.position) < distanceChecked)
            {
                if (coil != gameObject)
                    neighbors.Add(coil);
            }
        }
    }

    private void SetElectricity(Transform neigbor)
    {
        GameObject obj = Instantiate(electricityEffect);
        electricityObjects.Add(obj);

        VisualEffect VFX = obj.GetComponent<VisualEffect>();

        VFX.SetVector3("PositionA", transform.position);
        VFX.SetVector3("PositionB", neigbor.position);

        // distance between this coil and its neighbor
        float distance = Vector3.Distance(transform.position, neigbor.position);

        float bounceHeight = distance / bounceModifier;

        // gets the medians for every node in the bezier curve
        Vector3 median = GetMedian.Median(transform.position, neigbor.position);

        Vector3 bounceA = GetMedian.Median(transform.position, median);
        Vector3 bounceB = GetMedian.Median(neigbor.position, median);

        // applies an extra offset to create more of an illusion of electricity
        #region apply bounce
        bounceA.y += bounceHeight;
        bounceB.y -= bounceHeight;
        #endregion

        VFX.SetVector3("BounceA", bounceA);
        VFX.SetVector3("BounceB", bounceB);
    }
    private void DebugLines()
    {
        foreach(GameObject n in neighbors)
        {
            Debug.DrawLine(transform.position, n.transform.position, Color.cyan);
        }
    }
}
