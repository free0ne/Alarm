using System;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    [SerializeField] private AnalogClock _analogClock;
    [SerializeField] private DigitalClock _digitalClock;
    [SerializeField] private AlarmSetter _alarm;
    [SerializeField] private AlarmPlayer _alarmPlayer;

    private TimeGetter _timeGetter;
    private TimeOfDay _time;
    private const float recheckPeriod = 60 * 60; // resync every hour
    private bool _editingAlarm;

    private float _pausedTime = -1;

    private bool _alarmSet;
    private int _alarmSeconds;

    private void Awake()
    {
    #if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
    #endif
    }

    private void Start()
    {
        _editingAlarm = false;
        _timeGetter = new TimeGetter(TimeZoneInfo.Local.BaseUtcOffset, this);
        CheckAlarm();

        InvokeRepeating(nameof(GetInternetTime), 0, recheckPeriod);
    }

    private void Update()
    {
        if (_editingAlarm == false && _time != null)
        {
            if (_alarmSet)
            {
                if (_time.TimeInSeconds < _alarmSeconds && _time.TimeInSeconds + Time.deltaTime >= _alarmSeconds)
                {
                    _alarmPlayer.StartWaking();
                }
            }    

            _time.AddSeconds(Time.deltaTime);
            _analogClock.SetArrowsRotation(_time, true);
            _digitalClock.SetDigitalTime(_time);
        }
        else
        {
            if (_editingAlarm == true && _time != null) _time.AddSeconds(Time.deltaTime);
        }    
    }

    private void GetInternetTime()
    {
        _timeGetter.GetInternetTime();
    }


    private void ReloadClocks(TimeOfDay newTime)
    {
        _time = newTime;
        Debug.Log($"Reloaded: {_time}");
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) _pausedTime = Time.realtimeSinceStartup;
        else
        {
            if (_pausedTime > 0)
            {
                _time.AddSeconds(Time.realtimeSinceStartup - _pausedTime);
                _pausedTime = -1;
            }
        }
    }

    private void OnEnable()
    {
        TimeGetter.OnTimeReceived += ReloadClocks;
    }

    private void OnDisable()
    {
        TimeGetter.OnTimeReceived -= ReloadClocks;
    }

    public void OnAlarmButtonClicked(bool setAlarm)
    {
        _editingAlarm = !_editingAlarm;
        if (_editingAlarm) _alarm.EnterAlarmEditing(_time);
        else
        {
            _alarm.ExitAlarmEditing(setAlarm);
            CheckAlarm();
        }
    }

    public void OnDigitalPanelClicked()
    {
        if (_editingAlarm) _alarm.SwitchDigitalAlarmEditing();
    }

    private void CheckAlarm()
    {
        if (Storage.CheckSavedAlarm())
        {
            _alarm.ChangeAlarmButtonImage(true);
            _alarmSet = true;
            _alarmSeconds = Storage.GetSavedHours() * 3600 + Storage.GetSavedMinutes() * 60;
        }
        else
        {
            _alarm.ChangeAlarmButtonImage(false);
            _alarmSet = false;
        }
    }
}
