using System;
using UnityEngine;

namespace Assets.Script.Managers
{
    public delegate void OnScreenSizeChange(Vector2 newScreenSize, float aspectRatioDelta, bool aspectRatioChanged);

    public class ScreenManager : SceneSingleton<ScreenManager>
    {
        #region Public Fields

        public OnScreenSizeChange OnScreenSizeChanged;

        #endregion Public Fields

        #region Private Fields

        const float POSITION_COMPARISON_DELTA = 0.0001f;
        Vector2 lastScreenSize;

        #endregion Private Fields

        #region Private Methods

        void Awake()
        {
            lastScreenSize = new Vector2(Screen.width, Screen.height);
        }

        void Update()
        {
            var screenSize = new Vector2(Screen.width, Screen.height);

            if (lastScreenSize != screenSize)
            {
                LogManager.Log("Screen size changed: " + screenSize);
                var aspectRatioDelta = (lastScreenSize.x / lastScreenSize.y) - (screenSize.x / screenSize.y);
                var aspectRatioChanged = Math.Abs(aspectRatioDelta) > POSITION_COMPARISON_DELTA;
                lastScreenSize = screenSize;
                if (OnScreenSizeChanged != null)
                {
                    OnScreenSizeChanged.Invoke(screenSize, aspectRatioDelta, aspectRatioChanged); 
                }
            }
        }

        #endregion Private Methods
    }
}