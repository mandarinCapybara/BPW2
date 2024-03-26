using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour
{
    [SerializeField] private Transform position1, position2;
    [SerializeField] private float speed = 0.5f;

    private bool toPos1 = false;
    void Update()
    {
        if (toPos1)
        {
            transform.position = Vector3.MoveTowards(transform.position, position1.position, Time.deltaTime * speed);
            if (Vector3.Distance(transform.position, position1.position) < 0.1f)
                toPos1 = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, position2.position, Time.deltaTime * speed);
            if (Vector3.Distance(transform.position, position2.position) < 0.1f)
                toPos1 = true;
        }
    }
}
