using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTagSetter : MonoBehaviour
{
    private void Start()
    {
        Transform _t = transform;
        foreach(Transform t in _t)
        {
            t.gameObject.tag = _t.gameObject.tag;
        }
    }
}
