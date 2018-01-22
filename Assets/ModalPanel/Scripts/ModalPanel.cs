using System;
using UnityEngine;
using UnityEngine.UI;

public class ModalPanel : MonoBehaviour
{
    #region Private Fields

    static ModalPanel modalPanel;

    [SerializeField]
    Button button1;
    [SerializeField]
    Text button1Text;
    [SerializeField]
    Image button1Icon;
    [SerializeField]
    Button button2;
    [SerializeField]
    Image button2Icon;
    [SerializeField]
    Text button2Text;
    [SerializeField]
    Button button3;
    [SerializeField]
    Text button3Text;
    [SerializeField]
    Image button3Icon;
    [SerializeField]
    Image iconImage;
    [SerializeField]
    GameObject modalPanelObject;
    [SerializeField]
    Text modalText;

    #endregion Private Fields

    #region Public Methods

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
    /// <exception cref="ArgumentNullException">details - button1Details</exception>
    public void Show(ModalPanelModel details)
    {
        modalPanelObject.SetActive(true);
        DeactivateElements();
        RemoveAllListeners();
        modalText.text = details.ModalText;
        ConfigureIcon(details);

        if (details.Button1Model == null)
        {
            throw new ArgumentNullException("details", "button1Details");
        }

        ConfigureButton(button1, button1Text, button1Icon, details.Button1Model);
        ConfigureButton(button2, button2Text, button2Icon, details.Button2Model);
        ConfigureButton(button3, button3Text, button3Icon, details.Button3Model);
    }

    #endregion Public Methods

    #region Private Methods

    void ClosePanel()
    {
        modalPanelObject.SetActive(false);
    }

    void ConfigureButton(Button button, Text buttonText, Image buttonImage, ModalButtonModel buttonDetails)
    {
        if (buttonDetails != null)
        {
            button.gameObject.SetActive(true);
            button.onClick.AddListener(buttonDetails.Action ?? (() => { }));
            button.onClick.AddListener(ClosePanel);
            SetButtonText(buttonText, buttonDetails);
            SetButtonIcon(buttonImage, buttonDetails);
        }
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

    void RemoveAllListeners()
    {
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();
    }

    void OnEnable()
    {
        gameObject.transform.SetAsFirstSibling();
    }

    #endregion Private Methods
}
