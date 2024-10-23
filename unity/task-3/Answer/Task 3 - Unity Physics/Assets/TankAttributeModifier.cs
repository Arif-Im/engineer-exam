using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TankAttributeModifier : MonoBehaviour
{
    protected MLRSBehaviour mLRSTank;
    protected CIWSBehaviour cIWSTank;

    protected Slider mLRSTankSpeed;
    protected Slider mLRSHomingSpeed;
    protected Slider mLRSProjectileSpeed;
    protected Slider cIWSTankSpeed;
    protected Slider cIWSProjectileSpeed;

    // Start is called before the first frame update
    void Start()
    {
        mLRSTank = FindObjectOfType<MLRSBehaviour>();
        cIWSTank = FindObjectOfType<CIWSBehaviour>();

        mLRSTankSpeed = transform.Find("MLRS Tank Speed").Find("MLRS Tank Speed Slider").GetComponent<Slider>();
        mLRSHomingSpeed = transform.Find("MLRS Homing Speed").Find("MLRS Homing Speed Slider").GetComponent<Slider>();
        mLRSProjectileSpeed = transform.Find("MLRS Projectile Speed").Find("MLRS Projectile Speed Slider").GetComponent<Slider>();
        cIWSTankSpeed = transform.Find("CIWS Tank Speed").Find("CIWS Tank Speed Slider").GetComponent<Slider>();
        cIWSProjectileSpeed = transform.Find("CIWS Projectile Speed").Find("CIWS Projectile Speed Slider").GetComponent<Slider>();

        mLRSTankSpeed.onValueChanged.AddListener((value) => {
            mLRSTank.SetTankSpeed(value);
        });
        mLRSHomingSpeed.onValueChanged.AddListener((value) => {
            mLRSTank.SetHomingSpeed(value);
        });
        mLRSProjectileSpeed.onValueChanged.AddListener((value) => {
            mLRSTank.SetBulletSpeed(value);
        });
        cIWSTankSpeed.onValueChanged.AddListener((value) => {
            cIWSTank.SetTankSpeed(value);
        });
        cIWSProjectileSpeed.onValueChanged.AddListener((value) => {
            cIWSTank.SetBulletSpeed(value);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
