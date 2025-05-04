using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlador para ajustar el volumen desde la interfaz de usuario.
/// </summary>
/// <remarks>
/// Añadir este script a un panel o canvas que contenga sliders para controlar
/// los diferentes volúmenes del juego. Proporciona una interfaz entre los controles
/// de UI y el sistema de gestión de audio global, permitiendo al jugador personalizar
/// la experiencia auditiva del juego.
/// </remarks>
public class ControladorVolumen : MonoBehaviour
{
    /// <summary>
    /// Slider para controlar el volumen global del juego.
    /// </summary>
    [Header("Referencias UI")]
    [SerializeField] private Slider sliderVolumenGlobal;
    
    /// <summary>
    /// Slider para controlar el volumen de la música de fondo.
    /// </summary>
    [SerializeField] private Slider sliderVolumenMusica;
    
    /// <summary>
    /// Slider para controlar el volumen de los efectos de sonido.
    /// </summary>
    [SerializeField] private Slider sliderVolumenEfectos;
    
    /// <summary>
    /// Slider para controlar el volumen de los sonidos de la interfaz de usuario.
    /// </summary>
    [SerializeField] private Slider sliderVolumenUI;
    
    /// <summary>
    /// Determina si los sliders se actualizan automáticamente al iniciar con los valores actuales.
    /// </summary>
    [Header("Configuración")]
    [SerializeField] private bool actualizarAlIniciar = true;
    
    /// <summary>
    /// Volumen global por defecto (entre 0 y 1).
    /// </summary>
    [Header("Valores Por Defecto")]
    [SerializeField] private float volumenGlobalPorDefecto = 1.0f;
    
    /// <summary>
    /// Volumen de la música por defecto (entre 0 y 1).
    /// </summary>
    [SerializeField] private float volumenMusicaPorDefecto = 1.0f;
    
    /// <summary>
    /// Volumen de los efectos por defecto (entre 0 y 1).
    /// </summary>
    [SerializeField] private float volumenEfectosPorDefecto = 1.0f;
    
    /// <summary>
    /// Volumen de la UI por defecto (entre 0 y 1).
    /// </summary>
    [SerializeField] private float volumenUIPorDefecto = 1.0f;
    
    /// <summary>
    /// Inicializa los controles de volumen y configura sus valores iniciales.
    /// </summary>
    private void Start()
    {
        // Configurar listeners para los sliders
        ConfigurarSliders();
        
        // Actualizar valores iniciales desde la configuración actual
        if (actualizarAlIniciar)
        {
            ActualizarSlidersDesdeSistema();
        }
    }
    
    /// <summary>
    /// Configura los listeners para los controles deslizantes de volumen.
    /// </summary>
    /// <remarks>
    /// Conecta cada slider con su respectiva función de cambio de volumen.
    /// </remarks>
    private void ConfigurarSliders()
    {
        if (sliderVolumenGlobal != null)
        {
            sliderVolumenGlobal.onValueChanged.AddListener(CambiarVolumenGlobal);
        }
        
        if (sliderVolumenMusica != null)
        {
            sliderVolumenMusica.onValueChanged.AddListener(CambiarVolumenMusica);
        }
        
        if (sliderVolumenEfectos != null)
        {
            sliderVolumenEfectos.onValueChanged.AddListener(CambiarVolumenEfectos);
        }
        
        if (sliderVolumenUI != null)
        {
            sliderVolumenUI.onValueChanged.AddListener(CambiarVolumenUI);
        }
    }
    
    /// <summary>
    /// Actualiza las posiciones de los sliders con los valores actuales del sistema de audio.
    /// </summary>
    /// <remarks>
    /// Obtiene los valores actuales del GestorAudioGlobal y actualiza los sliders correspondientes.
    /// Puede ser llamado cuando se abre un menú de opciones para reflejar la configuración actual.
    /// </remarks>
    public void ActualizarSlidersDesdeSistema()
    {
        if (GestorAudioGlobal.instancia == null)
        {
            Debug.LogWarning("ControladorVolumen: No se encuentra instancia de GestorAudioGlobal");
            return;
        }
        
        if (sliderVolumenGlobal != null)
        {
            sliderVolumenGlobal.value = GestorAudioGlobal.instancia.volumenGlobal;
        }
        
        if (sliderVolumenMusica != null)
        {
            sliderVolumenMusica.value = GestorAudioGlobal.instancia.volumenMusica;
        }
        
        if (sliderVolumenEfectos != null)
        {
            sliderVolumenEfectos.value = GestorAudioGlobal.instancia.volumenEfectos;
        }
        
        if (sliderVolumenUI != null)
        {
            sliderVolumenUI.value = GestorAudioGlobal.instancia.volumenUI;
        }
    }
    
    /// <summary>
    /// Restaura todos los valores de volumen a los valores por defecto definidos en el inspector.
    /// </summary>
    /// <remarks>
    /// Útil para proporcionar un botón de "Restaurar valores" en el menú de opciones.
    /// </remarks>
    public void RestaurarValoresPorDefecto()
    {
        if (GestorAudioGlobal.instancia == null)
            return;
            
        // Establecer volúmenes en GestorAudioGlobal
        GestorAudioGlobal.instancia.EstablecerVolumenGlobal(volumenGlobalPorDefecto);
        GestorAudioGlobal.instancia.EstablecerVolumenMusica(volumenMusicaPorDefecto);
        GestorAudioGlobal.instancia.EstablecerVolumenEfectos(volumenEfectosPorDefecto);
        GestorAudioGlobal.instancia.EstablecerVolumenUI(volumenUIPorDefecto);
        
        // Actualizar sliders con los nuevos valores
        if (sliderVolumenGlobal != null)
            sliderVolumenGlobal.value = volumenGlobalPorDefecto;
            
        if (sliderVolumenMusica != null)
            sliderVolumenMusica.value = volumenMusicaPorDefecto;
            
        if (sliderVolumenEfectos != null)
            sliderVolumenEfectos.value = volumenEfectosPorDefecto;
            
        if (sliderVolumenUI != null)
            sliderVolumenUI.value = volumenUIPorDefecto;
            
        Debug.Log("ControladorVolumen: Valores de volumen restaurados a valores por defecto");
    }
    
    /// <summary>
    /// Cambia el volumen global cuando se mueve el slider correspondiente.
    /// </summary>
    /// <param name="valor">Nuevo valor para el volumen global (entre 0 y 1).</param>
    public void CambiarVolumenGlobal(float valor)
    {
        if (GestorAudioGlobal.instancia != null)
        {
            GestorAudioGlobal.instancia.EstablecerVolumenGlobal(valor);
        }
    }
    
    /// <summary>
    /// Cambia el volumen de la música cuando se mueve el slider correspondiente.
    /// </summary>
    /// <param name="valor">Nuevo valor para el volumen de música (entre 0 y 1).</param>
    public void CambiarVolumenMusica(float valor)
    {
        if (GestorAudioGlobal.instancia != null)
        {
            GestorAudioGlobal.instancia.EstablecerVolumenMusica(valor);
        }
    }
    
    /// <summary>
    /// Cambia el volumen de los efectos de sonido cuando se mueve el slider correspondiente.
    /// </summary>
    /// <param name="valor">Nuevo valor para el volumen de efectos (entre 0 y 1).</param>
    public void CambiarVolumenEfectos(float valor)
    {
        if (GestorAudioGlobal.instancia != null)
        {
            GestorAudioGlobal.instancia.EstablecerVolumenEfectos(valor);
        }
    }
    
    /// <summary>
    /// Cambia el volumen de los sonidos de UI cuando se mueve el slider correspondiente.
    /// </summary>
    /// <param name="valor">Nuevo valor para el volumen de UI (entre 0 y 1).</param>
    public void CambiarVolumenUI(float valor)
    {
        if (GestorAudioGlobal.instancia != null)
        {
            GestorAudioGlobal.instancia.EstablecerVolumenUI(valor);
        }
    }
    
    /// <summary>
    /// Silencia o restaura todo el audio del juego.
    /// </summary>
    /// <param name="silenciar">True para silenciar, False para restaurar el audio.</param>
    /// <remarks>
    /// Este método puede ser vinculado a un toggle de "Silenciar" en la interfaz.
    /// </remarks>
    public void AlternarSilencio(bool silenciar)
    {
        if (GestorAudioGlobal.instancia != null)
        {
            if (silenciar)
            {
                GestorAudioGlobal.instancia.EstablecerVolumenGlobal(0f);
                if (sliderVolumenGlobal != null)
                {
                    sliderVolumenGlobal.value = 0f;
                }
            }
            else
            {
                GestorAudioGlobal.instancia.EstablecerVolumenGlobal(1f);
                if (sliderVolumenGlobal != null)
                {
                    sliderVolumenGlobal.value = 1f;
                }
            }
        }
    }
} 