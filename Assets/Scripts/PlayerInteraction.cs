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

    private Camera cam;
    private NPCInteractionData currentNPC;
    private FirstPersonCamera camController;   

    void Start()
    {
        cam = Camera.main;
        if (cam != null)
            camController = cam.GetComponent<FirstPersonCamera>(); 

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
        if (Keyboard.current == null) return;

        
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryOpenInteraction();
        }

        
        if (Keyboard.current.eKey.wasReleasedThisFrame)
        {
            CloseButtons();
        }
    }

    void TryOpenInteraction()
    {
       
        if (interactionRoot != null && interactionRoot.activeSelf)
            return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        
        if (!Physics.Raycast(ray, out hit, interactDistance, npcLayer))
        {
            Debug.Log("Raycast no pegó en ningún NPC.");
            return;
        }

        NPCInteractionData npcData = hit.collider.GetComponentInParent<NPCInteractionData>();
        if (npcData == null)
        {
            Debug.Log("El objeto golpeado no tiene NPCInteractionData.");
            return;
        }

        NPCQueueMovement move = npcData.GetComponent<NPCQueueMovement>();
        if (move == null || !move.IsFirstInLine)
        {
            Debug.Log("NPC no es el primero de la fila o no tiene NPCQueueMovement.");
            return;
        }

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
        if (camController != null) camController.freezeCamera = true;

        
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
        if (camController != null) camController.freezeCamera = false;

        currentNPC = null;
    }

    
    void ShowCurrentNPCDNI()
    {
        if (currentNPC == null || dniImage == null)
        {
            Debug.LogWarning("No hay currentNPC o dniImage sin asignar en el inspector.");
            return;
        }

        if (currentNPC.dniImagen == null)
        {
            Debug.LogWarning("El NPC " + currentNPC.npcName + " no tiene dniImagen asignado.");
            return;
        }

        Debug.Log("Mostrando DNI de " + currentNPC.npcName);

        dniImage.sprite = currentNPC.dniImagen;
        dniImage.gameObject.SetActive(true);

        
        dniImage.transform.SetAsLastSibling();
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
}
