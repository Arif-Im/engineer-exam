using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected BoxCollider2D boxCollider;
    [SerializeField] protected KeyCode upKey;
    [SerializeField] protected KeyCode downKey;
    [SerializeField] protected KeyCode leftKey;
    [SerializeField] protected KeyCode rightKey;
    [SerializeField] protected float defaultSpeed = 100.0f;
    protected float speed = 100.0f;
    [SerializeField] protected Bullet bulletPrefab;
    [SerializeField] protected string bulletPoolName;
    [SerializeField] protected float fireRate = 1.0f;
    [SerializeField] protected float defaultBulletSpeed = 100.0f;
    protected float bulletSpeed = 100.0f;
    protected GameObject bulletPoolGroupObject;
    [SerializeField] protected List<Bullet> bulletPool = new();
    protected IEnumerator coroutine;
    protected GameObject target;

    // Start is called before the first frame update
    protected void Start()
    {
        speed = defaultSpeed;
        bulletSpeed = defaultBulletSpeed;

        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        bulletPoolGroupObject = GameObject.Find(bulletPoolName);
        foreach(Bullet bullet in bulletPoolGroupObject.GetComponentsInChildren<Bullet>())
        {
            bulletPool.Add(bullet);
            bullet.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        var weaponVelocity = new Vector2(0,0);

        if(Input.GetKey(upKey))
            weaponVelocity += Time.deltaTime * new Vector2(0,1);
        if(Input.GetKey(downKey))
            weaponVelocity += Time.deltaTime * new Vector2(0,-1);
        if(Input.GetKey(rightKey))
            weaponVelocity += Time.deltaTime * new Vector2(1,0);
        if(Input.GetKey(leftKey))
            weaponVelocity += Time.deltaTime * new Vector2(-1,0);

        var normalizedWeaponVelocity = weaponVelocity.normalized;
        rb.velocity = speed * Time.deltaTime * normalizedWeaponVelocity;
        float angle = Mathf.Atan2(normalizedWeaponVelocity.y, normalizedWeaponVelocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    protected Bullet GetBullet()
    {
        foreach(Bullet bullet in bulletPool)
        {
            if(bullet.gameObject.activeInHierarchy == false){
                bullet.gameObject.SetActive(true);
                return bullet;
            }
        }
        Bullet spawnedBullet = Instantiate(bulletPrefab);
        spawnedBullet.transform.parent = bulletPoolGroupObject.transform;
        bulletPool.Add(spawnedBullet);
        return spawnedBullet;
    }
    
    protected Bullet LaunchMissile(){
        Bullet bullet = GetBullet();
        bullet.transform.position = transform.position;
        return bullet;
    }

    public virtual IEnumerator LaunchMissileCoroutine(){
        Bullet bullet = LaunchMissile();
        bullet.SetTarget(target);
        bullet.SetBulletSpeed(bulletSpeed);
        yield return new WaitForSeconds(fireRate);
        coroutine = LaunchMissileCoroutine();
        StartCoroutine(coroutine);
    }

    public void SetBulletSpeed(float bulletSpeed){
        this.bulletSpeed = Mathf.Max(defaultBulletSpeed + bulletSpeed, 0.0f);
    }
    public void SetTankSpeed(float tankSpeed){
        this.speed = Mathf.Max(defaultSpeed + tankSpeed, 0.0f);
    }
}
