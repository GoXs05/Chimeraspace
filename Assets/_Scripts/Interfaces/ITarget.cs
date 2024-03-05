using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget
{
    public void TakeImpact(float impactForce, RaycastHit hit);
}
