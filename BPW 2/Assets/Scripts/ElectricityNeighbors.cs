using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ElectricityNeighbors : MonoBehaviour
{
    [SerializeField] private bool debugMode;
    private List<NeighborNodes> nodes;
    [SerializeField] private int distanceChecked;
    [SerializeField] private float bounceModifier;
    [SerializeField] private GameObject electricityEffect;

    private List<GameObject> electricityObjects;
    GameObject[] coils;
    private List<GameObject> neighbors;

    private void Start()
    {
        neighbors = new List<GameObject>();
        coils = GameObject.FindGameObjectsWithTag("Coil Alive");
        InvokeRepeating("SetPotentialNeighbors", 0, 0.5f);
    }

    private void SetPotentialNeighbors()
    {
        foreach (GameObject g in coils)
        {
            if (Vector3.Distance(transform.position, g.transform.position) < distanceChecked)
            {
                if (!neighbors.Contains(g))
                {
                    neighbors.Add(g);
                    CalculateElectricity();
                }
            }
            else
            {
                if (neighbors.Contains(g))
                {
                    neighbors.Remove(g);
                }
            }
        }
    }

    private void Update()
    {
        foreach (NeighborNodes n in nodes)
        {
            if (n.currentNode.transform.position != n.storedPosition)
            {
                CalculateElectricity();
            }
        }
    }

    public void CalculateElectricity()
    {
        ClearElectricityObjects();

        nodes = new List<NeighborNodes>();
        AddNode(gameObject);
    }
    public void AddNode(GameObject nodeObject)
    {
        NeighborNodes n = new NeighborNodes();
        nodes.Add(n);
        n.mainSystem = this;
        n.currentNode = nodeObject;
        n.SetStoredPosition();

        n.GetNewNeighbors();
    }

    private void ClearElectricityObjects()
    {
        if (electricityObjects != null)
        {
            foreach (GameObject obj in electricityObjects)
            {
                Destroy(obj);
            }
        }
        electricityObjects = new List<GameObject>();
    }


    [System.Serializable]
    public class NeighborNodes
    {
        public ElectricityNeighbors mainSystem;
        public GameObject currentNode;
        public List<GameObject> connectedNodes;
        public Vector3 storedPosition;

        public void SetStoredPosition()
        {
            storedPosition = currentNode.transform.position;
        }

        public void GetNewNeighbors()
        {
            connectedNodes = new List<GameObject>();

            GameObject[] coils = GameObject.FindGameObjectsWithTag("Coil Alive");

            foreach (GameObject coil in coils)
            {
                if (Vector3.Distance(currentNode.transform.position, coil.transform.position) < mainSystem.distanceChecked)
                {
                    if (coil != currentNode)
                    {
                        bool locked = false;
                        foreach (NeighborNodes n in mainSystem.nodes)
                        {
                            if (n.currentNode == coil)
                            {
                                locked = true;
                                break;
                            }
                        }
                        if (!locked)
                            connectedNodes.Add(coil);
                    }
                }
            }

            foreach (GameObject neighbor in connectedNodes)
            {
                mainSystem.AddNode(neighbor);
                SetElectricity(neighbor.transform);
            }
        }

        private void SetElectricity(Transform neigbor)
        {
            GameObject obj = Instantiate(mainSystem.electricityEffect);
            mainSystem.electricityObjects.Add(obj);

            VisualEffect VFX = obj.GetComponent<VisualEffect>();

            VFX.SetVector3("PositionA", currentNode.transform.position);
            VFX.SetVector3("PositionB", neigbor.position);

            // distance between this coil and its neighbor
            float distance = Vector3.Distance(currentNode.transform.position, neigbor.position);

            float bounceHeight = distance / mainSystem.bounceModifier;

            // gets the medians for every node in the bezier curve
            Vector3 median = GetMedian.Median(currentNode.transform.position, neigbor.position);

            Vector3 bounceA = GetMedian.Median(currentNode.transform.position, median);
            Vector3 bounceB = GetMedian.Median(neigbor.position, median);

            // applies an extra offset to create more of an illusion of electricity
            #region apply bounce
            bounceA.y += bounceHeight;
            bounceB.y -= bounceHeight;
            #endregion

            VFX.SetVector3("BounceA", bounceA);
            VFX.SetVector3("BounceB", bounceB);
        }
    }
}
