using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopupHandler : MonoBehaviour
{
    [SerializeField] private static Transform pfDamagePopup;
    private TextMeshPro textMesh;

    public static DamagePopupHandler Create(Vector3 position, float dmgAmt)
    {
        Transform dmgPopupTransform = Instantiate(pfDamagePopup, Vector3.zero, Quaternion.identity);

        DamagePopupHandler dmgPopup = dmgPopupTransform.GetComponent<DamagePopupHandler>();
        dmgPopup.Setup(dmgAmt);

        return dmgPopup;
    }

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(float dmgAmt)
    {
        textMesh.SetText(dmgAmt.ToString());
    }
}
