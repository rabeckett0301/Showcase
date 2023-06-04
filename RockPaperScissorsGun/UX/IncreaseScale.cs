using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class IncreaseScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 scale;

    public bool Highlighted;

    void Start()
    {
        scale = this.gameObject.transform.localScale;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        this.gameObject.transform.localScale = scale * 1.2f;
        this.gameObject.GetComponent<ShakeButton>().ShakeIntensity = 2.0f;
        AudioHandler.Instance.PlayAdd();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        this.gameObject.transform.localScale = scale;
        this.gameObject.GetComponent<ShakeButton>().ShakeIntensity = 0;
    }
}
