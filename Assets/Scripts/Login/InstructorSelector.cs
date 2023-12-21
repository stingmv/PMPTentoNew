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
    [SerializeField] private UnityEvent _onStart;
    [SerializeField] private PathToInstantiateInstructor _pathToInstantiateInstructor;
    [SerializeField] private ButtonAnimation _buttonNext;
    [SerializeField] private ButtonAnimation _buttonPrevious;
    [SerializeField] private Transform _container;
    [SerializeField] private GameObject _instructorPlatform;

    private int index = 0;
    private List<GameObject> _instructors = new List<GameObject>();
    public void InstantiateInstructor()
    {
        for (int i = 0; i < _objectInstructor.instructors.Length; i++)
        {
            (Vector3, Vector3) posRot = _pathToInstantiateInstructor.GetPositionToInstantiate(i);
            var instant = Instantiate(_objectInstructor.instructors[i].prefab, posRot.Item1, quaternion.identity, _container);
            instant.layer = 6;
            for (int j = 0; j < instant.transform.childCount; j++)
            {
                instant.transform.GetChild(j).gameObject.layer = 6;
            }
            instant.transform.forward = posRot.Item2;
            Instantiate(_instructorPlatform, posRot.Item1, Quaternion.identity).transform.parent = instant.transform;
            

            _instructors.Add(instant);
        }
    }

    private void Start()
    {
        _onStart?.Invoke();
    }

    public void InitValues()
    {
        InstantiateInstructor();
        ComprobeNext();
        ComprobePrevious();
    }

    public void ClearListInsructor()
    {
        for (int j = 0; j < _instructors.Count; j++)
        {
            Destroy(_instructors[j]);
        }
        _instructors.Clear();
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
