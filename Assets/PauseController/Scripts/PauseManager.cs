using Assets.PauseController.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : SceneSingleton<PauseManager>
{

    #region Private Fields
    bool _pauseOnLostFocus = true;
    List<IPausable> _pausableMembers = new List<IPausable>();
    IResettable _resetabbleMember;
    IQuittable _quittableMember;

    #endregion Private Fields

    #region Public Properties

    public IPausable TimePause { get; set; }

    public bool IsGameEnded { get; set; }

    public bool IsPaused { get; private set; }

    #endregion Public Properties

    #region Public Methods

    public void AddToIPausableMembers(IPausable iPausable)
    {
        _pausableMembers.Add(iPausable);
    }

    public void SetIQuittableMembers(IQuittable iQuittable)
    {
        if (_quittableMember == null)
        {
            _quittableMember = iQuittable;
        }
        else
        {
            throw new System.InvalidOperationException("QuittableMember have already been setted");
        }
    }

    public void SetIResettableMember(IResettable iResettable)
    {
        if (_resetabbleMember == null)
        {
            _resetabbleMember = iResettable;
        }
        else
        {
            throw new System.InvalidOperationException("ResettableMemember have already been setted");
        }
    }

    public void OnPause()
    {
        LogManager.Log("PauseManager.OnPause");
        IsPaused = true;
        FireOnPauseChanged();
    }

    public void OnQuit()
    {
        LogManager.Log("PauseManager.OnQuit");
        OnResume();
        if (_quittableMember != null)
        {
            _quittableMember.OnQuit(); 
        }
    }

    public void OnReset()
    {
        LogManager.Log("PauseManager.OnReset");
        IsPaused = false;
        OnResume();
        if (_resetabbleMember != null)
        {
            _resetabbleMember.OnReset();
        }
    }

    public void OnResume()
    {
        LogManager.Log("PauseManager.OnResume");
        IsPaused = false;
        FireOnPauseChanged();
    }

    #endregion Public Methods

    #region Private Methods

    void FireOnPauseChanged()
    {
        CallIPausableMethods(TimePause);
        foreach (var iPausable in _pausableMembers)
        {
            CallIPausableMethods(iPausable);
        }
    }

    void CallIPausableMethods(IPausable iPausable)
    {
        if (IsPaused)
        {
            if (iPausable != null)
            {
                iPausable.OnPause();
            }
        }
        else
        {
            if (iPausable != null)
            {
                iPausable.OnResume(); 
            }
        }
    }

    void OnDestroy()
    {
        _pausableMembers.Clear();
        _quittableMember = null;
        _resetabbleMember = null;
    }

    void OnApplicationPause(bool pause)
    {
        if (!IsPaused && pause && _pauseOnLostFocus && !IsGameEnded)
        {
            OnPause();
        }
    }

    void OnApplicationFocus(bool focusStatus)
    {
        if (!IsPaused && !focusStatus && _pauseOnLostFocus && !IsGameEnded)
        {
            OnPause();
        }
    }

    void Start()
    {
        _pauseOnLostFocus |= !Debug.isDebugBuild;

#if UNITY_EDITOR
        _pauseOnLostFocus = false;
#endif
    }

    #endregion Private Methods
}