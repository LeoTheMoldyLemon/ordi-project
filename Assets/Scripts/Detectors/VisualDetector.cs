using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class VisualDetector : Detector
{
    [SerializeField] private float range;
    [SerializeField] private bool ignoreTerrain;
    [SerializeField] private bool ignoreTerrainAfterDetection;
    [SerializeField] private bool lockOnAfterDetection;

    public override bool Detect()
    {
        if (isCurrentlyDetected && lockOnAfterDetection)
            return true;

        if (Vector2.Distance(target.position, transform.position) > range)
            return false;

        if (ignoreTerrain || isCurrentlyDetected && ignoreTerrainAfterDetection)
            return true;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, range);
        return !hit || hit.transform == target;
    }
}
