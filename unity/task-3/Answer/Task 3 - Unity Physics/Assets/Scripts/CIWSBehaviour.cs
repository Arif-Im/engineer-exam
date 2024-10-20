using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CIWSBehaviour : WeaponBehavior
{
    [SerializeField] protected float sphereRadius = 5.0f;
    [SerializeField] LayerMask layer;
    protected float fireTime;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        fireTime = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if(fireTime>0){
            fireTime -= Time.deltaTime;
        }
        if (Physics2D.CircleCast(transform.position, sphereRadius, transform.right, 0, layer))
        {
            RaycastHit2D bulletHit = Physics2D.CircleCast(transform.position, sphereRadius, transform.right, 0, layer);
            if(!bulletHit.collider.gameObject.activeInHierarchy) return;
            if(!bulletHit.collider.gameObject.activeInHierarchy) return;
            if(fireTime<=0){
                GameObject target = bulletHit.collider.gameObject;
                Bullet bullet = LaunchMissile(target);
                bullet.RotateBullet(target.transform.position);
                fireTime = fireRate;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, sphereRadius);
    }
    
    protected Bullet LaunchMissile(GameObject target){
        Bullet bullet = GetBullet();
        bullet.SetTarget(target);
        bullet.transform.position = transform.position;
        return bullet;
    }
}
