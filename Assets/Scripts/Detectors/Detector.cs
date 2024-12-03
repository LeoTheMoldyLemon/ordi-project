using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class Detector : MonoBehaviour
{

    [SerializeField] protected Transform target;
    [SerializeField] private double period;
    public UnityEvent targetDetected = new(), targetLost = new();

    private double secondsSinceLastDetect = 0;
    protected bool isCurrentlyDetected = false;

    abstract public bool Detect();

    void Update()
    {
        secondsSinceLastDetect += Time.deltaTime;
        if (secondsSinceLastDetect > period)
        {
            secondsSinceLastDetect = 0;
            if (Detect())
            {
                if (!isCurrentlyDetected)
                {
                    isCurrentlyDetected = true;
                    Debug.Log("Detector: target detected");
                    targetDetected.Invoke();
                }
            }
            else if (isCurrentlyDetected)
            {
                isCurrentlyDetected = false;
                Debug.Log("Detector: target lost");
                targetLost.Invoke();
            }
        }
    }
}
