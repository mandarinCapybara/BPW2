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

    [SerializeField] private HeightMode settings;

    [SerializeField] private float minHeight, maxHeight;
    bool toMax = true;
    private float delay = 2;

    private enum HeightMode
    {
        Static,
        Dynamic,
        Switch
    }

    private void Start()
    {
        UpdatePole();
    }

    private void Update()
    {
        switch (settings)
        {
            case HeightMode.Static:
                break;
            case HeightMode.Dynamic:
                if (delay <= 0)
                {
                    if (toMax)
                    {
                        if (height < maxHeight)
                        {
                            height += Time.deltaTime * 10;
                        }
                        else
                        {
                            delay = 2;
                            toMax = false;
                        }
                    }
                    else
                    {
                        if (height > minHeight)
                        {
                            height -= Time.deltaTime * 10;
                        }
                        else
                        {
                            delay = 2;
                            toMax = true;
                        }
                    }
                }
                else
                {
                    delay -= Time.deltaTime;
                }

                if (height != storedHeight)
                {
                    UpdatePole();
                }
                break;
            case HeightMode.Switch:
                if (height != storedHeight)
                {
                    UpdatePole();
                }
                break;
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
