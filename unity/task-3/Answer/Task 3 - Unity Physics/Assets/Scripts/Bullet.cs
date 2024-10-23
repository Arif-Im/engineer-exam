using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected BoxCollider2D boxCollider;
    [SerializeField] protected float speed = 10.0f;
    [SerializeField] protected float startingLifetimeDuration = 10.0f;
    [SerializeField] string[] targetTags;
    protected float lifetime;
    protected GameObject target;

    // Start is called before the first frame update
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        lifetime = startingLifetimeDuration;
    }

    // Update is called once per frame
    protected void Update()
    {
        if(gameObject.activeInHierarchy){
            lifetime -= Time.deltaTime;
        }
        if(lifetime <= 0){
            DestroyBullet();
        }
    }

    protected void FixedUpdate()
    {
        rb.velocity = speed * Time.deltaTime * transform.right;
    }

    protected void DestroyBullet()
    {
        gameObject.SetActive(false);
        lifetime = startingLifetimeDuration;
    }

    public virtual void RotateBullet(Vector3 targetPos)
    {
        var heading = target.transform.position - transform.position;
        heading.Normalize();
        float angle = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void SetTarget(GameObject target){
        this.target = target;
    }

    public void SetBulletSpeed(float speed){
        this.speed = speed;
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        foreach(string tag in targetTags){
            if(col.gameObject.CompareTag(tag))
            {
                DestroyBullet();
            }
        }
    }
}
