using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopupAnimation : MonoBehaviour
{
    [SerializeField] private AnimationCurve opacityCurve;
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private AnimationCurve heightCurve;

    private Transform playerTransform;
    private TextMeshProUGUI tmp;
    private float time = 0;
    private Vector3 origin;
    private float dist;

    void Awake()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
        playerTransform = GameObject.Find("Player").transform;
    }
    void Update()
    {
        tmp.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
        dist = Vector3.Distance(origin, playerTransform.position);
        transform.localScale = (dist * 0.065f * scaleCurve.Evaluate(time)) * Vector3.one;
        transform.position = origin + new Vector3 (0, 1 + heightCurve.Evaluate(time), 0);
        time += Time.deltaTime;
    }
}
