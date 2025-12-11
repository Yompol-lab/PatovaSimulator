using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    
    public void Jugar()
    {
        
        SceneManager.LoadScene("Game");
    }


    public void Salir()
    {
        Debug.Log("Salir del juego...");
        Application.Quit();
    } 

       

}
