using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalOptimization : MonoBehaviour
{
    [SerializeField] private Transform[] decals;
    [SerializeField] private float checkTime = 0.5f;
    [SerializeField] private float renderDistance = 10f;
    private Transform player;  
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(DecalCheck());
    }

    private void OnEnable()
    {
        StartCoroutine(DecalCheck());
    }

    private IEnumerator DecalCheck()
    {
        while (true)
        {
            try
            {
                Debug.Log("checking, my name is " + transform.name);
                foreach (Transform decal in decals)
                {
                    if (Vector3.Distance(decal.position, player.position) < renderDistance)
                    {
                        decal.GetComponent<DecalProjector>().enabled = true;
                    }
                    else
                        decal.GetComponent<DecalProjector>().enabled = false;
                }
            }
            catch { }
            yield return new WaitForSeconds(checkTime);
        }
    }
}
