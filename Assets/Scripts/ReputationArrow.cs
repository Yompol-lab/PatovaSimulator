using System.Collections;
using UnityEngine;

public class ReputationArrow : MonoBehaviour
{
    public RectTransform arrowTransform;

    [Header("Ángulos")]
    public float greenAngle = 30f;
    public float yellowAngle = 90f;
    public float redAngle = 160f;

    [Header("Puntaje base")]
    [Range(0, 100)]
    public int score = 50;        
    public int goodDelta = 10;      
    public int badDelta = -15;      

    [Header("Multiplicador de tolerancia")]
    public float toleranceMultiplier = 1f; 

    [Header("Rotación")]
    public float rotateSpeed = 360f;

    float targetAngle;

    void Awake()
    {
        if (arrowTransform == null)
            arrowTransform = GetComponent<RectTransform>();

        score = Mathf.Clamp(score, 0, 100);
        targetAngle = GetAngleFromScore(score);
        SetArrowInstant(targetAngle);
    }

    void Update()
    {
        if (arrowTransform == null) return;

        float currentZ = arrowTransform.localEulerAngles.z;
        float newZ = Mathf.MoveTowardsAngle(currentZ, targetAngle, rotateSpeed * Time.deltaTime);

        Vector3 euler = arrowTransform.localEulerAngles;
        euler.z = newZ;
        arrowTransform.localEulerAngles = euler;
    }

    public void ApplyGoodDecision()
    {
        ModifyScore(goodDelta);
    }

    public void ApplyBadDecision()
    {
        int finalDelta = Mathf.RoundToInt(badDelta * toleranceMultiplier);
        ModifyScore(finalDelta);
    }

    public void SetToleranceMultiplier(float mult)
    {
        toleranceMultiplier = Mathf.Max(0f, mult);
    }

    

    void ModifyScore(int delta)
    {
        score = Mathf.Clamp(score + delta, 0, 100);
        targetAngle = GetAngleFromScore(score);
    }

    float GetAngleFromScore(int s)
    {
        
        if (s <= 40) return redAngle;
        if (s < 60) return yellowAngle;
        return greenAngle;
    }

    void SetArrowInstant(float angle)
    {
        if (arrowTransform == null) return;

        Vector3 euler = arrowTransform.localEulerAngles;
        euler.z = angle;
        arrowTransform.localEulerAngles = euler;
    }
}
