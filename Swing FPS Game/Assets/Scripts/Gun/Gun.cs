using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class Gun : ScriptableObject
{
    public int gunId;
    public string gunName;
    public GameObject gunPrefab;
    public int ammoAmount;
    public int maxAmmo;

    [Header("Stats")]
    public int minDamage;
    public int maxDamage;
    public float maximumRange;
    [SerializeField] public GameObject ImpactParticleSystem;
    public WeaponHandler weaponHandler;

    public AudioSource shoot;
    public AudioSource reload;

    public virtual void OnLeftMouseDown(Transform cameraPos) { }
    public virtual void OnLeftMouseHold(Transform cameraPos) { }
    public virtual void OnRightMouseDown() { }

    protected void Fire(Transform cameraPos)
    {
        if (ammoAmount > 0)
        {
            weaponHandler.muzzleFlash.GetComponent<ParticleSystem>().Play();
            shoot.Play();

            RaycastHit whatIHit;
            if (Physics.Raycast(cameraPos.position, cameraPos.transform.forward, out whatIHit, Mathf.Infinity))
            {
                IDamageable damageable = whatIHit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.DealDamage(Mathf.RoundToInt(maxDamage));
                }

                if(whatIHit.collider.tag.CompareTo("NoParticle") != 0)
                {
                    Instantiate(ImpactParticleSystem, whatIHit.point, Quaternion.LookRotation(whatIHit.normal));
                }
                
                
            }

            ammoAmount--;
        }
    }

    
}
