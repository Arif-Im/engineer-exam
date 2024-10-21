using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class APIDisplay : MonoBehaviour
{
    protected APICall apiCall;
    protected TextMeshProUGUI zoneText;
    protected TextMeshProUGUI yearText;
    protected TextMeshProUGUI monthText;
    protected TextMeshProUGUI lastUpdatedText;
    [SerializeField] protected GameObject prayerSlotPrefab;
    protected Transform content;

    // Start is called before the first frame update
    void Start()
    {
        apiCall = GetComponent<APICall>();

        zoneText = transform.Find("Calendar").Find("Zone").Find("Zone Text").GetComponent<TextMeshProUGUI>();
        yearText = transform.Find("Calendar").Find("Year").Find("Year Text").GetComponent<TextMeshProUGUI>();
        monthText = transform.Find("Calendar").Find("Month").Find("Month Text").GetComponent<TextMeshProUGUI>();
        lastUpdatedText = transform.Find("Calendar").Find("Last Updated").Find("Last Updated Text").GetComponent<TextMeshProUGUI>();

        content = transform.Find("Scroll Area").Find("Content");
    }
    
    public void SetCalendarTexts(string zone, string year, string month, string lastUpdated)
    {
        zoneText.text = zone;
        yearText.text = year;
        monthText.text = month;
        lastUpdatedText.text = lastUpdated;
    }
    
    public void CreatePrayerSlot(
        string day, 
        string subuh, 
        string syuruk, 
        string zohor, 
        string asar, 
        string maghrib, 
        string isyak)
    {
        GameObject prayerSlot = Instantiate<GameObject>(prayerSlotPrefab);
        prayerSlot.transform.parent = content;

        TextMeshProUGUI dayText = prayerSlot.transform.Find("Day").Find("Day Text").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI subuhText = prayerSlot.transform.Find("Prayers").Find("Subuh Slot").Find("Subuh Info").Find("Subuh Info Text").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI syurukText = prayerSlot.transform.Find("Prayers").Find("Syuruk Slot").Find("Syuruk Info").Find("Syuruk Info Text").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI zohorText = prayerSlot.transform.Find("Prayers").Find("Zohor Slot").Find("Zohor Info").Find("Zohor Info Text").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI asarText = prayerSlot.transform.Find("Prayers").Find("Asar Slot").Find("Asar Info").Find("Asar Info Text").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI maghribText = prayerSlot.transform.Find("Prayers").Find("Maghrib Slot").Find("Maghrib Info").Find("Maghrib Info Text").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI isyakText = prayerSlot.transform.Find("Prayers").Find("Isyak Slot").Find("Isyak Info").Find("Isyak Info Text").GetComponent<TextMeshProUGUI>();

        dayText.text = day;
        subuhText.text = subuh;
        syurukText.text = syuruk;
        zohorText.text = zohor;
        asarText.text = asar;
        maghribText.text = maghrib;
        isyakText.text = isyak;
    }
}
