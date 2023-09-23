using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GridProperties gridProperties;
    [SerializeField] float camSpeed;

    public float minSize, maxSize;

    private Camera cam;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float vertExtent;
    private float horzExtent;


    private void Start()
    {
        cam = GetComponent<Camera>();

        transform.position = new Vector3(gridProperties.size.x / 2, gridProperties.size.y / 2, -10f);

        ResetBounce();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // transform.position = Vector3.zero;
            return;
        }

        HandleMouseZoom();      // именно в этом порядке, чтобы не выходило
        HandleMouseMovement();  // за границы экрана grid'а
    }

    private void HandleMouseMovement() 
    {
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

    private void HandleMouseZoom() 
    {
        float zoom = cam.orthographicSize;
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && vertExtent < maxSize) {
            zoom++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && vertExtent > minSize) {
            zoom--;
        }

        ResetBounce();

        cam.orthographicSize = zoom;
    }

    private void ResetBounce()
    {
        vertExtent = cam.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;

        minX = horzExtent;
        maxX = gridProperties.size.x - horzExtent;
        minY = vertExtent;
        maxY = gridProperties.size.y - vertExtent;
    }
}
