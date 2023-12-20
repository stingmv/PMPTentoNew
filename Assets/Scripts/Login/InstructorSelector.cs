using System;
using System.Collections;
using System.Collections.Generic;
using Button;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class InstructorSelector : MonoBehaviour
{
    [SerializeField] private ScriptableObjectInstructor _objectInstructor;
    [SerializeField] private ScriptableObjectUser _objectUser;
    [SerializeField] private UnityEvent _onSelectInstructor;
    [SerializeField] private PathToInstantiateInstructor _pathToInstantiateInstructor;
    [SerializeField] private ButtonAnimation _buttonNext;
    [SerializeField] private ButtonAnimation _buttonPrevious;
    [SerializeField] private Transform _container;

    private int index = 0;
    
    public void InstantiateInstructor()
    {
        for (int i = 0; i < _objectInstructor.instructors.Length; i++)
        {
            Instantiate(_objectInstructor.instructors[i].prefab, _pathToInstantiateInstructor.GetPositionToInstantiate(i), quaternion.identity, _container);
        }
    }

    private void Start()
    {
        InstantiateInstructor();
        ComprobeNext();
        ComprobePrevious();
    }

    public void GetNextInstructor()
    {
        index++;
        ComprobeNext();
        ComprobePrevious();
        _pathToInstantiateInstructor.RotateToItem(index);
    }

    public void ComprobeNext()
    {
        if ((index+ 1) >= _objectInstructor.instructors.Length)
        {
            _buttonNext.gameObject.SetActive(false);
        }
        else
        {
            _buttonNext.gameObject.SetActive(true);
        }
    }
    public void ComprobePrevious()
    {
        if ((index - 1) < 0)
        {
            _buttonPrevious.gameObject.SetActive(false);
        }
        else
        {
            _buttonPrevious.gameObject.SetActive(true);
        }
    }
    public void GetPreviousInstructor()
    {
        index--;
        ComprobeNext();
        ComprobePrevious();
        _pathToInstantiateInstructor.RotateToItem(index);
    }
    public void SelectInstructor()
    {
        _buttonNext.DisableButton();
        _buttonPrevious.DisableButton();
        _objectUser.userInfo.haveInstructor = true;
        _objectUser.userInfo.idInstructor = index;
        PlayerPrefs.SetString("userInfo", JsonUtility.ToJson(_objectUser.userInfo));
        _onSelectInstructor?.Invoke();
    }
}
