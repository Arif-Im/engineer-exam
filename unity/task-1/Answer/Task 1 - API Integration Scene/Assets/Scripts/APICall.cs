using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APICall : MonoBehaviour
{
    [System.Serializable]
    public class Prayer
    {
        public string asr;
        public string fajr;
        public string isha;
        public string day;
        public string dhuhr;
        public string maghrib;
        public string hijri;
        public string syuruk;
    }

    [System.Serializable]
    public class Calendar
    {
        public string zone;
        public int year;
        public string month;
        public string last_updated;
        public List<Prayer> prayers;
    }

    public Calendar calendar;
    APIDisplay display;

    // Start is called before the first frame update
    void Start()
    {
        display = FindObjectOfType<APIDisplay>();
        StartCoroutine(GetRequest("https://api.waktusolat.app/v2/solat/sgr01"));
    }
    
    IEnumerator GetRequest(string uri)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(String.Format("Error sending web request: {0}", webRequest.error));
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Web Request Succeeded");
                SetupDisplay(webRequest);
                break;
        }
    }

    private void SetupDisplay(UnityWebRequest webRequest)
    {
        SetCalendarLastUpdated(webRequest);
        ConvertAllPrayerTimestamps();
        display.SetCalendarTexts(calendar.zone, calendar.year.ToString(), calendar.month, calendar.last_updated);
        foreach(Prayer prayer in calendar.prayers){
            display.CreatePrayerSlot(
                prayer.day,
                prayer.fajr,
                prayer.syuruk,
                prayer.dhuhr,
                prayer.asr,
                prayer.maghrib,
                prayer.isha
            );
        }
    }

    private void SetCalendarLastUpdated(UnityWebRequest webRequest)
    {
        calendar = JsonUtility.FromJson<Calendar>(webRequest.downloadHandler.text);

        string isoTimestamp = calendar.last_updated;

        DateTime dateTime = DateTime.Parse(isoTimestamp, null, System.Globalization.DateTimeStyles.RoundtripKind);

        DateTime localTime = dateTime.ToLocalTime();

        calendar.last_updated = localTime.ToString();
    }

    private void ConvertAllPrayerTimestamps()
    {
        foreach (Prayer prayer in calendar.prayers)
        {
            prayer.asr = ConvertTimestampToDateTime(prayer.asr);
            prayer.fajr = ConvertTimestampToDateTime(prayer.fajr);
            prayer.isha = ConvertTimestampToDateTime(prayer.isha);
            prayer.dhuhr = ConvertTimestampToDateTime(prayer.dhuhr);
            prayer.maghrib = ConvertTimestampToDateTime(prayer.maghrib);
            prayer.syuruk = ConvertTimestampToDateTime(prayer.syuruk);
        }
    }

    protected string ConvertTimestampToDateTime(string prayer)
    {
        long timestamp = (long)Convert.ToDouble(prayer);

        DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
        DateTime localTime = dateTime.ToLocalTime();
        
        return localTime.ToString();
    }
}
