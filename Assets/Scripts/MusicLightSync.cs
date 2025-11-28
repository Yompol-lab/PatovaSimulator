using UnityEngine;

public class MusicLightSync : MonoBehaviour
{
    public MusicManager musicManager;
    public Light targetLight;
    public float intensityMultiplier = 2f;

    void Update()
    {
        float level = musicManager.currentAudioLevel;

        targetLight.intensity = 1 + (level * intensityMultiplier);
    }
}
