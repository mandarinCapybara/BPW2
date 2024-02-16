using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    private void Awake()
    {
        instance = this; 
    }

    [SerializeField] private bool present;

    [SerializeField] private GameObject presentObjects;
    [SerializeField] private GameObject pastObjects;

    public void ChangeTime()
    {
        present = !present;
        UpdateEnvironment();
    }

    public bool isPresent()
    {
        return present;
    }

    private void UpdateEnvironment()
    {
        if (present)
        {
            presentObjects.SetActive(true);
            pastObjects.SetActive(false);
        }
        else
        {
            presentObjects.SetActive(false);
            pastObjects.SetActive(true);
        }
    }
}
