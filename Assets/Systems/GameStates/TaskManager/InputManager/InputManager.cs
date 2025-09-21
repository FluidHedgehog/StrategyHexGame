using System.Collections;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    //------------------------------------------------------------------------------
    // Initialization of events
    //------------------------------------------------------------------------------
    public class GridPositionEvent : UnityEvent<Vector3Int> {}
        public GridPositionEvent OnGridPositionChanged = new GridPositionEvent();
        public GridPositionEvent Interact = new GridPositionEvent();

    public class ButtonEvent : UnityEvent {}
        public ButtonEvent Cancel = new ButtonEvent();
        public ButtonEvent EndTurn = new ButtonEvent();

    //------------------------------------------------------------------------------
    // Core references
    //------------------------------------------------------------------------------

    [SerializeField] CameraController cameraController;
    [SerializeField] PathController pathfinderController;
    [SerializeField] GridManager gridManager;
    [SerializeField] TaskManager taskManager;
    [SerializeField] Tilemap tilemap;

    //------------------------------------------------------------------------------
    // Core variables
    //------------------------------------------------------------------------------

    public InputSystem_Actions input;
    public Vector2 moveInput;
    public Vector2 mousePos;
    public Vector2 worldPos;
    public Vector3Int gridPosition;
    public Vector3Int lastGridPosition;

    //------------------------------------------------------------------------------
    // Initialization
    //------------------------------------------------------------------------------

    void Awake()
    {
        input = new InputSystem_Actions();
        cameraController = FindFirstObjectByType<CameraController>();
        pathfinderController = FindFirstObjectByType<PathController>();
        gridManager = FindFirstObjectByType<GridManager>();
    }

    void Start()
    {
        input.Player.Zoom.performed += ctx => OnZoom();
        input.Player.Interact.performed += ctx => OnInteract();
        input.Player.Cancel.performed += ctx => OnCancel();
        input.Player.EndTurn.performed += ctx => OnEndTurn();
    }

    void OnEnable() => input.Enable();
    void OnDisable() => input.Disable();

    //------------------------------------------------------------------------------
    // Detect mouse position
    //------------------------------------------------------------------------------

    void Update()
    {
        mousePos = input.Player.Point.ReadValue<Vector2>();
        moveInput = input.Player.Move.ReadValue<Vector2>();
        worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        gridPosition = tilemap.WorldToCell(worldPos);

        if (gridPosition != lastGridPosition)
        {
            lastGridPosition = gridPosition;
            OnGridPositionChanged?.Invoke(gridPosition);
        }
    }

    //------------------------------------------------------------------------------
    // Detecting input
    //------------------------------------------------------------------------------

    void OnZoom()
    {
        cameraController.Zoom();
    }

    void OnInteract()
    {
        Interact.Invoke(gridPosition);
    }

    void OnCancel()
    {
        Cancel.Invoke();
    }

    void OnEndTurn()
    {
        EndTurn.Invoke();
    }
}
