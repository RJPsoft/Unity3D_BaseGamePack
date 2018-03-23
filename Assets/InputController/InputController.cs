using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Holds touch and mouse specific methods
/// </summary>
public static class InputController
{
    #region Private Fields

    static readonly object LOCK = new object();

    #endregion Private Fields

    #region Public Properties

    /// <summary>
    /// Gets a value indicating whether the mouse was clicked.
    /// </summary>
    /// <value>
    ///   <c>true</c> if mouse was clicked; otherwise, <c>false</c>.
    /// </value>
    public static bool IsMouseClick
    {
        get
        {
            return Input.GetMouseButtonDown(0);
        }
    }

    /// <summary>
    /// Gets a value indicating whether the screen was touched or the mouse was clicked.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the screen was touched or the mouse was clicked; otherwise, <c>false</c>.
    /// </value>
    public static bool IsScreenOrMouseTouch
    {
        get
        {
            return IsScreenTouched || IsMouseClick;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the screen was touched.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the screen was touched; otherwise, <c>false</c>.
    /// </value>
    public static bool IsScreenTouched
    {
        get
        {
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
        }
    }

    /// <summary>
    /// Gets the touch position.
    /// </summary>
    public static Vector3 TouchPosition
    {
        get
        {
            return new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 1);
        }
    }

    /// <summary>
    /// Gets the mouse position.
    /// </summary>
    public static Vector3 MousePosition
    {
        get
        {
            return Input.mousePosition;
        }
    }

    /// <summary>
    /// Gets the input point. It can be either touch position or mouse position
    /// </summary>
    public static Vector3 InputPoint
    {
        get
        {
            return IsScreenTouched ? TouchPosition : MousePosition;
        }
    }

    /// <summary>
    /// Gets the touch ray.
    /// </summary>
    public static Ray TouchRay
    {
        get
        {
            return Camera.main.ScreenPointToRay(InputPoint);
        }
    }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Executes the <paramref name="actionToExecute"/> if the "escape" button was hitted and if de <paramref name="gameObject"/> is active
    /// </summary>
    /// <param name="gameObject">The game object to test is is active.</param>
    /// <param name="monoBehaviour">a mono behaviour instance.</param>
    /// <param name="actionToExecute">The action to execute.</param>
    public static void HitEscapeAndIsActive(GameObject gameObject, MonoBehaviour monoBehaviour, Action actionToExecute)
    {
        lock (LOCK)
        {
            if (gameObject.transform.IsLastSibling() && Input.GetKeyUp(KeyCode.Escape))
            {
                monoBehaviour.StartCoroutine(InvokeAction(actionToExecute));
            }
        }
    }

    /// <summary>
    /// Determines whether the specifyed key was pressed.
    /// </summary>
    /// <param name="keyCode">The key code to verify.</param>
    /// <returns>
    ///   <c>true</c> if the specifyed key was pressed; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsKeyDown(KeyCode keyCode)
    {
        return Input.GetKeyDown(keyCode);
    }

    #endregion Public Methods

    #region Private Methods

    static IEnumerator InvokeAction(Action executeIfTrue)
    {
        yield return new WaitForEndOfFrame();
        if (executeIfTrue != null)
        {
            executeIfTrue.Invoke();
        }
    }

    #endregion Private Methods
}