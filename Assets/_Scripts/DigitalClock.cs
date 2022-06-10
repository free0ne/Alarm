using UnityEngine;
using UnityEngine.UI;

public class DigitalClock : MonoBehaviour
{
    [SerializeField] private Text _digitalTime;

    public void SetDigitalTime(TimeOfDay time)
    {
        _digitalTime.text = time.ToString();
    }
}
