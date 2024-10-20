using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLRSBullet : Bullet
{

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        RotateBullet(target.transform.position);
    }

    public override void RotateBullet(Vector3 targetPos)
    {
        var heading = targetPos - transform.position;
        heading.Normalize();
        float angle = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 100 * Time.deltaTime);
    }
}