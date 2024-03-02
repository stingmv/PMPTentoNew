using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _viewPortTransform;
    [SerializeField] private RectTransform _contentPanelTransform;
    [SerializeField] private VerticalLayoutGroup _vlg;
    [SerializeField] private RectTransform _canvasTransform;
    [Range(0, 5)]
    [SerializeField] private float _dragThreshold;
    [Range(0, 3)]
    [SerializeField] private float _duration;
    [SerializeField] private RectTransform _puntoPartida;
    [SerializeField] private RectTransform scrollViewItem;
    
    private RectTransform _currentPanel;
    private List<RectTransform> _listItems = new List<RectTransform>();
    private float _bottomLimit;
    private RectTransform _rCurrent;
    private Vector2 _deltaMouse;
    
    
    private int initCount;
    private bool inLerp = false;
    

    private Vector3 _ContentInitPosition;
    private void Start()
    {
        _rCurrent = _canvasTransform;
        RectTransform rt = null;
        for (int i = 0; i < 3; i++)
        {
            rt = Instantiate(scrollViewItem, _contentPanelTransform);
            rt.sizeDelta = new Vector2(_rCurrent.sizeDelta.x, _rCurrent.sizeDelta.y);
            _listItems.Add(rt);
        }

        initCount = 3;
        _bottomLimit = _listItems[0].rect.height * initCount;

        _currentPanel = _listItems[0];
    
    }
    
    private void Update()
    {
        if (!inLerp && Input.touchCount > 0)
        {
            
            if (_contentPanelTransform.localPosition.y > 0 )
            {
                Canvas.ForceUpdateCanvases();
                _contentPanelTransform.localPosition -=
                    new Vector3(0, initCount * (_listItems[0].rect.height + _vlg.spacing), 0);
            }
            //
            if (_contentPanelTransform.localPosition.y < 0 )
            {
                Canvas.ForceUpdateCanvases();
                _contentPanelTransform.localPosition +=
                    new Vector3(0, initCount * (_listItems[0].rect.height + _vlg.spacing), 0);
            }
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        inLerp = true;
                  _deltaMouse = Input.GetTouch(0).deltaPosition;

        if (Mathf.Approximately(_ContentInitPosition.y - _contentPanelTransform.localPosition.y,0))
        {
            return;
        }
        // Debug.Log(_deltaMouse.y);
        if (Mathf.Abs(_deltaMouse.y) > _dragThreshold)
        {
            var currentPanel = GetNearTransform(_currentPanel, _deltaMouse.y >= 0);
            StartCoroutine(FitPanel(currentPanel));
        }
        else
        {
            var currentPanel = GetNearTransform();
            StartCoroutine(FitPanel(currentPanel));
        }
    }

    private RectTransform GetNearTransform(RectTransform target = null, bool dragUp= false)
    {
        RectTransform currentPanel = null;
            float minDistance = Single.MaxValue;
            for (int i = 0; i < _listItems.Count; i++)
            {
                var distance = Vector3.Distance(_listItems[i].position, _puntoPartida.position);
                if (minDistance > distance /*&&( !target || currentPanel != target)*/)
                {
                    if (target)
                    {
                        if (_listItems[i] == target) continue;
                        minDistance = distance;
                        currentPanel = _listItems[i];

                    }
                    else
                    {
                        minDistance = distance;
                        currentPanel = _listItems[i];    
                    }
                    
                }
            }

        return currentPanel;
    }
    IEnumerator FitPanel(RectTransform target)
    {

        var currentTime = 0f;
        var nitPosition = _contentPanelTransform.position;
        var difference = -target.position + _puntoPartida.position;
        // Debug.Log(difference + " " +
        //      contentPanelTransform.TransformPoint(difference) + " " + contentPanelTransform.InverseTransformPoint(difference));
        while (currentTime <=1)
        {
            
            Debug.DrawLine(target.position, _puntoPartida.position, Color.red);
            currentTime += Time.deltaTime / _duration;
            var movementInY = Mathf.Lerp(nitPosition.y, (nitPosition + difference).y, currentTime);
            _contentPanelTransform.position =
                new Vector3(0,movementInY , 0);
            yield return null;
        }
        _contentPanelTransform.position =
            new Vector3(0,(nitPosition + difference).y , 0);
        _currentPanel = target;
        if (_currentPanel.transform.GetSiblingIndex() == 2)
        {
            _contentPanelTransform.transform.GetChild(0).SetAsLastSibling();
            Canvas.ForceUpdateCanvases();
            _contentPanelTransform.localPosition -=
                new Vector3(0, _listItems[0].rect.height + _vlg.spacing, 0);
        }
        if (_currentPanel.transform.GetSiblingIndex() == 0)
        {
            _contentPanelTransform.transform.GetChild(2).SetAsFirstSibling();
            Canvas.ForceUpdateCanvases();
            _contentPanelTransform.localPosition +=
                new Vector3(0, _listItems[0].rect.height + _vlg.spacing, 0);
        }
        inLerp = false;
    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < _listItems.Count; i++)
        {
            Gizmos.DrawLine(_listItems[i].position, _puntoPartida.position);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _ContentInitPosition = _contentPanelTransform.localPosition;
    }
}