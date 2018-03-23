using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    static ExampleScript exampleScript;

    [SerializeField]
    Sprite modalIcon;
    [SerializeField]
    Sprite button1Icon;
    [SerializeField]
    Sprite button2Icon;

    public static ExampleScript Instance()
    {
        if (!exampleScript)
        {
            var exampleScripts = FindObjectsOfType<ExampleScript>();
            if (exampleScripts.Length == 0)
            {
                Debug.LogError("There needs to be one active ExampleScript script on a GameObject in your scene.");
            }
            else if (exampleScripts.Length > 1)
            {
                Debug.LogError("There needs to be only one active ExampleScript script on a GameObject in your scene.");
            }
            else
            {
                exampleScript = exampleScripts[0];
            }
        }

        return exampleScript;
    }

    public void OnClick()
    {
        var button1model = new ModalButtonModel { Icon = button1Icon };
        var button2Model = new ModalButtonModel{ Text = "Log message", Action = Log };
        var button3Model = new ModalButtonModel{ Text = "Log custon message", Icon = button2Icon, Action = () => LogCustonMessage("Custom message") };
        var modalDetail = new ModalPanelModel("Had of her little ungodly who friend")
        {
            PositiveButtonModel = button1model,
            NeutralButtonModel = button2Model,
            NegativeButtonModel = button3Model,
            IconImage = modalIcon
        };

        ModalPanel.Instance().Show(modalDetail);
    }

    public void OnClickShowOneButton()
    {
        var button1model = new ModalButtonModel { Icon = button1Icon };
        var modalDetail = new ModalPanelModel("Had of her little ungodly who friend")
        {
            PositiveButtonModel = button1model,
            IconImage = modalIcon
        };

        ModalPanel.Instance().Show(modalDetail);
    }

    void Log()
    {
        Debug.Log("Losel feud yet bade");
    }

    void LogCustonMessage(string message)
    {
        Debug.Log(message);
    }
}
