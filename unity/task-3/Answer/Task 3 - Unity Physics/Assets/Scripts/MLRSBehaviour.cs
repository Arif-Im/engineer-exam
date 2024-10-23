using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MLRSBehaviour : WeaponBehavior
{
    [SerializeField] protected float defaultHomingSpeed = 100.0f;
    protected float homingSpeed = 100.0f;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        homingSpeed = defaultBulletSpeed;
        target = FindAnyObjectByType<CIWSBehaviour>().gameObject;
        coroutine = LaunchMissileCoroutine();
        StartCoroutine(coroutine);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator LaunchMissileCoroutine(){
        MLRSBullet bullet = (MLRSBullet)LaunchMissile();
        bullet.SetTarget(target);
        bullet.SetBulletSpeed(bulletSpeed);
        bullet.SetHomingSpeed(homingSpeed);
        yield return new WaitForSeconds(fireRate);
        coroutine = LaunchMissileCoroutine();
        StartCoroutine(coroutine);
    }

    public void SetHomingSpeed(float homingSpeed){
        this.homingSpeed = Mathf.Max(defaultBulletSpeed + homingSpeed, 0.0f);
    }
}
