using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    public InputSystem_Actions input;
    public Vector2 moveInput;
    public Vector2 mousePos;
    public Vector2 worldPos;
    public Vector3Int gridPosition;

    [SerializeField] CameraController cameraController;
    [SerializeField] PathfinderController pathfinderController;
    [SerializeField] PathfinderInitializer gridManager;
    [SerializeField] Tilemap tilemap;

    void Awake()
    {
        input = new InputSystem_Actions();
        cameraController = FindFirstObjectByType<CameraController>();
        pathfinderController = FindFirstObjectByType<PathfinderController>();
        gridManager = FindFirstObjectByType<PathfinderInitializer>();
    }

    void OnEnable() => input.Enable();
    void OnDisable() => input.Disable();

    void Update()
    {
        mousePos = input.Player.Point.ReadValue<Vector2>();
        moveInput = input.Player.Move.ReadValue<Vector2>();
        worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        gridPosition = tilemap.WorldToCell(worldPos);
    }

    void Start()
    {
        input.Player.Zoom.performed += ctx => OnZoom();
        input.Player.Interact.performed += ctx => OnInteract();
    }

    void OnZoom()
    {
        cameraController.Zoom();
    }

    void OnInteract()
    {
        pathfinderController.Controller(gridPosition);
    }
}
