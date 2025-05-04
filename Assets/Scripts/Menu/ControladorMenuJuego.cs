using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla el menú de pausa durante el juego.
/// </summary>
/// <remarks>
/// Esta clase gestiona la pausa del juego, la navegación entre los paneles
/// del menú de pausa y las acciones de los botones mientras se juega.
/// </remarks>
public class ControladorMenuJuego : MonoBehaviour
{
    /// <summary>
    /// Panel principal del menú de pausa.
    /// </summary>
    public GameObject menu;
    
    /// <summary>
    /// Panel de opciones dentro del menú de pausa.
    /// </summary>
    public GameObject opciones;
    
    /// <summary>
    /// Botón que debería seleccionarse por defecto al abrir el menú.
    /// </summary>
    /// <remarks>
    /// Normalmente se asigna el botón "Continuar" desde el inspector.
    /// </remarks>
    public GameObject botonPorDefecto; // Asigna el botón "Continuar" desde el inspector

    /// <summary>
    /// Inicializa el controlador del menú de juego.
    /// </summary>
    /// <remarks>
    /// Asegura que el juego no esté pausado y que los paneles estén ocultos al iniciar.
    /// </remarks>
    void Start()
    {
        Time.timeScale = 1; // Asegurar que el juego no esté pausado al volver
        menu.SetActive(false); // Ocultar el menú al iniciar
        opciones.SetActive(false); // Ocultar las opciones al iniciar
    }

    /// <summary>
    /// Detecta la tecla Escape para mostrar/ocultar el menú de pausa.
    /// </summary>
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

    /// <summary>
    /// Alterna entre el estado de pausa y juego.
    /// </summary>
    /// <remarks>
    /// Muestra u oculta el menú principal de pausa y ajusta el tiempo de juego.
    /// </remarks>
    public void PausarContinuarJuego()
    {
        menu.SetActive(!menu.activeSelf);
        opciones.SetActive(false); // Asegurar que opciones se cierre
        Time.timeScale = menu.activeSelf ? 0 : 1; // Pausar el juego al abrir el menú
    }

    /// <summary>
    /// Continúa el juego al cerrar el menú de pausa.
    /// </summary>
    public void BotonContinuar()
    {
        menu.SetActive(!menu.activeSelf);
        Time.timeScale = menu.activeSelf ? 0 : 1;
    }

    /// <summary>
    /// Muestra el panel de opciones y oculta el menú principal.
    /// </summary>
    public void BotonOpciones()
    {
        menu.SetActive(false); // Oculta el menú principal
        opciones.SetActive(true); // Muestra el panel de opciones
    }

    /// <summary>
    /// Regresa al menú principal de pausa desde el panel de opciones.
    /// </summary>
    public void BotonVolverMenu()
    {
        opciones.SetActive(false); // Oculta opciones
        menu.SetActive(true); // Vuelve al menú principal
    }

    /// <summary>
    /// Sale del juego actual y regresa al menú principal.
    /// </summary>
    public void BotonSalirMenuprincipal()
    {
        SceneManager.LoadScene(0);
    }
}
