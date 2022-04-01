using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class Gun : ScriptableObject
{
    public string gunName;
    public GameObject gunPrefab;

    [Header("Stats")]
    public int minDamage;
    public int maxDamage;
    public float maximumRange;
    public int totalAmmo;
    public int magazineSize;
    private int total;
    private int currentAmmo;
    //public int currentAmmo

    [SerializeField] private GameObject ImpactParticleSystem;

    public virtual void OnLeftMouseDown(Transform cameraPos) { }
    public virtual void OnLeftMouseHold(Transform cameraPos) { }
    public virtual void OnRightMouseDown() { }

    private void Awake()
    {
        total = totalAmmo;
        currentAmmo = magazineSize;
    }
    protected void Fire(Transform cameraPos)
    {
        Debug.Log(total + " " + currentAmmo);
        if (total > 0 || currentAmmo > 0)
        {
            if (currentAmmo > 0)
            {
                currentAmmo--;
            }
            else
            {
                Reload();
            }
            RaycastHit whatIHit;
            if (Physics.Raycast(cameraPos.position, cameraPos.transform.forward, out whatIHit, Mathf.Infinity))
            {
                IDamageable damageable = whatIHit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.DealDamage(Mathf.RoundToInt(maxDamage));
                }

                Instantiate(ImpactParticleSystem, whatIHit.point, Quaternion.LookRotation(whatIHit.normal));
            }
        }
    }

    protected void Reload()
    {
        total -= (magazineSize - currentAmmo);
        currentAmmo = magazineSize;
    }

}
