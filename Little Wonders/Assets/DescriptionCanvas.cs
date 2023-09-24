using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DescriptionCanvas : MonoBehaviour
{
    public static DescriptionCanvas Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(worldPosition.x, worldPosition.y - 1f, transform.position.z);
    }

    public void ShowCanvas(string _text)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(worldPosition.x, worldPosition.y - 1f, transform.position.z);
        gameObject.SetActive(true);
        GetComponentInChildren<TextMeshProUGUI>().text = _text;
    }

    public void HideCanvas()
    {
        gameObject.SetActive(false);
    }
}
