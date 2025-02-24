using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenuJuego : MonoBehaviour
{
    public GameObject menu;
    public GameObject opciones;
    public GameObject botonPorDefecto; // Asigna el botón "Continuar" desde el inspector

    void Start()
    {
        Time.timeScale = 1; // Asegurar que el juego no esté pausado al volver
        menu.SetActive(false); // Ocultar el menú al iniciar
        opciones.SetActive(false); // Ocultar las opciones al iniciar
    }

     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Muestra/Oculta con ESC
        {
        if (opciones.activeSelf) 
            {
                BotonVolverMenu(); // Si está en opciones, vuelve al menú principal
            }
            else
            {
                PausarContinuarJuego(); // Si no, pausa o reanuda el juego
            }        
        }
    }

    public void PausarContinuarJuego()
    {
        menu.SetActive(!menu.activeSelf);
        opciones.SetActive(false); // Asegurar que opciones se cierre
        Time.timeScale = menu.activeSelf ? 0 : 1; // Pausar el juego al abrir el menú
    }

    public void BotonContinuar()
    {
        menu.SetActive(!menu.activeSelf);
        Time.timeScale = menu.activeSelf ? 0 : 1;
    }

    public void BotonOpciones()
    {
        menu.SetActive(false); // Oculta el menú principal
        opciones.SetActive(true); // Muestra el panel de opciones
    }

    public void BotonVolverMenu()
    {
        opciones.SetActive(false); // Oculta opciones
        menu.SetActive(true); // Vuelve al menú principal
    }

    public void BotonSalirMenuprincipal()
    {
        SceneManager.LoadScene(0);
    }

}
