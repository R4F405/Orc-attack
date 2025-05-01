using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlador para ajustar el volumen desde la interfaz de usuario.
/// Añadir este script a un panel o canvas que contenga sliders para controlar el volumen.
/// </summary>
public class ControladorVolumen : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private Slider sliderVolumenGlobal;
    [SerializeField] private Slider sliderVolumenMusica;
    [SerializeField] private Slider sliderVolumenEfectos;
    [SerializeField] private Slider sliderVolumenUI;
    
    [Header("Configuración")]
    [SerializeField] private bool actualizarAlIniciar = true;
    
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
    /// Configura los listeners para los sliders
    /// </summary>
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
    /// Actualiza los valores de los sliders desde el sistema de audio global
    /// </summary>
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
    /// Cambia el volumen global cuando se mueve el slider
    /// </summary>
    public void CambiarVolumenGlobal(float valor)
    {
        if (GestorAudioGlobal.instancia != null)
        {
            GestorAudioGlobal.instancia.EstablecerVolumenGlobal(valor);
        }
    }
    
    /// <summary>
    /// Cambia el volumen de la música cuando se mueve el slider
    /// </summary>
    public void CambiarVolumenMusica(float valor)
    {
        if (GestorAudioGlobal.instancia != null)
        {
            GestorAudioGlobal.instancia.EstablecerVolumenMusica(valor);
        }
    }
    
    /// <summary>
    /// Cambia el volumen de los efectos cuando se mueve el slider
    /// </summary>
    public void CambiarVolumenEfectos(float valor)
    {
        if (GestorAudioGlobal.instancia != null)
        {
            GestorAudioGlobal.instancia.EstablecerVolumenEfectos(valor);
        }
    }
    
    /// <summary>
    /// Cambia el volumen de la UI cuando se mueve el slider
    /// </summary>
    public void CambiarVolumenUI(float valor)
    {
        if (GestorAudioGlobal.instancia != null)
        {
            GestorAudioGlobal.instancia.EstablecerVolumenUI(valor);
        }
    }
    
    /// <summary>
    /// Silencia/No silencia todo el audio del juego
    /// </summary>
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