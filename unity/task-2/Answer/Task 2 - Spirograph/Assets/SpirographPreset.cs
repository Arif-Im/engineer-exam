using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpirographPreset : MonoBehaviour
{
    protected CircleGenerator circleGenerator;

    [SerializeField] protected float fixedGearRadius;
    [SerializeField] protected float movingGearRadius;
    [SerializeField] protected float drawOffsetRadius;

    // Start is called before the first frame update
    void Start()
    {
        circleGenerator = FindObjectOfType<CircleGenerator>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            circleGenerator.CreatePreset(fixedGearRadius, movingGearRadius, drawOffsetRadius);
        });
    }
}
