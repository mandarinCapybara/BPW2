using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAtDistance : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private GameObject target;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(Vector3.Distance(player.transform.position, transform.position) < distance)
        {
            target.SetActive(true);
        }
        else
        {
            target.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, distance);
    }
}
