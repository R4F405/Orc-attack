using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenuPrincipal: MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject panelPrincipal; // Panel con botones principales
    [SerializeField] private GameObject panelOpciones;  // Panel de opciones
    [SerializeField] private GameObject panelCreditos;  // Panel de créditos (opcional)
    
    [Header("Configuración")]
    [SerializeField] private string nombreEscenaJuego = "Nivel1"; // Nombre de la escena a cargar al jugar
    
    // Nombre del sistema para debug
    private const string NOMBRE_SISTEMA = "ControladorMenuPrincipal";
    
    private void Start()
    {
        // Mostrar panel principal al inicio
        MostrarPanelPrincipal();
    }
    
    /// <summary>
    /// Muestra el panel principal y oculta los demás
    /// </summary>
    public void MostrarPanelPrincipal()
    {
        OcultarTodosPaneles();
        if (panelPrincipal != null)
            panelPrincipal.SetActive(true);
    }
    
    /// <summary>
    /// Abre el panel de opciones
    /// </summary>
    public void MostrarOpciones()
    {
        OcultarTodosPaneles();
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(true);
            
            // Si hay un ControladorVolumen, actualizamos los sliders
            ControladorVolumen controlVolumen = panelOpciones.GetComponentInChildren<ControladorVolumen>();
            if (controlVolumen != null)
                controlVolumen.ActualizarSlidersDesdeSistema();
                
            Debug.Log($"{NOMBRE_SISTEMA}: Panel de opciones abierto");
        }
        else
        {
            Debug.LogWarning($"{NOMBRE_SISTEMA}: Panel de opciones no asignado");
        }
    }
    
    /// <summary>
    /// Abre el panel de créditos
    /// </summary>
    public void MostrarCreditos()
    {
        OcultarTodosPaneles();
        if (panelCreditos != null)
        {
            panelCreditos.SetActive(true);
            Debug.Log($"{NOMBRE_SISTEMA}: Panel de créditos abierto");
        }
        else
        {
            Debug.LogWarning($"{NOMBRE_SISTEMA}: Panel de créditos no asignado");
        }
    }
    
    /// <summary>
    /// Oculta todos los paneles del menú
    /// </summary>
    private void OcultarTodosPaneles()
    {
        if (panelPrincipal != null)
            panelPrincipal.SetActive(false);
            
        if (panelOpciones != null)
            panelOpciones.SetActive(false);
            
        if (panelCreditos != null)
            panelCreditos.SetActive(false);
    }
    
    public void BotonIniciar()
    {
        // Verificar si hay un componente MusicaMenuPrincipal para preparar la transición
        MusicaMenuPrincipal musicaMenu = FindObjectOfType<MusicaMenuPrincipal>();
        if (musicaMenu != null)
        {
            musicaMenu.PrepararTransicionANivel();
        }
        
        // Cargar la escena del juego
        if (string.IsNullOrEmpty(nombreEscenaJuego))
        {
            SceneManager.LoadScene(1); // Cargar escena 1 por defecto
        }
        else
        {
            SceneManager.LoadScene(nombreEscenaJuego); // Cargar la escena configurada
        }
    }

    public void BotonSalir()
    {
        Application.Quit();
        
        // Si estás en el editor, esto permite salir del modo de juego
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
