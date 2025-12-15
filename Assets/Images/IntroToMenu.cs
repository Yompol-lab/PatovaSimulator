using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class IntroToMenu : MonoBehaviour
{
    public GameObject introCanvas;
    public GameObject menuCanvas;
    public float introDuration = 3f;

    void Start()
    {
        if (menuCanvas != null)
            menuCanvas.SetActive(false);

        StartCoroutine(IntroFlow());
    }

    IEnumerator IntroFlow()
    {
        yield return new WaitForSeconds(introDuration);

        if (introCanvas != null)
            introCanvas.SetActive(false);

        if (menuCanvas != null)
            menuCanvas.SetActive(true);
    }
}
