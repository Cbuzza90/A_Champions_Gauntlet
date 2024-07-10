using UnityEngine;

public class FrameRateCapper : MonoBehaviour
{
    [SerializeField] private int targetFrameRate = 360;

    void Start()
    {
        Debug.Log("Setting target frame rate to " + targetFrameRate);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }
}
