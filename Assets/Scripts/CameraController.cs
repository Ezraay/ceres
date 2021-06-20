using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Camera))]
public class CameraController : MonoBehaviour {
    static Transform target;
    static new Camera camera;

    [SerializeField] float angle = 45;
    [SerializeField] float smoothTime = 4;
    [SerializeField] float minimumZoom = 10;
    [SerializeField] float maximumZoom = 20;
    float zoom;

    public static void SetTarget (Transform target) {
        CameraController.target = target;
    }

    void Start () {
        zoom = minimumZoom;
    }

    void Update () {
        float scrollDelta = Input.mouseScrollDelta.y;
        zoom -= scrollDelta;
        zoom = Mathf.Clamp (zoom, minimumZoom, maximumZoom);

        float angleRadians = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3 (0, zoom * Mathf.Sin(angleRadians), -zoom * Mathf.Cos(angleRadians));
        Vector3 targetPosition = target.position + offset;

        transform.position = Vector3.Lerp (transform.position, targetPosition, smoothTime * Time.deltaTime);
        transform.rotation = Quaternion.Euler (angle, 0, 0);
    }
}