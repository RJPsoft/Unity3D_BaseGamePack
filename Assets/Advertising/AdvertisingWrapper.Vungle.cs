using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// Wraps Vungle funcionality
/// </summary>
/// <remarks>
/// https://support.vungle.com/hc/en-us/articles/115000468731-Get-Started-with-Vungle-SDK-v-5-1-and-higher-Unity
/// https://github.com/unity-plugins/Unity-Admob/wiki/How-to-Use-Admob-Plugin-for-Unity
/// </remarks>
public static partial class AdvertisingWrapper
{
    #region Private Fields

    static Action<string, AdFinishedEventArgs> _onAdFinishedCallback;
    static Action<string> _onAdStartedCallback;
    static Action _onVungleInitialized;
    static bool _isVungleInitialized;

    #endregion Private Fields

    #region Public Events

    /// <summary>
    /// Occurs when the ads playability changes.
    /// </summary>
    public static event Action<string, bool> OnAdPlayableChanged;
    public static event Action<string, AdFinishedEventArgs> OnAdFinished;
    public static event Action<string> OnAdStarted;

    #endregion Public Events

    #region Public Methods

    /// <summary>
    /// Gets the placement.
    /// </summary>
    /// <param name="placemetId">The placemet identifier.</param>
    public static string GetPlacement(VunglePlacementsIds placemetId)
    {
        return VungleConfigurations.PLACEMENTS_DICT[(int)placemetId];
    }

    public static bool IsVungleInitialized
    {
        get
        {
            return _isVungleInitialized;
        }
    }

    /// <summary>
    /// Determines whether the specified placement is playable.
    /// </summary>
    /// <param name="placemetId">The placemet identifier.</param>
    /// <returns>
    /// <c>true</c> if the specified placement is playable; otherwise, <c>false</c>.
    /// </returns>
    static bool IsAdPlayable(VunglePlacementsIds placemetId)
    {
        var isAdPlayable = Vungle.isAdvertAvailable(GetPlacement(placemetId));
        OnLogEvent(string.Format("Is ad {0} playable: {1}", placemetId.ToString(), isAdPlayable));
        return Application.isEditor || isAdPlayable;
    }

    /// <summary>
    /// Plays the ad.
    /// </summary>
    /// <param name="onAdFinishedCallback">The callback that will be called when the ad finishes playing.</param>
    /// <param name="onAdStartedCallback">The callback that will be called when the ad starts playing.</param>
    /// <param name="placementId">The placemet identifier.</param>
    public static void PlayAd(Action<string, AdFinishedEventArgs> onAdFinishedCallback, Action<string> onAdStartedCallback,
        VunglePlacementsIds placementId)
    {
        if (Application.isEditor)
        {
            ShowVungleMockModalDialog(onAdFinishedCallback, placementId);
        }
        else if (IsAdPlayable(placementId))
        {
            _onAdFinishedCallback = onAdFinishedCallback;
            _onAdStartedCallback = onAdStartedCallback;
            Vungle.playAd(GetPlacement(placementId));
        }
        else
        {
            if (onAdFinishedCallback != null)
            {
                onAdFinishedCallback.Invoke(GetPlacement(placementId), new AdFinishedEventArgs { IsCompletedView = true }); 
            }
        }
    }

    /// <summary>
    /// Loads the ad.
    /// </summary>
    /// <param name="placementId">The placement identifier.</param>
    public static void LoadAd(VunglePlacementsIds placementId)
    {
        Vungle.loadAd(GetPlacement(placementId));
    }

    #endregion Public Methods

    #region Private Methods

    static void OnAdFinishedEvent(string placementid, AdFinishedEventArgs obj)
    {
        OnLogEvent(string.Format("Ad finished completed: {0}", obj.IsCompletedView));
        PauseManager.Instance.OnResume();
        if (_onAdFinishedCallback != null)
        {
            _onAdFinishedCallback.Invoke(placementid, obj); 
        }
        _onAdFinishedCallback = null;
        if (OnAdFinished != null)
        {
            OnAdFinished.Invoke(placementid, obj); 
        }
    }

    static void OnAdPlayableEvent(string placement, bool isPlayable)
    {
        OnLogEvent(string.Format("Is ad {0} playable: {1}", placement, isPlayable));
        if (OnAdPlayableChanged != null)
        {
            OnAdPlayableChanged.Invoke(placement, isPlayable); 
        }
    }

    static void OnAdStartedEvent(string placementId)
    {
        OnLogEvent("Ad started");
        PauseManager.Instance.OnPause();
        if (_onAdStartedCallback != null)
        {
            _onAdStartedCallback.Invoke(placementId); 
        }
        _onAdStartedCallback = null;

        if (OnAdStarted != null)
        {
            OnAdStarted.Invoke(placementId); 
        }
    }

    static void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            Vungle.onPause();
        }
        else
        {
            Vungle.onResume();
        }
    }

    static void OnInitializeEvent()
    {
        LogManager.Log("VunglInitialized");
        _isVungleInitialized = true;
        if (_onVungleInitialized != null)
        {
            _onVungleInitialized.Invoke(); 
        }
        _onVungleInitialized = null;
    }

    static void OnLogEvent(string message)
    {
        LogManager.Log(string.Format("VungleWrapper Log: {0}", message));
    }

    static void ShowVungleMockModalDialog(Action<string, AdFinishedEventArgs> onAdFinishedCallback, VunglePlacementsIds placementId)
    {
        MockUiCreator.CreateUI(
            () => onAdFinishedCallback.Invoke(placementId.ToString(), new AdFinishedEventArgs { IsCompletedView = true }),
            () => onAdFinishedCallback.Invoke(placementId.ToString(), new AdFinishedEventArgs { IsCompletedView = false })
        );
    }

    #endregion Private Methods
}

static class VungleConfigurations
{
    internal static readonly string APP_ID;
    internal static readonly string[] PLACEMENTS;
    internal static readonly Dictionary<int, string> PLACEMENTS_DICT = new Dictionary<int, string>();

    static VungleConfigurations()
    {
#if UNITY_IPHONE
            APP_ID = "fghfghfgh";
            PLACEMENTS_DICT.Add((int)VunglePlacementsIds.DEFAULT, "DEFAULT95694");
#elif UNITY_ANDROID
        APP_ID = "fhghfghfgh";
        PLACEMENTS_DICT.Add((int)VunglePlacementsIds.DEFAULT, "DEFAULT04628");
#elif UNITY_WSA_10_0 || UNITY_WINRT_8_1 || UNITY_METRO
        APP_ID = "fghfghfgh";
        PLACEMENTS_DICT.Add((int)VunglePlacementsIds.DEFAULT, "DEFAULT27951");
#endif
        PLACEMENTS = PLACEMENTS_DICT.Values.Select(v => v).ToArray();
    }
}

static class MockUiCreator
{
    const int LayerUI = 5;

    public static void CreateUI(UnityAction completeViewAction, UnityAction partialViewAction)
    {
        var canvas = CreateCanvas();

        CreateEventSystem(canvas);

        var panel = CreatePanel(canvas.transform);
        var panerWidth = panel.GetComponent<RectTransform>().rect.width;
        CreateText(panel.transform, 0, 300, panerWidth, 400, "VUGLE MOCK\nDo you want to send a complete view awnser\nor a partial view awnser?", 38);

        CreateButton(panel.transform, -150, -100, 200, 50, "Complete View", () => completeViewAction.Invoke(), canvas);
        CreateButton(panel.transform, 150, -100, 200, 50, "Partial View", () => partialViewAction.Invoke(), canvas);
    }

    static GameObject CreateCanvas()
    {
        // create the canvas
        var canvasObject = new GameObject("VungleMock - Canvas")
        {
            layer = LayerUI
        };

        canvasObject.AddComponent<RectTransform>();

        var canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.pixelPerfect = true;

        var canvasScal = canvasObject.AddComponent<CanvasScaler>();
        canvasScal.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScal.referenceResolution = new Vector2(800, 600);

        canvasObject.AddComponent<GraphicRaycaster>();

        return canvasObject;
    }

    static GameObject CreateEventSystem(GameObject parent)
    {
        var eventSystem = UnityEngine.Object.FindObjectOfType<EventSystem>();

        var esObject = new GameObject("VungleMock - EventSystem");

        if (eventSystem == null)
        {
            var esClass = esObject.AddComponent<EventSystem>();
            esClass.sendNavigationEvents = true;
            esClass.pixelDragThreshold = 5;
            var stdInput = esObject.AddComponent<StandaloneInputModule>();
            stdInput.horizontalAxis = "Horizontal";
            stdInput.verticalAxis = "Vertical";
        }

        esObject.transform.SetParent(parent.transform);

        return esObject;
    }

    static GameObject CreatePanel(Transform parent)
    {
        var panelObject = new GameObject("VungleMock - Panel");
        panelObject.transform.SetParent(parent);

        panelObject.layer = LayerUI;

        var trans = panelObject.AddComponent<RectTransform>();
        trans.anchorMin = new Vector2(0, 0);
        trans.anchorMax = new Vector2(1, 1);
        trans.anchoredPosition3D = new Vector3(0, 0, 0);
        trans.anchoredPosition = new Vector2(0, 0);
        trans.offsetMin = new Vector2(0, 0);
        trans.offsetMax = new Vector2(0, 0);
        trans.localPosition = new Vector3(0, 0, 0);
        trans.sizeDelta = new Vector2(0, 0);
        trans.localScale = new Vector3(0.8f, 0.8f, 1.0f);

        panelObject.AddComponent<CanvasRenderer>();

        var image = panelObject.AddComponent<Image>();
        image.color = Color.gray;

        return panelObject;
    }

    static GameObject CreateText(Transform parent, float x, float y, float w, float h, string message, int fontSize)
    {
        var textObject = new GameObject("VungleMock - Text");
        textObject.transform.SetParent(parent);

        textObject.layer = LayerUI;

        var trans = textObject.AddComponent<RectTransform>();
        trans.sizeDelta = new Vector2(w, h);
        trans.anchoredPosition3D = new Vector3(0, 0, 0);
        trans.anchoredPosition = new Vector2(x, y);
        trans.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        trans.localPosition.Set(0, 0, 0);

        textObject.AddComponent<CanvasRenderer>();

        var text = textObject.AddComponent<Text>();
        text.supportRichText = true;
        text.text = message;
        text.fontSize = fontSize;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        text.color = Color.black;

        return textObject;
    }

    static GameObject CreateButton(Transform parent, float x, float y, float w, float h, string message,
                                        UnityAction eventListner, GameObject root)
    {
        var buttonObject = new GameObject("Button");
        buttonObject.transform.SetParent(parent);

        buttonObject.layer = LayerUI;

        var trans = buttonObject.AddComponent<RectTransform>();
        SetSize(trans, new Vector2(w, h));
        trans.anchoredPosition3D = new Vector3(0, 0, 0);
        trans.anchoredPosition = new Vector2(x, y);
        trans.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        trans.localPosition.Set(0, 0, 0);

        buttonObject.AddComponent<CanvasRenderer>();

        var image = buttonObject.AddComponent<Image>();

        image.color = Color.blue;

        var button = buttonObject.AddComponent<Button>();
        button.interactable = true;
        button.onClick.AddListener(eventListner);
        button.onClick.AddListener(() => UnityEngine.Object.Destroy(root));

        CreateText(buttonObject.transform, 0, 0, w, h, message, 28);

        return buttonObject;
    }

    static void SetSize(RectTransform trans, Vector2 size)
    {
        var currSize = trans.rect.size;
        var sizeDiff = size - currSize;
        trans.offsetMin = trans.offsetMin -
                                  new Vector2(sizeDiff.x * trans.pivot.x,
                                      sizeDiff.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax + new Vector2(sizeDiff.x * (1.0f - trans.pivot.x),
                                      sizeDiff.y * (1.0f - trans.pivot.y));
    }
}