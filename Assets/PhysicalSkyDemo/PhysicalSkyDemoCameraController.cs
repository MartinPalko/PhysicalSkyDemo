using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalSkyDemoCameraController : MonoBehaviour
{
    public Vector2 mouseSensitivity = new Vector2(1.0f, 1.0f);

    public float cameraHeight = 1.0f;
    public float cameraDistance = 5.0f;

    private Vector3 lastMousePosition;
    private Vector2 cameraRoations;

    private void Start()
    {
        lastMousePosition = Input.mousePosition;
    }

    void Update ()
    {
        // Mouse deltas
        Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
        lastMousePosition = Input.mousePosition;

        Vector2 moveDirection = mouseDelta;
        moveDirection.Scale(mouseSensitivity);

        if (Input.GetMouseButton(0))
        {
            // Camera Rotations
            cameraRoations += moveDirection;
            cameraRoations.y = Mathf.Clamp(cameraRoations.y, -30.0f, 80.0f);
        }

        // Position Camera
        transform.position = new Vector3(Mathf.Sin(cameraRoations.x * Mathf.Deg2Rad) * cameraDistance, cameraHeight, Mathf.Cos(cameraRoations.x * Mathf.Deg2Rad) * cameraDistance);
        transform.rotation = Quaternion.LookRotation(new Vector3(-transform.position.x, 0, -transform.position.z));
        transform.Rotate(Vector3.left, cameraRoations.y, Space.Self);
        

        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }
}
