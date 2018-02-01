using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModalPanel : MonoBehaviour
{
    #region Public Properties

    /// <summary>
    /// Gets a value indicating whether the modal is showing.
    /// </summary>
    /// <value>
    /// <c>true</c> if the modal is showing; otherwise, <c>false</c>.
    /// </value>
    public bool IsShowing { get { return modalPanelObject.activeSelf; } }

    #endregion Public Properties

    #region Fields

    static ModalPanel modalPanel;

    UnityAction[] _button1Actions = new UnityAction[2];
    UnityAction[] _button2Actions = new UnityAction[2];
    UnityAction[] _button3Actions = new UnityAction[2];

    [SerializeField]
    Button button1;
    [SerializeField]
    Image button1Icon;
    [SerializeField]
    Text button1Text;
    [SerializeField]
    Button button2;
    [SerializeField]
    Image button2Icon;
    [SerializeField]
    Text button2Text;
    [SerializeField]
    Button button3;
    [SerializeField]
    Image button3Icon;
    [SerializeField]
    Text button3Text;
    [SerializeField]
    Image iconImage;
    [SerializeField]
    GameObject modalPanelObject;
    [SerializeField]
    Text modalText;

    #endregion Fields

    #region Public Methods

    /// <summary>
    /// Get this instance.
    /// </summary>
    /// <returns></returns>
    public static ModalPanel Instance()
    {
        if (!modalPanel)
        {
            var modalPanels = FindObjectsOfType<ModalPanel>();
            if (modalPanels.Length == 0)
            {
                Debug.LogError("There needs to be one active ModalPanel script on a GameObject in your scene.");
            }
            else if (modalPanels.Length > 1)
            {
                Debug.LogError("There needs to be only one active ModalPanel script on a GameObject in your scene.");
            }
            else
            {
                modalPanel = modalPanels[0];
            }
        }

        return modalPanel;
    }

    /// <summary>
    /// Shows the Modal.
    /// </summary>
    /// <param name="details">The details to show.</param>
    /// <exception cref="InvalidOperationException">At least one ButtonModel is necessary</exception>
    public void Show(ModalPanelModel details)
    {
        modalPanelObject.SetActive(true);
        DeactivateElements();
        RemoveAllListeners();
        modalText.text = details.ModalText;
        ConfigureIcon(details);

        if (details.Button1Model == null && details.Button2Model == null && details.Button3Model == null)
        {
            throw new InvalidOperationException("At least one ButtonModel is necessary");
        }

        ConfigureButton(button1, button1Text, button1Icon, details.Button1Model, Button1Listener, _button1Actions);
        ConfigureButton(button2, button2Text, button2Icon, details.Button2Model, Button2Listener, _button2Actions);
        ConfigureButton(button3, button3Text, button3Icon, details.Button3Model, Button3Listener, _button3Actions);
    }

    #endregion Public Methods

    #region Methods

    static void SetButtonText(Text buttonText, ModalButtonModel buttonDetails)
    {
        if (!string.IsNullOrEmpty(buttonDetails.Text))
        {
            buttonText.gameObject.SetActive(true);
            buttonText.text = buttonDetails.Text;
        }
        else
        {
            buttonText.gameObject.SetActive(false);
        }
    }

    void Button1Listener()
    {
        _button1Actions[0].Invoke();
        StartCoroutine(ExecuteAction(_button1Actions[1]));
    }

    void Button2Listener()
    {
        _button2Actions[0].Invoke();
        StartCoroutine(ExecuteAction(_button2Actions[1]));
    }

    void Button3Listener()
    {
        _button3Actions[0].Invoke();
        StartCoroutine(ExecuteAction(_button3Actions[1]));
    }

    void ClosePanel()
    {
        modalPanelObject.SetActive(false);
    }

    void ConfigureButton(Button button, Text buttonText, Image buttonImage, ModalButtonModel buttonDetails, UnityAction listener, UnityAction[] actions)
    {
        if (buttonDetails != null)
        {
            button.gameObject.SetActive(true);
            button.onClick.AddListener(listener);
            actions[0] = ClosePanel;
            actions[1] = buttonDetails.Action ?? (() => { });
            SetButtonText(buttonText, buttonDetails);
            SetButtonIcon(buttonImage, buttonDetails);
        }
    }

    void ConfigureIcon(ModalPanelModel details)
    {
        if (details.IconImage)
        {
            iconImage.sprite = details.IconImage;
            iconImage.gameObject.SetActive(true);
        }
    }

    void DeactivateElements()
    {
        iconImage.gameObject.SetActive(false);
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
    }

    IEnumerator ExecuteAction(UnityAction unityAction)
    {
        yield return null;
        unityAction.Invoke();
    }

    void OnEnable()
    {
        gameObject.transform.SetAsFirstSibling();
    }

    void RemoveAllListeners()
    {
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();
    }

    void SetButtonIcon(Image buttonIcon, ModalButtonModel buttonDetails)
    {
        if (buttonDetails.Icon != null)
        {
            buttonIcon.gameObject.SetActive(true);
            buttonIcon.sprite = buttonDetails.Icon;
        }
        else
        {
            buttonIcon.gameObject.SetActive(false);
        }
    }

    #endregion Methods
}