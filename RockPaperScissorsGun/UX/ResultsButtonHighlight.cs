using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class ResultsButtonHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isHighlighted;

    public Color turnColor;
    public Color normalColor;

    public Vector3 initialPos;

    void Start()
    {
        isHighlighted = false;
        initialPos = this.gameObject.transform.localPosition;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        this.gameObject.transform.localPosition = initialPos + Random.insideUnitSphere * 5f;
        this.gameObject.GetComponent<Image>().color = turnColor;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        this.gameObject.transform.localPosition = initialPos;
        this.gameObject.GetComponent<Image>().color = normalColor;
    }
}
