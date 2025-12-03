using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interacción NPC")]
    public float interactDistance = 2.5f;
    public LayerMask npcLayer;

    [Header("UI Principal")]
    public GameObject interactionRoot;
    public Button btnDNI;
    public Button btnEntrada;
    public Button btnVestimenta;
    public Button btnGuardia;
    public Button btnComportamiento;
    public Button btnAceptar;
    public Button btnRechazar;

    [Header("UI DNI")]
    public GameObject DNIPanelRoot;
    public Image dniImage;
    public Button btnCerrarDNI;

    [Header("UI Ticket")]
    public GameObject TicketPanelRoot;
    public Image ticketImage;
    public Button btnCerrarTicket;

    [Header("Crosshair")]
    public GameObject crosshair;

    [Header("Guardia")]
    public GuardController guardController;

    [Header("Contrabando")]
    public Transform handHoldPoint;
    public LayerMask contrabandLayer;

    private Camera cam;
    private FirstPersonCamera cameraFPS;
    private NPCInteractionData currentNPC;
    private bool menuAbierto = false;

    private GameObject carriedContraband;

    void Start()
    {
        cam = Camera.main;
        cameraFPS = cam.GetComponent<FirstPersonCamera>();

        if (interactionRoot != null) interactionRoot.SetActive(false);
        if (DNIPanelRoot != null) DNIPanelRoot.SetActive(false);
        if (TicketPanelRoot != null) TicketPanelRoot.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (crosshair != null) crosshair.SetActive(true);
    }

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!menuAbierto)
                TryOpenInteraction();
            else
                CloseAllMenus();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (carriedContraband == null)
                TryPickupContraband();
            else
                DropContraband();
        }
    }

    void TryOpenInteraction()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance, npcLayer))
            return;

        NPCInteractionData npc = hit.collider.GetComponentInParent<NPCInteractionData>();
        if (npc == null) return;

        NPCQueueMovement move = npc.GetComponent<NPCQueueMovement>();
        if (move != null && !move.IsFirstInLine)
            return;

        currentNPC = npc;
        OpenMainMenu();
    }

    void OpenMainMenu()
    {
        menuAbierto = true;

        interactionRoot.SetActive(true);
        if (DNIPanelRoot != null) DNIPanelRoot.SetActive(false);
        if (TicketPanelRoot != null) TicketPanelRoot.SetActive(false);

        if (cameraFPS != null) cameraFPS.freezeCamera = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (crosshair != null) crosshair.SetActive(false);

        btnDNI.onClick.RemoveAllListeners();
        btnEntrada.onClick.RemoveAllListeners();
        btnGuardia.onClick.RemoveAllListeners();
        btnVestimenta.onClick.RemoveAllListeners();
        btnComportamiento.onClick.RemoveAllListeners();
        btnAceptar.onClick.RemoveAllListeners();
        btnRechazar.onClick.RemoveAllListeners();

        btnDNI.onClick.AddListener(ShowDNI);
        btnEntrada.onClick.AddListener(ShowTicket);
        if (guardController != null)
            btnGuardia.onClick.AddListener(() => guardController.StartCheck(currentNPC, currentNPC.hasDrugs));
        btnVestimenta.onClick.AddListener(() => currentNPC.CheckDressCode());
        btnComportamiento.onClick.AddListener(() => currentNPC.CheckBehavior());
        btnAceptar.onClick.AddListener(AcceptNPC);
        btnRechazar.onClick.AddListener(RejectNPC);
    }

    void CloseAllMenus()
    {
        menuAbierto = false;

        if (interactionRoot != null) interactionRoot.SetActive(false);
        if (DNIPanelRoot != null) DNIPanelRoot.SetActive(false);
        if (TicketPanelRoot != null) TicketPanelRoot.SetActive(false);

        if (cameraFPS != null) cameraFPS.freezeCamera = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (crosshair != null) crosshair.SetActive(true);

        currentNPC = null;
    }

    void ShowDNI()
    {
        if (currentNPC == null || DNIPanelRoot == null || dniImage == null) return;

        interactionRoot.SetActive(false);
        DNIPanelRoot.SetActive(true);

        dniImage.sprite = currentNPC.dniImagen;

        btnCerrarDNI.onClick.RemoveAllListeners();
        btnCerrarDNI.onClick.AddListener(() =>
        {
            DNIPanelRoot.SetActive(false);
            interactionRoot.SetActive(true);
        });
    }

    void ShowTicket()
    {
        if (currentNPC == null || TicketPanelRoot == null || ticketImage == null) return;

        interactionRoot.SetActive(false);
        TicketPanelRoot.SetActive(true);

        ticketImage.sprite = currentNPC.ticketImagen;

        btnCerrarTicket.onClick.RemoveAllListeners();
        btnCerrarTicket.onClick.AddListener(() =>
        {
            TicketPanelRoot.SetActive(false);
            interactionRoot.SetActive(true);
        });
    }

    void AcceptNPC()
    {
        currentNPC?.EnterClub();
        CloseAllMenus();
    }

    void RejectNPC()
    {
        currentNPC?.LeaveClub();
        CloseAllMenus();
    }

    void TryPickupContraband()
    {
        if (handHoldPoint == null) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance, contrabandLayer))
            return;

        GameObject obj = hit.collider.gameObject;
        carriedContraband = obj;

        
        carriedContraband.transform.SetParent(handHoldPoint);
        carriedContraband.transform.localPosition = Vector3.zero;
        carriedContraband.transform.localRotation = Quaternion.identity;

        Rigidbody rb = carriedContraband.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }


    void DropContraband()
    {
        if (carriedContraband == null) return;

        carriedContraband.transform.SetParent(null);

        Rigidbody rb = carriedContraband.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;

        carriedContraband = null;
    }

    public GameObject GetCarriedContraband()
    {
        return carriedContraband;
    }
}
