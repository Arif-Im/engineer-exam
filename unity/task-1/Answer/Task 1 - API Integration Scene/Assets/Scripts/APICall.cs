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

    // Start is called before the first frame update
    void Start()
    {
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
                calendar = JsonUtility.FromJson<Calendar>(webRequest.downloadHandler.text);

                string isoTimestamp = calendar.last_updated;

                // Parse the ISO 8601 string to DateTime
                DateTime dateTime = DateTime.Parse(isoTimestamp, null, System.Globalization.DateTimeStyles.RoundtripKind);

                // Optionally, convert to local time
                DateTime localTime = dateTime.ToLocalTime();

                calendar.last_updated = localTime.ToString();
                ConvertAllPrayerTimestamps();
                break;
        }
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
