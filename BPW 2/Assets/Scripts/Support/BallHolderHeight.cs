using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallHolderHeight : MonoBehaviour
{
    [Range(0.1f, 8f)]
    [SerializeField] private float height = 1;
    float storedHeight = 0;

    [SerializeField] private float holderOffset = 0.3f;

    [SerializeField] private Transform pole;
    [SerializeField] private Transform ballHolder;

    private void Start()
    {
        UpdatePole();
    }

    private void Update()
    {
        if(height != storedHeight)
        {
            UpdatePole();
        }
    }

    private void UpdatePole()
    {
        storedHeight = height;

        Vector3 scale = new()
        {
            x = 1,
            y = 1,
            z = height
        };

        scale *= 100;

        pole.localScale = scale;

        Vector3 position = new()
        {
            x = 0,
            y = holderOffset + height,
            z = 0
        };
        ballHolder.transform.localPosition = position;
    }
}
