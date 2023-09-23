using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private void Update()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(worldPosition.x, worldPosition.y - 1f, transform.position.z);
    }
}
