using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]

public class GunScriptableObject : ScriptableObject
{
    public GunType type;
    public string name;
    public GameObject modelPrefab;
    public Vector3 spawnPoint;
    public Vector3 spawnRotation;

    public ShootConfiguration shootConfig;
    public TrailConfiguration trailConfig;

    private MonoBehavior activeMonoBehavior;
    private GameObject model;
    private float lastShootTime;
    private ParticleSystem shootSystem;
    private ObjectPool<TrailRenderer> TrailPool;

    public void Spawn(Transform parent, MonoBehavior activeMonoBehavior)
    {
        this.activeMonoBehavior = activeMonoBehavior;
        lastShootTime = 0;
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);

        model = Instantiate(modelPrefab);
        model.transform.setParent(parent, false);
        model.transform.localPosition = spawnPoint;
        model.transform.localRotation = Quaternion.Euler(spawnRotation);

        shootSystem = model.GetComponentinChildren<ParticleSystem>();
    }

    public void Shoot()
    {
        if (Time.time > shootConfig.time + lastShootTime)
        {
            lastShootTime = Time.time;
            shootSystem.Play();

            Vector3 shootDirection = shootSystem.transform.forward
                + new Vector3(
                    Random.Range(
                        -shootConfig.Spread.x,
                        shootConfig.Spread.x
                    ),
                    Random.Range(
                        -shootConfig.Spread.y,
                        shootConfig.Spread.y
                    ),
                    Random.Range(
                        -shootConfig.Spread.z,
                        shootConfig.Spread.z
                    )
                );
            shootDirection.normalize();
        }
    }

    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();

        trail.colorGradient = trailConfig.color;
        trail.material = trailConfig.material;
        trail.widthCurve = trailConfig.widthCurve;
        trail.time = trailConfig.time;
        trail.minVertexDistance = trailConfig.minVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }
}
