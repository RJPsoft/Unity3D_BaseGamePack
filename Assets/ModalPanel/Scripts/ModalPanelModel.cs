using UnityEngine;

public class ModalPanelModel
{
    readonly string modalText;

    public ModalPanelModel(string modalText)
    {
        this.modalText = modalText;
    }

    public ModalButtonModel Button1Model { get; set; }

    public ModalButtonModel Button2Model { get; set; }

    public ModalButtonModel Button3Model { get; set; }

    public Sprite IconImage { get; set; }

    public string ModalText { get { return modalText; } }
}
