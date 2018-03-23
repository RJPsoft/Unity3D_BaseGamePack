using Assets.PauseController.Scripts;
using UnityEngine;

public class TimePause : MonoBehaviour, IPausable
{
    #region Public Fields

    public float pauseDelay = 0.3f;

    #endregion Public Fields

    #region Private Fields

    float _timeScale;

    #endregion Private Fields

    public void OnPause()
    {
        _timeScale = Time.timeScale;
        Invoke("StopTime", pauseDelay);
    }

    public void OnResume()
    {
        if (IsInvoking("StopTime"))
        {
            CancelInvoke("StopTime");
        }

        Time.timeScale = _timeScale;
    }

    #region Private Methods

    void Start()
    {
        _timeScale = Time.timeScale;
        OnResume();
        PauseManager.Instance.TimePause = this;
    }

#pragma warning disable CC0091 // Use static method
    void StopTime()
    {
        Time.timeScale = 0;
    }
#pragma warning restore CC0091 // Use static method

    #endregion Private Methods
}