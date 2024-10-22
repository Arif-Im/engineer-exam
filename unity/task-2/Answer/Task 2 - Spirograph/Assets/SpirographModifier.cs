using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpirographModifier : MonoBehaviour
{
    protected CircleGenerator circleGenerator;
    protected Slider rollingCircleRadiusSlider;
    protected Slider drawPointOffsetSlider;
    protected Slider fixedCircleRadiusSlider;

    // Start is called before the first frame update
    void Start()
    {
        circleGenerator = FindObjectOfType<CircleGenerator>();

        rollingCircleRadiusSlider = gameObject.transform.Find("Rolling Circle Radius").Find("Rolling Circle Radius Slider").GetComponent<Slider>();
        drawPointOffsetSlider = gameObject.transform.Find("Draw Point Offset").Find("Draw Point Offset Slider").GetComponent<Slider>();
        fixedCircleRadiusSlider = gameObject.transform.Find("Fixed Circle Radius").Find("Fixed Circle Radius Slider").GetComponent<Slider>();

        rollingCircleRadiusSlider.onValueChanged.AddListener((value) => {
            circleGenerator.SetMovingCircleRadius(value);
        });
        drawPointOffsetSlider.onValueChanged.AddListener((value) => {
            circleGenerator.SetDrawOffsetRadius(value);
        });
        fixedCircleRadiusSlider.onValueChanged.AddListener((value) => {
            circleGenerator.SetFixedCircleRadius(value);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
