using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }

    private DatabaseReference reference;
    public bool isReady = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {

                var options = new AppOptions
                {
                    ProjectId = "patova-simulator",
                    AppId = "1:394683873314:android:50b3f2555b3a9a1502b27b",
                    ApiKey = "AIzaSyCNWxQa2FdR3uiE2yx6VCocUy8Dw7idelI",

                    
                    DatabaseUrl = new System.Uri("https://patova-simulator-default-rtdb.firebaseio.com/")
                };



                FirebaseApp app = FirebaseApp.Create(options, "PatovaApp");

                
                reference = FirebaseDatabase.GetInstance(app).RootReference;

                isReady = true;
                Debug.Log("Firebase inicializado correctamente (PatovaApp)");
            }
            else
            {
                Debug.LogError(" No se pudieron resolver las dependencias de Firebase: " + dependencyStatus);
            }
        });
    }

    
    public void SaveDecision(NPCInteractionData.DecisionLog log)
    {
        if (!isReady || reference == null)
        {
            Debug.LogWarning("Firebase no está listo todavía, no se guardó la decisión.");
            return;
        }

        string key = reference.Child("decisionLogs").Push().Key;
        string json = JsonUtility.ToJson(log);

        reference.Child("decisionLogs").Child(key)
            .SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError(" Error al guardar decisión en Firebase: " + task.Exception);
                }
                else
                {
                    Debug.Log("Decisión guardada en Firebase con ID: " + key);
                }
            });
    }
}
