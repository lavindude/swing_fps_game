using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class Gun : ScriptableObject
{
    public string gunName;
    public GameObject gunPrefab;

    [Header("Stats")]
    public AmmunitionTypes ammunitionType;
    public int minDamage;
    public int maxDamage;
    public float maximumRange;
    [SerializeField] private GameObject ImpactParticleSystem;
    [SerializeField] private ParticleSystem MuzzleParticleSystem;

    public virtual void OnLeftMouseDown(Transform cameraPos) { }
    public virtual void OnLeftMouseHold(Transform cameraPos) { }
    public virtual void OnRightMouseDown() { }

    protected void Fire(Transform cameraPos)
    {
        if (AmmunitionManager.instance.ConsumeAmmo(ammunitionType))
        {
            RaycastHit whatIHit;
            if (Physics.Raycast(cameraPos.position, cameraPos.transform.forward, out whatIHit, Mathf.Infinity))
            {
                IDamageable damageable = whatIHit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.DealDamage(Mathf.RoundToInt(maxDamage));
                }

                Instantiate(ImpactParticleSystem, whatIHit.point, Quaternion.LookRotation(whatIHit.normal));
                MuzzleParticleSystem.Emit(1);
            }
        }
        
    }

}
