using UnityEngine;

public static class Storage
{
    private const string flagKey = "AlarmSet";
    private const string hoursKey = "Hours";
    private const string minutesKey = "Minutes";

    public static bool CheckSavedAlarm()
    {
        if (PlayerPrefs.HasKey(flagKey))
        {
            if (PlayerPrefs.GetInt(flagKey) == 1)
                return true;
            else return false;
        }
        else
            return false;
    }

    public static int GetSavedHours()
    {
        if (CheckSavedAlarm())
            return PlayerPrefs.GetInt(hoursKey);
        else return 0;
    }

    public static int GetSavedMinutes()
    {
        if (CheckSavedAlarm())
            return PlayerPrefs.GetInt(minutesKey);
        else return 0;
    }

    public static void SaveAlarm(int hours, int minutes)
    {
        PlayerPrefs.SetInt(flagKey, 1);
        PlayerPrefs.SetInt(hoursKey, hours);
        PlayerPrefs.SetInt(minutesKey, minutes);
        PlayerPrefs.Save();
    }

    public static void UnsetAlarm()
    {
        PlayerPrefs.SetInt(flagKey, -1);
        PlayerPrefs.Save();
    }
}
