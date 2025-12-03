using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;   

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interacción")]
    public float interactDistance = 2.5f;
    public LayerMask npcLayer;

    [Header("UI (botones sueltos)")]
    public GameObject interactionRoot;
    public Button btnDNI;
    public Button btnGuardia;
    public Button btnVestimenta;
    public Button btnComportamiento;
    public Button btnAceptar;
    public Button btnRechazar;

    [Header("UI DNI")]
    public Image dniImage;

    [Header("Crosshair")]
    public GameObject crosshair;

    [Header("Contrabando")]
    public Transform handHoldPoint;     
    public LayerMask contrabandLayer;   
    public LayerMask trashLayer;        

    private Camera cam;
    private NPCInteractionData currentNPC;
    private GameObject carriedContraband;

    void Start()
    {
        cam = Camera.main;

        if (interactionRoot != null)
            interactionRoot.SetActive(false);

        if (dniImage != null)
            dniImage.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (crosshair != null) crosshair.SetActive(true);
    }

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null) return;

        
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryOpenInteraction();
        }

        if (Keyboard.current.eKey.wasReleasedThisFrame)
        {
            CloseButtons();
        }

        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (carriedContraband == null)
                TryPickupContraband();
            else
                TryThrowContraband();
        }
    }

   

    void TryOpenInteraction()
    {
        if (interactionRoot != null && interactionRoot.activeSelf)
            return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, interactDistance, npcLayer))
            return;

        NPCInteractionData npcData = hit.collider.GetComponentInParent<NPCInteractionData>();
        if (npcData == null) return;

        NPCQueueMovement move = npcData.GetComponent<NPCQueueMovement>();
        if (move == null || !move.IsFirstInLine)
            return;

        currentNPC = npcData;
        OpenButtons();
    }

    void OpenButtons()
    {
        if (interactionRoot == null) return;

        interactionRoot.SetActive(true);

        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (crosshair != null) crosshair.SetActive(false);

        
        btnDNI.onClick.RemoveAllListeners();
        btnGuardia.onClick.RemoveAllListeners();
        btnVestimenta.onClick.RemoveAllListeners();
        btnComportamiento.onClick.RemoveAllListeners();
        btnAceptar.onClick.RemoveAllListeners();
        btnRechazar.onClick.RemoveAllListeners();

       
        btnDNI.onClick.AddListener(ShowCurrentNPCDNI);
        btnGuardia.onClick.AddListener(() => currentNPC.PerformGuardCheck());
        btnVestimenta.onClick.AddListener(() => currentNPC.CheckDressCode());
        btnComportamiento.onClick.AddListener(() => currentNPC.CheckBehavior());
        btnAceptar.onClick.AddListener(AcceptNPC);
        btnRechazar.onClick.AddListener(RejectNPC);
    }

    void CloseButtons()
    {
        if (interactionRoot != null)
            interactionRoot.SetActive(false);

        if (dniImage != null)
            dniImage.gameObject.SetActive(false);

        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (crosshair != null) crosshair.SetActive(true);

        currentNPC = null;
    }

    void ShowCurrentNPCDNI()
    {
        if (currentNPC == null || dniImage == null) return;

        if (currentNPC.dniImagen == null)
        {
            Debug.LogWarning("El NPC " + currentNPC.npcName + " no tiene dniImagen asignado.");
            return;
        }

        dniImage.sprite = currentNPC.dniImagen;
        dniImage.gameObject.SetActive(true);
    }

    void AcceptNPC()
    {
        if (currentNPC != null)
            currentNPC.EnterClub();

        CloseButtons();
    }

    void RejectNPC()
    {
        if (currentNPC != null)
            currentNPC.LeaveClub();

        CloseButtons();
    }

    

    void TryPickupContraband()
    {
        if (handHoldPoint == null) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, interactDistance, contrabandLayer))
            return;

        GameObject obj = hit.collider.gameObject;
        carriedContraband = obj;

        
        obj.transform.SetParent(handHoldPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;
    }

    void TryThrowContraband()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        
        if (Physics.Raycast(ray, out hit, interactDistance, trashLayer))
        {
            
            Destroy(carriedContraband);
            carriedContraband = null;
            Debug.Log("Tiraste la merca a la basura");
            return;
        }

        
        carriedContraband.transform.SetParent(null);
        Rigidbody rb = carriedContraband.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;

        carriedContraband = null;
    }
}
