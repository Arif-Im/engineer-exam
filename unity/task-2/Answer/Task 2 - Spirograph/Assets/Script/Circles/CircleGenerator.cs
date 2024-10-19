using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGenerator : MonoBehaviour
{
    protected float accumulatedTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.DrawLine(Vector3.zero, new Vector3(5, 0, 0), Color.red, 300.0f);
    }

    // Update is called once per frame
    void Update()
    {
        accumulatedTime += Time.deltaTime;
    }
}
