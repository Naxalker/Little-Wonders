using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public string descriptionText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        DescriptionCanvas.Instance.ShowCanvas(descriptionText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DescriptionCanvas.Instance.HideCanvas();
    }

    public void SetButton(Sprite _sprite, bool _isActive, string _descriptionText)
    {
        gameObject.SetActive(_isActive);
        GetComponent<Image>().sprite = _sprite;
        descriptionText = _descriptionText;
    }
}
