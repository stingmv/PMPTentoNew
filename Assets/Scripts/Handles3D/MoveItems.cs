using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Handles3D
{
    public class MoveItems : MonoBehaviour
    {
        #region Variables

        [SerializeField] private RectTransform _itemsContainer;
        [SerializeField] private TextMeshProUGUI _domainText;

        [SerializeField] private float _timeVelocity;
        [SerializeField] private float _timeToTransition;
        private float _currentTime;
        [SerializeField]private List<HandlesItem> _options;
        [SerializeField] private RectTransform _offset;
        [SerializeField] private UnityEvent _onSelectedItem;
        [SerializeField] private HandlesItem _prefabItem;
        [SerializeField] private RectTransform _prefabOffset;

        private int count;
        private HandlesItem _elegido;

        public HandlesItem Elegido
        {
            get => _elegido;
            set => _elegido = value;
        }

        #endregion

        #region Unity Methods

        // Start is called before the first frame update
        private void Start()
        {
            List<String> tags = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                var s = $"------>{i}<------";
                tags.Add(s);
            }
            SetData(tags);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        #endregion

        #region Methods

        public void SetTextTittle(string tittle)
        {
            _domainText.text = tittle;
        }
        public void SetData(List<string> tags)
        {
            for (int i = 0; i < _options.Count; i++)
            {
                Destroy(_options[i].gameObject);
            }
            _options.Clear();
            HandlesItem item;

            for (int i = 0; i < tags.Count; i++)
            {
                item = Instantiate(_prefabItem, _itemsContainer);
                item.SetData(tags[i].Replace('_', ' ').Replace('\n', ' '), tags[i]);
                _options.Add(item);
            }

            _offset = Instantiate(_prefabOffset, _itemsContainer);

            var rect = _itemsContainer.rect;
            rect.yMin = 0;
            // Debug.Log(_options[0].Rect.rect.height);
            _itemsContainer.offsetMin = tags.Count %2 == 0? new Vector2(0,-_options[0].Rect.rect.height ): new Vector2(0, 0);

        }
        public void StartTransition()
        {
            count = _options.Count - 1;
            StartCoroutine(IStartCounter());
            StartCoroutine(ITransition());
        }

        IEnumerator IStartCounter()
        {
            var currentTime = 0f;
            var rand = Random.Range(5f, 10f);
            while (currentTime <= 2)
            {
                currentTime += Time.deltaTime/ _timeToTransition;
                _timeVelocity = Mathf.Lerp(.01f, 4, 1 / (1 + Mathf.Exp(-7 * currentTime + rand)));
                yield return null;

            }
            _timeVelocity = 4;
        }
        IEnumerator ITransition()
        {
            var i = count;
            while (true)
            {
                var currentItem = _options[i];
                _currentTime = 0f;
                var heithTemp = currentItem.Rect.sizeDelta.y;
                currentItem.Rect.sizeDelta = new Vector2(currentItem.Rect.sizeDelta.x, 0);
                currentItem.Rect.SetAsFirstSibling();
                var y = 0f;
                while (_currentTime<=1)
                {
                    _currentTime += Time.deltaTime / _timeVelocity;
                    y = (_currentTime) * heithTemp;
                    currentItem.Rect.sizeDelta = new Vector2(currentItem.Rect.sizeDelta.x, y);
                    _offset.sizeDelta = new Vector2(currentItem.Rect.sizeDelta.x, heithTemp - y);
                    yield return null;

                }
                y = heithTemp;
                currentItem.Rect.sizeDelta = new Vector2(currentItem.Rect.sizeDelta.x, y); 
                _offset.sizeDelta = new Vector2(currentItem.Rect.sizeDelta.x,heithTemp - y);
                if (_timeVelocity > 3.5f)
                {
                    _elegido = _itemsContainer.GetChild((_itemsContainer.childCount - 2) / 2).GetComponent<HandlesItem>();
                    Debug.Log("elegido => " +_elegido.ID);
                    _onSelectedItem?.Invoke();
                    yield break;
                }
                i--;
                if (i < 0)
                {
                    i = count;
                }
            }
        }

        
        #endregion

    }

}