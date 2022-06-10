using UnityEngine;
using UnityEngine.UI;

public class AlarmSetter : MonoBehaviour
{
    [SerializeField] private AnalogClock _analogClock;
    [SerializeField] private Image _hourArrow;
    [SerializeField] private Image _minuteArrow;
    [SerializeField] private GameObject _secondsArrow;
    [SerializeField] private Text _digitalTimeText;
    [SerializeField] private Image _alarmButtonImage;

    [SerializeField] private Color _editingColor;
    [SerializeField] private Sprite _alarmNotSetSprite;
    [SerializeField] private Sprite _alarmSetSprite;

    [SerializeField] private Text _digitalAlarmText;
    [SerializeField] private GameObject _digitalAlarmButtonPanel;
    [SerializeField] private GameObject _alarmAbortButton;

    [SerializeField] private GameObject _hourArrowDragger;
    [SerializeField] private GameObject _minuteArrowDragger;

    private Color _defaultArrowsColor;
    private Color _defaultTextColor;
    private Color _defaultButtonColor;

    private TimeOfDay _tempTime;
    public TimeOfDay TempTime => _tempTime;

    private void Awake()
    {
        _defaultArrowsColor = _hourArrow.color;
        _defaultTextColor = _digitalTimeText.color;
        _defaultButtonColor = _alarmButtonImage.color;

        _tempTime = new TimeOfDay();
    }

    public void EnterAlarmEditing(TimeOfDay time)
    {
        if (Storage.CheckSavedAlarm())
            _tempTime.SetNewTime(Storage.GetSavedHours(), Storage.GetSavedMinutes(), 0);
        else _tempTime.SetNewTime(time.Hours, time.Minutes, 0);
        UpdateAllClocks();

        _hourArrow.color = _minuteArrow.color = _editingColor;
        _secondsArrow.SetActive(false);
        _digitalTimeText.color = _editingColor;
        _alarmButtonImage.color = _editingColor;

        SwitchArrowDraggers(true);

        ChangeAlarmButtonImage(true);
        _alarmAbortButton.SetActive(true);
    }

    public void ExitAlarmEditing(bool setAlarm)
    {
        _hourArrow.color = _minuteArrow.color = _defaultArrowsColor;
        _secondsArrow.SetActive(true);
        _digitalTimeText.color = _defaultTextColor;
        _alarmButtonImage.color = _defaultButtonColor;


        ExitDigitalAlarmEditing();
        SwitchArrowDraggers(false);


        _alarmAbortButton.SetActive(false);

        if (setAlarm)
        {
            ChangeAlarmButtonImage(true);
            Storage.SaveAlarm(_tempTime.Hours, _tempTime.Minutes);
        }
        else
        {
            ChangeAlarmButtonImage(false);
            Storage.UnsetAlarm();
        }

    }

    public void SwitchDigitalAlarmEditing()
    {
        if (!_digitalAlarmText.gameObject.activeSelf)
        {
            _digitalTimeText.gameObject.SetActive(false);
            _digitalAlarmText.text = _digitalTimeText.text.Substring(0, 5);
            _digitalAlarmText.gameObject.SetActive(true);
            _digitalAlarmButtonPanel.SetActive(true);

            SwitchArrowDraggers(false);
        }
        else
        {
            ExitDigitalAlarmEditing();
        }
    }

    private void ExitDigitalAlarmEditing()
    {
        _digitalAlarmText.gameObject.SetActive(false);
        _digitalAlarmButtonPanel.SetActive(false);
        _digitalTimeText.gameObject.SetActive(true);

        SwitchArrowDraggers(true);
    }

    public void OnDigitalAlarmInput(int num)
    {
        if (num != -1)
        {
            switch (_digitalAlarmText.text.Length)
            {
                case 0:
                    if (num == 0 || num == 1 || num == 2) _digitalAlarmText.text += num;
                    break;
                case 1:
                    switch (int.Parse(_digitalAlarmText.text))
                    {
                        case 0:
                        case 1:
                            _digitalAlarmText.text += $"{num}:";
                            break;
                        case 2:
                            if (num == 0 || num == 1 || num == 2 || num == 3) _digitalAlarmText.text += $"{num}:";
                            break;
                    }
                    break;
                case 3:
                    if (num == 0 || num == 1 || num == 2 || num == 3 || num == 4 || num == 5) _digitalAlarmText.text += num;
                    break;
                case 4:
                    _digitalAlarmText.text += num;

                    _tempTime.SetNewTime(int.Parse(_digitalAlarmText.text.Substring(0, 2)), int.Parse(_digitalAlarmText.text.Substring(3, 2)), 0);

                    UpdateAllClocks();

                    break;
            }    
        }
        else
        {
            switch (_digitalAlarmText.text.Length)
            {
                case 1:
                case 2:
                case 4:
                case 5:
                    _digitalAlarmText.text = _digitalAlarmText.text.Substring(0, _digitalAlarmText.text.Length - 1);
                    break;
                case 3:
                    _digitalAlarmText.text = _digitalAlarmText.text.Substring(0, 1);
                    break;
            }
        }
    }

    public void UpdateAllClocks()
    {
        _digitalTimeText.text = _tempTime.ToString();
        _analogClock.SetArrowsRotation(_tempTime, false);
    }

    private void SwitchArrowDraggers(bool newState)
    {
        _hourArrowDragger.SetActive(newState);
        _minuteArrowDragger.SetActive(newState);
    }

    public void ChangeAlarmButtonImage(bool newState)
    {
        if (newState) _alarmButtonImage.sprite = _alarmSetSprite;
        else _alarmButtonImage.sprite = _alarmNotSetSprite;
    }
}
