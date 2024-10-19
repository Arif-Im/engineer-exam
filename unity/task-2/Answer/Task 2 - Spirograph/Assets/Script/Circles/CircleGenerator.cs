using System;
using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;

public class CircleGenerator : MonoBehaviour
{
    protected float thetaScale = 0.1f;
    protected float accumulatedTime = 0.0f;
    protected float fixedGearRadius = 5.0f;
    protected Vector2 fixedGearPosition = Vector2.zero;
    protected float movingGearRadius = 1.5f;
    protected float drawOffsetRadius = 1.2f;
    protected float drawCircleRadius = 0.1f;
    protected float fixedMovingRatio = 0.0f;
    protected LineRenderer lineRenderer;
    protected int lineIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        DrawCircle(fixedGearPosition, fixedGearRadius, Mathf.Infinity);
        fixedMovingRatio = fixedGearRadius / movingGearRadius;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = lineRenderer.endWidth = drawCircleRadius;
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.startColor = lineRenderer.endColor = Color.white;
        lineRenderer.positionCount = lineIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            accumulatedTime += Time.deltaTime * 2;
            lineIndex++;
        }

        Vector2 movingGearOffset = new(
            (fixedGearRadius - movingGearRadius) * Mathf.Cos(accumulatedTime),
            (fixedGearRadius - movingGearRadius) * Mathf.Sin(accumulatedTime)
        );

        Vector2 drawOffset = new(
            drawOffsetRadius * Mathf.Cos(-accumulatedTime * fixedMovingRatio),
            drawOffsetRadius * Mathf.Sin(-accumulatedTime * fixedMovingRatio)
        );

        Vector2 movingGearPosition = fixedGearPosition + movingGearOffset;
        Vector2 drawPosition = movingGearPosition + drawOffset;

        DrawCircle(movingGearOffset, movingGearRadius, 0);
        DrawCircle(drawPosition, drawCircleRadius, 0);
        if(lineIndex > 0)
        {
            lineRenderer.positionCount = lineIndex;
            lineRenderer.SetPosition(lineIndex-1, drawPosition);
        }
    }

    public void DrawCircle(Vector2 position, float radius, float duration)
    {
        bool isStarting = true;
        Vector2 currentDrawPoint = new(0,0);

        for(float theta = 0; theta < (2 * Mathf.PI) + thetaScale; theta += thetaScale){
            Vector2 newDrawPoint = new(
                radius * Mathf.Cos(theta) + position.x, 
                radius * Mathf.Sin(theta) + position.y
            );
            
            if(isStarting)
            {
                currentDrawPoint = newDrawPoint;
                isStarting = false;
            }

            Debug.DrawLine(currentDrawPoint, newDrawPoint, Color.red, duration);

            currentDrawPoint = newDrawPoint;
        }
    }
}
