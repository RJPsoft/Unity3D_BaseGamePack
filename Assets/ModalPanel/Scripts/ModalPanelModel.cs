using UnityEngine;

public class ModalPanelModel
{
    readonly ModalButtonModel button1Model;
    readonly string modalText;

    public ModalPanelModel(string modalText, ModalButtonModel button1Model)
    {
        this.modalText = modalText;
        this.button1Model = button1Model;
    }

    public ModalButtonModel Button1Model { get { return button1Model; } }

    public ModalButtonModel Button2Model { get; set; }

    public ModalButtonModel Button3Model { get; set; }

    public Sprite IconImage { get; set; }

    public string ModalText { get { return modalText; } }
}
