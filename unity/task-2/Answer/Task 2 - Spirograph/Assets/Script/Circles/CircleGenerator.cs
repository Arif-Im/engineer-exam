using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGenerator : MonoBehaviour
{
    protected float thetaScale = 0.1f;
    protected float accumulatedTime = 0.0f;
    [SerializeField] protected Vector2 fixedGearPosition = Vector2.zero;
    protected float drawCircleRadius = 0.1f;
    protected float fixedMovingRatio = 0.0f;
    protected LineRenderer lineRenderer;
    protected int lineIndex = 0;

    [Header("Circle Radius")]
    [SerializeField] protected float defaultFixedGearRadius = 5.0f;
    [SerializeField] protected float fixedGearRadius = 5.0f;
    [SerializeField] protected float defaultMovingGearRadius = 1.5f;
    [SerializeField] protected float movingGearRadius = 1.5f;
    [SerializeField] protected float defaultDrawOffsetRadius = 1.2f;
    [SerializeField] protected float drawOffsetRadius = 1.2f;

    [Header("Color")]
    [SerializeField] protected float morphTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        movingGearRadius = defaultMovingGearRadius;
        drawOffsetRadius = defaultDrawOffsetRadius;
        fixedGearRadius = defaultFixedGearRadius;

        fixedMovingRatio = fixedGearRadius / movingGearRadius;
        CreateLineRenderer();
        CreatePreset(defaultFixedGearRadius, defaultMovingGearRadius, drawOffsetRadius);
    }

    public void CreatePreset(float fixedGearRadius, float movingGearRadius, float drawOffsetRadius)
    {
        ClearCircle();
        
        this.fixedGearRadius = fixedGearRadius;
        this.movingGearRadius = movingGearRadius;
        this.drawOffsetRadius = drawOffsetRadius;

        int index = 0;
        for (float i = 0; i < 500; i += Time.deltaTime)
        {
            index++;
            DrawSpirograph(out Vector2 movingGearOffset, out Vector2 drawPosition, i, index);
        }
    }

    private void CreateLineRenderer()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = lineRenderer.endWidth = drawCircleRadius;
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.startColor = lineRenderer.endColor = Color.white;
        lineRenderer.positionCount = lineIndex;
        StartCoroutine(RandomMultiColorMorphing(lineRenderer, morphTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            accumulatedTime += Time.deltaTime * 2;
            lineIndex++;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            ClearCircle();
        }

        DrawSpirograph(out Vector2 movingGearOffset, out Vector2 drawPosition, accumulatedTime, lineIndex);

        DrawDebugCircle(fixedGearPosition, fixedGearRadius, 0);
        DrawDebugCircle(movingGearOffset, movingGearRadius, 0);
        DrawDebugCircle(drawPosition, drawCircleRadius, 0);
    }

    private void DrawSpirograph(out Vector2 movingGearOffset, out Vector2 drawPosition, float timeElapsed, int index)
    {
        movingGearOffset = new(
            (fixedGearRadius - movingGearRadius) * Mathf.Cos(timeElapsed),
            (fixedGearRadius - movingGearRadius) * Mathf.Sin(timeElapsed)
        );
        Vector2 drawOffset = new(
            drawOffsetRadius * Mathf.Cos(-timeElapsed * fixedMovingRatio),
            drawOffsetRadius * Mathf.Sin(-timeElapsed * fixedMovingRatio)
        );

        Vector2 movingGearPosition = fixedGearPosition + movingGearOffset;
        drawPosition = movingGearPosition + drawOffset;
        if (index > 0)
        {
            lineRenderer.positionCount = index;
            lineRenderer.SetPosition(index - 1, drawPosition);
        }
    }

    public void DrawDebugCircle(Vector2 position, float radius, float duration)
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

    public void ClearCircle()
    {
        accumulatedTime = 0.0f;
        lineRenderer.positionCount = 0;
        lineIndex = 0;
    }

    public void SetFixedCircleRadius(float value){
        fixedGearRadius = defaultFixedGearRadius + value;
    }
    public void SetMovingCircleRadius(float value){
        movingGearRadius = defaultMovingGearRadius + value;
    }
    public void SetDrawOffsetRadius(float value){
        drawOffsetRadius = defaultDrawOffsetRadius + value;
    }

    IEnumerator RandomMultiColorMorphing(LineRenderer lineRendererToChange, float timeToMorph)
    {
        float time = 0;
        while(true)
        {
            GradientColorKey[] initialColorKeys = lineRendererToChange.colorGradient.colorKeys;
            GradientColorKey[] newColorKeys = GenerateRandomColorKeys(initialColorKeys);
            time = 0;
            while(time<timeToMorph)
            {
                time += Time.deltaTime;
                float progress = time/timeToMorph;
                GradientColorKey[] currentColorKeys = GradientColorKeyLerp(initialColorKeys, newColorKeys, progress);
                Gradient tempGradient = lineRendererToChange.colorGradient;
                tempGradient.colorKeys = currentColorKeys;
                lineRendererToChange.colorGradient = tempGradient;
                yield return null;
            }
            yield return null;
        }
    }

    GradientColorKey[] GradientColorKeyLerp(GradientColorKey[] initialColorKeys, GradientColorKey[] endColorKeys, float progress)
    {
        GradientColorKey[] newColorKeys = new GradientColorKey[initialColorKeys.Length];
        for(int i = 0; i < newColorKeys.Length; i++)
        {
            newColorKeys[i].color = Color.Lerp(initialColorKeys[i].color, endColorKeys[i].color, progress);
            newColorKeys[i].time = initialColorKeys[i].time;
        }
        return newColorKeys;
    }

    GradientColorKey[] GenerateRandomColorKeys(GradientColorKey[] incomingColorKeys)
    {
        GradientColorKey[] newColorKeys = new GradientColorKey[incomingColorKeys.Length];
        for(int i = 0; i <newColorKeys.Length; i++)
        {
            newColorKeys[i].color = RandomColor();
            newColorKeys[i].time = incomingColorKeys[i].time;
        }
        return newColorKeys;
    }

    Color RandomColor(){
        return new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f));
    }
}
