using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class TimeGetter
{
    private AppManager _manager;
    private TimeSpan _utcOffset;

    private const string timeApi1 = "https://worldtimeapi.org/api/timezone/etc/utc"; // "datetime":"2022-06-08T13:16:01.513994+00:00"
    private const string timeApi2 = "https://www.timeapi.io/api/Time/current/zone?timeZone=utc"; //"dateTime":"2022-06-08T13:44:36.5332769"

    private string _dateTime;
    private Regex _timeRegex;
    private TimeOfDay _time;

    public static event Action<TimeOfDay> OnTimeReceived;

    public TimeGetter (TimeSpan utcOffset, AppManager manager)
    {
        _utcOffset = utcOffset;
        _manager = manager;
        _timeRegex = new Regex(@"\d{2}:\d{2}:\d{2}");
        _time = new TimeOfDay();
    }

    public void GetInternetTime()
    {
        _time.Unset();
        _manager.StartCoroutine(TryGetApiTime(timeApi1));
        _manager.StartCoroutine(TryGetApiTime(timeApi2));
    }

    IEnumerator TryGetApiTime(string api)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(api))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                if (!_time.IsSet)
                {
                    _dateTime = request.downloadHandler.text;

                    if (_timeRegex.IsMatch(_dateTime))
                    {
                        string[] timeParts = _timeRegex.Match(_dateTime).Value.Split(':');

                        _time.SetNewTime(int.Parse(timeParts[0]) + _utcOffset.Hours, int.Parse(timeParts[1]) + _utcOffset.Minutes, int.Parse(timeParts[2]));
                        OnTimeReceived?.Invoke(_time);
                    }
                }
            }
        }
    }
}

public class TimeOfDay
{
    private bool _set;
    public bool IsSet => _set;

    public float HoursParted { get { return Mathf.Repeat((float)TimeInSeconds / 3600f, 24f); } }
    public int Hours { get { return Mathf.FloorToInt(HoursParted); } }

    public float MinutesParted { get { return Mathf.Repeat((float)TimeInSeconds / 60f, 60f); } }
    public int Minutes { get { return Mathf.FloorToInt(MinutesParted); } }

    public float SecondsParted { get { return Mathf.Repeat((float)TimeInSeconds, 60f); } }
    public int Seconds { get { return Mathf.FloorToInt(SecondsParted); } }

    public double TimeInSeconds { get; private set; }

    private string format = "00";

    public TimeOfDay(int hours, int minutes, int seconds)
    {
        SetNewTime(hours, minutes, seconds);
    }

    public TimeOfDay()
    {
        SetNewTime(0, 0, 0);
    }

    public void SetNewTime(int hours, int minutes, int seconds)
    {
        _set = true;
        TimeInSeconds = hours * 60 * 60 + minutes * 60 + seconds;
    }

    public void AddSeconds(float seconds)
    {
        TimeInSeconds += seconds;
        if (TimeInSeconds > 86400) TimeInSeconds -= 86400;
    }

    public override string ToString()
    {
        return $"{Hours.ToString(format)}:{Minutes.ToString(format)}:{Seconds.ToString(format)}";
    }

    public void Unset()
    {
        _set = false;
    }
}