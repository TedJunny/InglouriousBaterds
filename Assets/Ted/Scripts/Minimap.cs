using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] Transform player;
    float followSmoothSpeed = 3f;

    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * followSmoothSpeed);
    }
}
