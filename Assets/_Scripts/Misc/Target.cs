using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DamagePopupGenerator))]
[RequireComponent(typeof(Animator))]
public class Target : MonoBehaviour, IDamageable, ITarget
{   
    [SerializeField] private Material[] materials;
    [SerializeField] private GameObject glowObj;
    private DamagePopupGenerator dmgPopupGen;

    private float health = 50;

    private Vector3 targetPos;
    private Vector3 startPos;

    private Animator animator;


    private void Start()
    {
        targetPos = transform.position;
        startPos = transform.position;

        dmgPopupGen = GetComponent<DamagePopupGenerator>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        PositionManager();
    }

    private void PositionManager()
    {
        targetPos = Vector3.Lerp(targetPos, startPos, 4f * Time.deltaTime);
        transform.position = targetPos;
    }
    
    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            dmgPopupGen.CreatePopup(transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f)), damage.ToString());
            if (health <= 0)
            {
                StartCoroutine(ColorManager());
                StartCoroutine(RotationManager());
                // implement target manager
            }
        }
    }

    public void TakeImpact(float impactForce, RaycastHit hit)
    {
        targetPos += -hit.normal * impactForce;
    }

    private IEnumerator ColorManager() 
    {
        yield return new WaitForSeconds(0.25f);
        glowObj.GetComponent<MeshRenderer>().sharedMaterial = materials[1];
        StartCoroutine(ColorResetTimer());
    }

    private IEnumerator ColorResetTimer() 
    {
        yield return new WaitForSeconds(3f);
        glowObj.GetComponent<MeshRenderer>().sharedMaterial = materials[0];
        health = 50;
    }

    private IEnumerator RotationManager()
    {
        animator.SetBool("death", true);
        yield return new WaitForSeconds(2.975f);
        animator.SetBool("death", false);
    }
}