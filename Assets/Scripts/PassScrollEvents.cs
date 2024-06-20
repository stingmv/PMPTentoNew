using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassScrollEvents : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerClickHandler
{
    private ScrollRect scrollRect;
    private bool isDragging=false;
    [SerializeField] UnityEvent onPointerClick;
   

    void Start()
    {              
        scrollRect = GetComponentInParent<ScrollRect>();// Obtiene el ScrollRect del padre más cercano
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
        {
            scrollRect.OnBeginDrag(eventData);//llama al metodo OnBeginDrag del ScrollRect padre, pasandole el evento
                                              //de arrastre, permite que ScrollRect comience a procesar el arrastre

            isDragging = true;//se esta arrastrando
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
        {
            scrollRect.OnDrag(eventData);//Llama a metodo OnDrag de ScrollRect padre,
                                         //permitiendo que continue procesando el arrastre
            isDragging = true;//se esta arrastrando
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
        {
            scrollRect.OnEndDrag(eventData);//llama a metodo OnEndDrag de ScrollRect padre,
                                            //finalizando el procesamiento del arrastre
            isDragging = false;//indicador de que se esta arrastrando en falso
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDragging)//si es true
        {
            return;//cancela OnPointerClick
        }
        Debug.Log("OnPointerClick");
        onPointerClick?.Invoke();//llama al evento onPointerClick y a todos sus metodos suscritos en el inspector
    }

}
