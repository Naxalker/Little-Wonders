using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GridProperties gridProperties;
    [SerializeField] float camSpeed;

    private Camera cam;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private void Start()
    {
        cam = GetComponent<Camera>();

        var vertExtent = cam.orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;

        minX = horzExtent - gridProperties.size.x / 2;
        maxX = gridProperties.size.x / 2 - horzExtent;
        minY = vertExtent - gridProperties.size.y / 2;
        maxY = gridProperties.size.x / 2 - vertExtent;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = Vector3.zero;
            return;
        }

        Vector3 pos = transform.position;

        if (Input.mousePosition.x >= Screen.width - 10f)
            pos.x += camSpeed * Time.deltaTime;
        if (Input.mousePosition.x <= 10f)
            pos.x -= camSpeed * Time.deltaTime;
        if (Input.mousePosition.y >= Screen.height - 10f)
            pos.y += camSpeed * Time.deltaTime;
        if (Input.mousePosition.y <= 10f)
            pos.y -= camSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}
