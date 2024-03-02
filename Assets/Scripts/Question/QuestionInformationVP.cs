using System.Collections;
using System.Collections.Generic;
using Question;
using TMPro;
using UnityEngine;

public class QuestionInformationVP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _question;
    [SerializeField] private OptionVP _opt1;
    [SerializeField] private OptionVP _opt2;
    [SerializeField] private OptionVP _opt3;
    [SerializeField] private OptionVP _opt4;

    public OptionVP Opt1
    {
        get => _opt1;
        set => _opt1 = value;
    }

    public OptionVP Opt2
    {
        get => _opt2;
        set => _opt2 = value;
    }

    public OptionVP Opt3
    {
        get => _opt3;
        set => _opt3 = value;
    }

    public OptionVP Opt4
    {
        get => _opt4;
        set => _opt4 = value;
    }
    public void SetData(QuestionData questionData)
    {
        _question.text = questionData.question;
        _opt1.SetData(questionData.options[0].respuesta,questionData.options[0].id.ToString());
        _opt2.SetData(questionData.options[1].respuesta,questionData.options[1].id.ToString());
        _opt3.SetData(questionData.options[2].respuesta,questionData.options[2].id.ToString());
        _opt4.SetData(questionData.options[3].respuesta,questionData.options[3].id.ToString());
        EnableOptions();
    }
    public void  EnableOptions()
    {
        _opt1.EnableOption();
        _opt2.EnableOption();
        _opt3.EnableOption();
        _opt4.EnableOption();
    }
    public void  DisableOptions()
    {
        _opt1.DisableOption();
        _opt2.DisableOption();
        _opt3.DisableOption();
        _opt4.DisableOption();
    }
}
