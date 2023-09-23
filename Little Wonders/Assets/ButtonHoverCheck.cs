using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverCheck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string descriptionText;
    private Button button;
    [SerializeField] private GameObject descriptionCanvas;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionCanvas.SetActive(true);
        descriptionCanvas.GetComponentInChildren<TextMeshProUGUI>().text = descriptionText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionCanvas.SetActive(false);
    }
}
