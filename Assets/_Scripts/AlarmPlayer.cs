using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AlarmPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _wakerPanel;

    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void OnStopAlarmButtonClicked()
    {
        StopWaking();
    }

    public void StartWaking()
    {
        _wakerPanel.SetActive(true);
        _audio.Play();
    }

    private void StopWaking()
    {
        _wakerPanel.SetActive(false);
        _audio.Stop();
    }
}
