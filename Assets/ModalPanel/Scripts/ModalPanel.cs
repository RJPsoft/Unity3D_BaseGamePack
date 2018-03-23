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
    public bool IsShowing { get { return  modalPanelObject.activeSelf; } }

    #endregion Public Properties

    #region Fields

    static ModalPanel modalPanel;

    UnityAction[] _positiveButtonActions = new UnityAction[2];
    UnityAction[] _neutralButtonActions = new UnityAction[2];
    UnityAction[] _negativeButtonActions = new UnityAction[2];

    [SerializeField]
    Button positiveButton;
    [SerializeField]
    Image positiveButtinIcon;
    [SerializeField]
    Text positiveButtonText;
    [SerializeField]
    Button neutralButton;
    [SerializeField]
    Image neutralButtonIcon;
    [SerializeField]
    Text neutralButtonText;
    [SerializeField]
    Button negativeButton;
    [SerializeField]
    Image negativeButtonIcon;
    [SerializeField]
    Text negativeButtonText;
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
                throw new InvalidOperationException("There needs to be one active ModalPanel script on a GameObject in your scene.");
            }
            else if (modalPanels.Length > 1)
            {
                throw new InvalidOperationException("There needs to be only one active ModalPanel script on a GameObject in your scene.");
            }
            else
            {
                modalPanel = modalPanels[0];
            }
        }

        return modalPanel;
    }

    void Update()
    {
        InputController.HitEscapeAndIsActive(modalPanelObject, this, ExecuteCloseAction);
    }

    void ExecuteCloseAction()
    {
        if (neutralButton.IsActive())
        {
            _neutralButtonActions[0].Invoke();
            StartCoroutine(ExecuteAction(_neutralButtonActions[1]));
        }
        else if (negativeButton.IsActive())
        {
            _negativeButtonActions[0].Invoke();
            StartCoroutine(ExecuteAction(_negativeButtonActions[1]));
        }
        else
        {
            _positiveButtonActions[0].Invoke();
            StartCoroutine(ExecuteAction(_positiveButtonActions[1]));
        }
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

        if (details.PositiveButtonModel == null && details.NeutralButtonModel == null && details.NegativeButtonModel == null)
        {
            throw new InvalidOperationException("At least one ButtonModel is necessary");
        }

        ConfigureButton(positiveButton, positiveButtonText, positiveButtinIcon, details.PositiveButtonModel, Button1Listener, _positiveButtonActions);
        ConfigureButton(neutralButton, neutralButtonText, neutralButtonIcon, details.NeutralButtonModel, Button2Listener, _neutralButtonActions);
        ConfigureButton(negativeButton, negativeButtonText, negativeButtonIcon, details.NegativeButtonModel, Button3Listener, _negativeButtonActions);
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
        _positiveButtonActions[0].Invoke();
        StartCoroutine(ExecuteAction(_positiveButtonActions[1]));
    }

    void Button2Listener()
    {
        _neutralButtonActions[0].Invoke();
        StartCoroutine(ExecuteAction(_neutralButtonActions[1]));
    }

    void Button3Listener()
    {
        _negativeButtonActions[0].Invoke();
        StartCoroutine(ExecuteAction(_negativeButtonActions[1]));
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
        positiveButton.gameObject.SetActive(false);
        neutralButton.gameObject.SetActive(false);
        negativeButton.gameObject.SetActive(false);
    }

#pragma warning disable CC0091 // Use static method
    IEnumerator ExecuteAction(UnityAction unityAction)
#pragma warning restore CC0091 // Use static method
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
        positiveButton.onClick.RemoveAllListeners();
        neutralButton.onClick.RemoveAllListeners();
        negativeButton.onClick.RemoveAllListeners();
    }

    static void SetButtonIcon(Image buttonIcon, ModalButtonModel buttonDetails)
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