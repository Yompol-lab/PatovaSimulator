using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    public Renderer targetRenderer;
    public Material[] materials;
    public float changeInterval = 1f;
    private int currentIndex = 0;

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        if (materials.Length > 0)
            targetRenderer.material = materials[currentIndex];

        InvokeRepeating(nameof(ChangeMaterial), changeInterval, changeInterval);
    }

    void ChangeMaterial()
    {
        if (materials.Length == 0) return;

        currentIndex = (currentIndex + 1) % materials.Length;
        targetRenderer.material = materials[currentIndex];
    }
}
