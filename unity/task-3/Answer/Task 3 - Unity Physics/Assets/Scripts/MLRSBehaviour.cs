using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLRSBehaviour : WeaponBehavior
{

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        target = FindAnyObjectByType<CIWSBehaviour>().gameObject;
        coroutine = LaunchMissileCoroutine();
        StartCoroutine(coroutine);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
