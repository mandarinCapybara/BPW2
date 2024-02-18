using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ElectricityNeighbors : MonoBehaviour
{
    [Header("Debug values")]
    [SerializeField] private bool debugMode;

    [Header("In-game values")]
    [SerializeField] private bool electrify;
    private bool storedElectrify;
    [SerializeField] private Wire connectedWire;

    GameObject[] coils;

    private List<NeighborNodes> nodes;
    private List<ElectricityHitbox> hitboxes;

    [SerializeField] private float distanceChecked;
    [SerializeField] private float maxNeighborDistance;

    private List<GameObject> neighbors;
    [SerializeField] private LayerMask shockMask;

    [Header("VFX values")]
    [SerializeField] private float bounceModifier;
    private List<GameObject> electricityObjects;

    [SerializeField] private GameObject electricityEffect;
    private void Start()
    {
        neighbors = new List<GameObject>();
        coils = GameObject.FindGameObjectsWithTag("Coil Alive");
        InvokeRepeating("SetPotentialNeighbors", 0, 0.5f);
        //InvokeRepeating("CalculateElectricity", 0, 1.5f);
        SetHitboxes(new List<ElectricityHitbox>());
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
            else if (Vector3.Distance(transform.position, g.transform.position) < maxNeighborDistance)
            {
                if (!neighbors.Contains(g))
                {
                    bool found = false;
                    if(nodes != null)
                    {
                        foreach (NeighborNodes n in nodes)
                        {
                            if (n.currentNode == g)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    neighbors.Add(g);
                    if (!found)
                    {
                        CalculateElectricity();
                    }
                }
            }
            else
            {
                if (neighbors.Contains(g))
                {
                    if (g.GetComponent<CoilElectrified>())
                    {
                        g.GetComponent<CoilElectrified>().SetElectified(false);
                    }
                    neighbors.Remove(g);
                }
            }
        }
    }

    private void Update()
    {
        if(nodes != null)
        foreach (NeighborNodes n in nodes)
        {
            if (n.currentNode.transform.position != n.storedPosition)
            {
                CalculateElectricity();
            }
        }

        

        if (debugMode && Input.GetKeyDown(KeyCode.Space))
        {
            SetElectricity(!electrify);
        }

        if(connectedWire != null)
        {
            if(storedElectrify != connectedWire.IsPowered())
                SetElectricity(connectedWire.IsPowered());
        }
    }

    private void FixedUpdate()
    {
        CheckPlayerHit();
    }
    public void SetElectricity(bool value)
    {
        electrify = value;
        storedElectrify = electrify;
        CalculateElectricity();
    }


    private void CheckPlayerHit()
    {
        foreach (ElectricityHitbox hb in hitboxes)
        {
            if (debugMode)
            {
                Debug.DrawLine(hb.StartPosition, hb.EndPosition, Color.magenta);
            }

            if (Physics.Linecast(hb.StartPosition, hb.EndPosition, out RaycastHit hit, shockMask))
            {
                Debug.Log(hit.transform.name);
            }

        }
    }

    public void CalculateElectricity()
    {
        ClearElectricityObjects();
        SetHitboxes(new List<ElectricityHitbox>());

        if(nodes != null)
        {
            foreach(NeighborNodes n in nodes)
            {
                if(n.currentNode.GetComponent<CoilElectrified>() != null)
                {
                    n.currentNode.GetComponent<CoilElectrified>().SetElectified(false);
                }
            }
        }

        nodes = new List<NeighborNodes>();


        if (electrify)
        {
            AddNode(gameObject);
        }
    }
    public void AddNode(GameObject nodeObject)
    {
        NeighborNodes n = new NeighborNodes();
        nodes.Add(n);
        n.mainSystem = this;
        n.currentNode = nodeObject;
        n.SetStoredPosition();

        if (n.currentNode.GetComponent<CoilElectrified>() != null)
        {
            n.currentNode.GetComponent<CoilElectrified>().SetElectified(true);
        }

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


    public List<ElectricityHitbox> GetHitboxes()
    {
        return hitboxes;
    }

    public void SetHitboxes(List<ElectricityHitbox> newHitboxes)
    {
        ;
        hitboxes = newHitboxes;
    }

    public void AddHitbox(ElectricityHitbox newHitbox)
    {
        hitboxes.Add(newHitbox);
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
            SetNewHitbox();

            foreach (GameObject neighbor in connectedNodes)
            {
                mainSystem.AddNode(neighbor);
                SetElectricity(neighbor.transform);
            }
        }

        private void SetNewHitbox()
        {
            foreach (GameObject neighbor in connectedNodes)
            {
                ElectricityHitbox hb = new ElectricityHitbox();
                hb.StartPosition = currentNode.transform.position;
                hb.EndPosition = neighbor.transform.position;
                mainSystem.AddHitbox(hb);
            }
        }

        private void SetElectricity(Transform neigbor)
        {
            GameObject obj = Instantiate(mainSystem.electricityEffect);
            mainSystem.electricityObjects.Add(obj);
            obj.transform.SetParent(currentNode.transform);

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

    [System.Serializable]
    public class ElectricityHitbox
    {
        public Vector3 StartPosition;
        public Vector3 EndPosition;
    }

    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            Gizmos.color = Color.cyan;
            if (nodes != null)
            {
                foreach (NeighborNodes n in nodes)
                {
                    Vector3 pos = n.storedPosition;

                    Gizmos.DrawWireSphere(pos, distanceChecked);
                }
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, maxNeighborDistance);
        }
    }
}
