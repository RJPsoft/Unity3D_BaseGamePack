using UnityEngine;

public class ModalPanelModel
{
    readonly string modalText;

    public ModalPanelModel(string modalText)
    {
        this.modalText = modalText;
    }

    public ModalButtonModel PositiveButtonModel { get; set; }

    public ModalButtonModel NeutralButtonModel { get; set; }

    public ModalButtonModel NegativeButtonModel { get; set; }

    public Sprite IconImage { get; set; }

    public Color IconColor { get; set; }

    public string ModalText
    {
        get
        {
            return modalText;
        }
    }
}
