using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenuPrincipal: MonoBehaviour
{
    public void BotonIniciar()
    {
        SceneManager.LoadScene(1);
    }

    public void BotonSalir()
    {
        Application.Quit();
    }

}
