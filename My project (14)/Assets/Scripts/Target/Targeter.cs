using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup _cinemachineTargetGroup;
    private List<Target> _targets = new List<Target>();

    public Target CurrentTarget { get; private set; }
    
    public bool IsTargeting { get; private set; }

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out var target)) return;
        _targets.Add(target);
        target.OnTargetDestroyed += RemoveTarget;
    }

    private void OnTriggerExit(Collider other)
    {
       if(other.TryGetComponent<Target>(out var target))
       {
           RemoveTarget(target);
       }
    }

    private void RemoveTarget(Target target)
    {
        if(CurrentTarget == target) 
        {
            _cinemachineTargetGroup.RemoveMember(target.transform);
            CurrentTarget = null;
        }

        target.OnTargetDestroyed -= RemoveTarget;
        _targets.Remove(target);
        if(_targets.Count <= 0 ) CancelTarget();
    }

    public bool SelectTarget()
    {
        if (_targets.Count <= 0) return false;
        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach (var target in _targets)
        {
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);
            if(viewPos.x is < 0 or > 1 || viewPos.y is < 0 or > 1 ) continue;

            var vectorToCenter = viewPos - new Vector2(0.5f, 0.5f);
            if (!(vectorToCenter.sqrMagnitude < closestTargetDistance)) continue;
            closestTarget = target;
            closestTargetDistance = vectorToCenter.sqrMagnitude;
        }

        if (closestTarget == null) return false;

        CurrentTarget = closestTarget;
        _cinemachineTargetGroup.AddMember(CurrentTarget.transform, 1, 2);
        ToggleTarget(true);
        return true;
    }

    public void CancelTarget()
    {
        ToggleTarget(false);
        if (CurrentTarget == null) return;
        _cinemachineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }
    
    public void ToggleTarget(bool value)
    {
        IsTargeting = value;
    }
}

