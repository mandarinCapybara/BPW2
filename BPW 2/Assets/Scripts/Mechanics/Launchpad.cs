using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Launchpad : MonoBehaviour
{
    [Header("Basic values")]
    [SerializeField] private float length;
    [SerializeField] private float velocityLauncherUp;
    [SerializeField] private float velocityLauncherDown;
    [SerializeField] private float velocityRB;

    private List<Rigidbody> rigidbodies = new List<Rigidbody>();
    private bool goUp = true;
    private bool move = false;

    [SerializeField] private float delayUp;
    [SerializeField] private float delayDown;

    [Header("Setup values")]
    private bool electrified;
    [SerializeField] private CoilElectrified targetSource;
    [SerializeField] private GameObject launcher;

    private void Start()
    {
        StartCoroutine(Delay(delayUp));
    }

    private void Update()
    {
        SetElectrified();

        if (electrified && move)
        {
            MoveLaunchpad();
        }
    }

    private void SetElectrified()
    {
        electrified = targetSource.GetElectrified();
    }

    private void MoveLaunchpad()
    {
        float target;

        if (goUp) target = length;
        else target = 0;

        if(Mathf.Abs(launcher.transform.localPosition.y - target) > 0.01f)
        {
            if (goUp)
            {
                Vector3 targetPos = new()
                {
                    x = 0,
                    y = length,
                    z = 0
                };
                launcher.transform.localPosition = Vector3.MoveTowards(launcher.transform.localPosition, targetPos, velocityLauncherUp * Time.deltaTime);
            }
            else
            {
                launcher.transform.localPosition = Vector3.MoveTowards(launcher.transform.localPosition, Vector3.zero, velocityLauncherDown * Time.deltaTime);
            }
        }
        else
        {
            float delay = delayDown;
            if (!goUp) delay = delayUp;

            StartCoroutine(Delay(delay));
        }
    }

    private IEnumerator Delay(float delay)
    {
        move = false;
        goUp = !goUp;
        yield return new WaitForSeconds(delay);
        move = true;

        if(goUp)
        Launch();
    }

    private void Launch()
    {
        if(rigidbodies != null)
        {
            foreach(Rigidbody rb in rigidbodies)
            {
                rb.velocity += transform.up * velocityRB;
            }
        }
    }

    public void AddRigidbody(Rigidbody body)
    {
        rigidbodies.Add(body);
    }
    public void RemoveRigidbody(Rigidbody body)
    {
        rigidbodies.Remove(body);
    }

}
