using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Number : MonoBehaviour
{
    [SerializeField] float duration;

    private float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - startTime >= duration) Destroy(gameObject);

        transform.position += new Vector3(0f, 10f * Time.deltaTime, 0f);

        Color newColor = GetComponent<TextMeshProUGUI>().color;
        newColor.a = 1f - (Time.time - startTime) / duration;
        GetComponent<TextMeshProUGUI>().color = newColor;
    }
}
