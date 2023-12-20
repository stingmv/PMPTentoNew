using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurateNavbarScale : MonoBehaviour
{
    [SerializeField] private Image _imageNavbarContainer;

    [SerializeField] private RectTransform _mainCanvas;

    [SerializeField] private float _minPixelMultiplier;
    [SerializeField] private float _maxPixelMultiplier;
    [SerializeField] private float _minWidthScale;
    [SerializeField] private float _maxWidthScale;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        var y = _mainCanvas.rect.width;

        var x = (y - _minWidthScale) * (_maxPixelMultiplier - _minPixelMultiplier) / (_maxWidthScale - _minWidthScale) +
                _minPixelMultiplier;
        _imageNavbarContainer.pixelsPerUnitMultiplier = x;


































    }
}
