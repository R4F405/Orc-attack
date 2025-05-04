using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla el menú principal del juego.
/// </summary>
/// <remarks>
/// Esta clase se encarga de gestionar los diferentes paneles del menú principal,
/// incluyendo la navegación entre ellos y las acciones de los botones.
/// </remarks>
public class ControladorMenuPrincipal: MonoBehaviour
{
    [Header("Paneles")]
    /// <summary>
    /// Panel que contiene los botones principales del menú.
    /// </summary>
    [SerializeField] private GameObject panelPrincipal; // Panel con botones principales
    
    /// <summary>
    /// Panel que contiene las opciones configurables del juego.
    /// </summary>
    [SerializeField] private GameObject panelOpciones;  // Panel de opciones
    
    /// <summary>
    /// Panel que contiene los créditos del juego (opcional).
    /// </summary>
    [SerializeField] private GameObject panelCreditos;  // Panel de créditos (opcional)
    
    [Header("Configuración")]
    /// <summary>
    /// Nombre de la escena que se cargará al iniciar el juego.
    /// </summary>
    [SerializeField] private string nombreEscenaJuego = "Nivel1"; // Nombre de la escena a cargar al jugar
    
    /// <summary>
    /// Nombre del sistema para los mensajes de debug.
    /// </summary>
    private const string NOMBRE_SISTEMA = "ControladorMenuPrincipal";
    
    /// <summary>
    /// Inicializa el menú principal mostrando el panel principal.
    /// </summary>
    private void Start()
    {
        // Mostrar panel principal al inicio
        MostrarPanelPrincipal();
    }
    
    /// <summary>
    /// Muestra el panel principal y oculta los demás.
    /// </summary>
    public void MostrarPanelPrincipal()
    {
        OcultarTodosPaneles();
        if (panelPrincipal != null)
            panelPrincipal.SetActive(true);
    }
    
    /// <summary>
    /// Abre el panel de opciones y actualiza los controles de volumen.
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
    /// Abre el panel de créditos del juego.
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
    /// Oculta todos los paneles del menú.
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
    
    /// <summary>
    /// Inicia el juego cargando la escena configurada.
    /// </summary>
    /// <remarks>
    /// Prepara la transición de audio y carga la escena del juego.
    /// </remarks>
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

    /// <summary>
    /// Cierra la aplicación o sale del modo juego en el editor.
    /// </summary>
    public void BotonSalir()
    {
        Application.Quit();
        
        // Si estás en el editor, esto permite salir del modo de juego
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
