using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsGroup : MonoBehaviour
{
    [SerializeField] private GlosaryChallengeController _glosaryChallengeController;
    private List<OptionGC> _options;

    private OptionGC _oldSelectedButton;

    public OptionGC OldSelectedButton
    {
        get => _oldSelectedButton;
        set
        {
            _oldSelectedButton = value; 
            _glosaryChallengeController.Evaluate();
        }
    }
    public void SelectButton(OptionGC buttonSelected)
    {
        if (buttonSelected.Equals(OldSelectedButton))
        {
            buttonSelected.DeselectButton();
            OldSelectedButton = null;
            return;
        }
        if (OldSelectedButton)
        {
            OldSelectedButton.DeselectButton();
        }
        buttonSelected.SelectButton();
        OldSelectedButton = buttonSelected;
    }

}
