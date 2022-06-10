using UnityEngine;
using UnityEngine.UI;

public class LayoutSwitcher : MonoBehaviour
{
    [SerializeField] private RectTransform _analogPanel;
    [SerializeField] private RectTransform _digitalAndAlarmPanel;
    //[SerializeField] private RectTransform _buttonPanel;

    private Vector4 _analogPortrait = new Vector4(0.143f, 0.5f, 0.857f, 0.9f); //min x y max x y
    private Vector4 _analogLandscape = new Vector4(0.05f, 0.1f, 0.45f, 0.9f);

    private Vector4 _digalPortrait = new Vector4(0.143f, 0.0823f, 0.857f, 0.377f);
    private Vector4 _digalLandscape = new Vector4(0.55f, 0.1f, 0.95f, 0.9f);

    //private Vector4 _digitButtonsPortrait = new Vector4(103.68f, -405.14f, 103.7f, 344.86f); //left top right botton
    //private Vector4 _digitButtonsLandscape = new Vector4(-748.32f, 161.86f, 955.68f, -222.14f); //left top right botton


    private void OnRectTransformDimensionsChange()
    {
        SetOrientationLayout();
    }

    private void SetPositionByAnchor(RectTransform transform, Vector4 positions)
    {
        transform.anchorMin = new Vector2(positions.x, positions.y);
        transform.anchorMax = new Vector2(positions.z, positions.w);

        transform.anchoredPosition = Vector3.zero;
    }

    private void SetPosition(RectTransform transform, Vector4 positions)
    {
        transform.offsetMin = new Vector2(positions.x, positions.w);
        transform.offsetMax = new Vector2(positions.z, positions.y);
    }

    public void SetOrientationLayout()
    {
        if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
        {
            SetPositionByAnchor(_analogPanel, _analogPortrait);
            SetPositionByAnchor(_digitalAndAlarmPanel, _digalPortrait);
            //SetPosition(_buttonPanel, _digitButtonsPortrait);
        }
        else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            SetPositionByAnchor(_analogPanel, _analogLandscape);
            SetPositionByAnchor(_digitalAndAlarmPanel, _digalLandscape);
            //SetPosition(_buttonPanel, _digitButtonsLandscape);
        }
    }
}
