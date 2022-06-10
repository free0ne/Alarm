using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowDragger : MonoBehaviour
{
    [SerializeField] private RectTransform _clockCenter;
    [SerializeField] private Camera _camera;
    [SerializeField] private AnalogClock _analogClock;
    [SerializeField] private AlarmSetter _alarm;

    private Vector2 _targetVector;
    private Vector3 _clockCenterScreenPosition;
    private float _targetRotation;
    private float _previousRotation;

    private void Awake()
    {
        _targetVector = Vector2.zero;
        _previousRotation = -1f;
    }

    public void DragHandlerMinutes(BaseEventData data)
    {
        GeneralDragCalculations(data);

        if ((_previousRotation > 330f && _previousRotation < 360f) && (_targetRotation >= 0f && _targetRotation < 30f))
        {
            _analogClock.SetTimeFromMinutesRotation(_alarm.TempTime, _targetRotation, 1);
        }
        else if ((_previousRotation >= 0f && _previousRotation < 30f) && (_targetRotation >= 330f && _targetRotation < 360f))
        {
            _analogClock.SetTimeFromMinutesRotation(_alarm.TempTime, _targetRotation, -1);
        }
        else _analogClock.SetTimeFromMinutesRotation(_alarm.TempTime, _targetRotation, 0);

        _alarm.UpdateAllClocks();

        _previousRotation = _targetRotation;
    }

    public void DragHandlerHours(BaseEventData data)
    {
        GeneralDragCalculations(data);

        bool isMorning = _alarm.TempTime.Hours < 12;


        if ((_previousRotation > 330f && _previousRotation < 360f) && (_targetRotation >= 0f && _targetRotation < 30f)
            || (_previousRotation >= 0f && _previousRotation < 30f) && (_targetRotation >= 330f && _targetRotation < 360f))
        {
            _analogClock.SetTimeFromHoursRotation(_alarm.TempTime, _targetRotation, !isMorning);
        }
        else _analogClock.SetTimeFromHoursRotation(_alarm.TempTime, _targetRotation, isMorning);

        _alarm.UpdateAllClocks();

        _previousRotation = _targetRotation;
    }

    private void GeneralDragCalculations(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        _clockCenterScreenPosition = _camera.WorldToScreenPoint(_clockCenter.position);
        _targetVector.x = pointerData.position.x - _clockCenterScreenPosition.x;
        _targetVector.y = pointerData.position.y - _clockCenterScreenPosition.y;

        _targetRotation = Vector3.Angle(Vector3.up, _targetVector);
        if (_targetVector.x < 0.0f) _targetRotation = 360.0f - _targetRotation;
    }
}
