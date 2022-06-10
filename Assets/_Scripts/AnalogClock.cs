using UnityEngine;
using UnityEngine.UI;

public class AnalogClock : MonoBehaviour
{
    [SerializeField] private Text _analogDigitPrefab;
    [SerializeField] private RectTransform _analogDigitsParent;

    [SerializeField] private RectTransform _hourArrow;
    [SerializeField] private RectTransform _minuteArrow;
    [SerializeField] private RectTransform _secondsArrow;

    private const float hoursToDegrees = 360f / 12f;
    private const float minutesToDegrees = 360f / 60f;
    private const float secondsToDegrees = 360f / 60f;

    private void Awake()
    {
        SetDigitsPosition(0.38f);
    }

    private void SetDigitsPosition(float radius)
    {
        Vector2 newPosition;
        Text newDigit;
        for (int i = 1; i < 13; i++)
        {
            newPosition.y = 0.5f + Mathf.Cos(Mathf.Deg2Rad * i * 30) * radius;
            newPosition.x = 0.5f + Mathf.Sin(Mathf.Deg2Rad * i * 30) * radius;

            newDigit = Instantiate(_analogDigitPrefab, _analogDigitsParent);
            newDigit.rectTransform.anchorMin = newDigit.rectTransform.anchorMax = newPosition;

            newDigit.rectTransform.anchoredPosition = Vector3.zero;
            newDigit.text = i.ToString();
        }
    }

    public void SetArrowsRotation(TimeOfDay time, bool enableAll)
    {
        if (enableAll)
        {
            _hourArrow.gameObject.SetActive(true);
            _minuteArrow.gameObject.SetActive(true);
            _secondsArrow.gameObject.SetActive(true);
        }

        _hourArrow.localRotation = Quaternion.Euler(0f, 0f, time.HoursParted * -hoursToDegrees);
        _minuteArrow.localRotation = Quaternion.Euler(0f, 0f, time.MinutesParted * -minutesToDegrees);
        _secondsArrow.localRotation = Quaternion.Euler(0f, 0f, time.SecondsParted * -secondsToDegrees);
    }

    public void SetTimeFromMinutesRotation(TimeOfDay timeToSet, float rotation, int hourChange)
    {
        if (rotation == 360f) rotation = 0; //just to be safe

        int newMinutes = Mathf.FloorToInt(rotation / minutesToDegrees);

        int newHours = timeToSet.Hours;

        if (hourChange == 1)
        {
            newHours = (newHours + 1) % 24;
        }
        else if (hourChange == -1)
        {
            if (newHours == 0) newHours = 23;
            else newHours -= 1;
        }
        
        timeToSet.SetNewTime(newHours, newMinutes, 0);
    }

    public void SetTimeFromHoursRotation(TimeOfDay timeToSet, float rotation, bool morning)
    {
        //init state

        if (rotation == 360f) rotation = 0; //just to be safe

        float partedHours = rotation / hoursToDegrees;

        int newHours = Mathf.FloorToInt(partedHours);

        int newMinutes = Mathf.FloorToInt((partedHours - newHours) * 60f);
        if (!morning) newHours += 12;

        timeToSet.SetNewTime(newHours, newMinutes, 0);
    }

}
