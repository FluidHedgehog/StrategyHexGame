using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] InputManager inputManager;
    [SerializeField] Camera cam;

    [SerializeField] float zoomSpeed;
    [SerializeField] float pushForce;
    float targetZoom;

    void Update()
    {
        inputManager.moveInput = inputManager.input.Player.Move.ReadValue<Vector2>();
        inputManager.moveInput *= pushForce * targetZoom * Time.deltaTime;
        cam.transform.position += new Vector3(inputManager.moveInput.x, inputManager.moveInput.y, 0);
        cam.transform.position = Mathf.Clamp(cam.transform.position.x, -10, 10) * Vector3.right + Mathf.Clamp(cam.transform.position.y, -10, 10) * Vector3.up + cam.transform.position.z * Vector3.forward;
    }
    public void Zoom()
    {
        float scroll = inputManager.input.Player.Zoom.ReadValue<float>();
        targetZoom -= scroll;
        targetZoom = Mathf.Clamp(targetZoom, 2f, 10f);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
    }
}
