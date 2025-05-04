using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controla el sistema de experiencia y nivel del jugador.
/// </summary>
/// <remarks>
/// Esta clase gestiona la barra de experiencia visual, el aumento de nivel
/// y las bonificaciones que se aplican al subir de nivel.
/// </remarks>
public class BarraExperiencia : MonoBehaviour
{
    /// <summary>
    /// Referencia al componente Slider que representa visualmente la barra de experiencia.
    /// </summary>
    public Slider barraExp;
    
    /// <summary>
    /// Texto que muestra el nivel actual del jugador.
    /// </summary>
    public TextMeshProUGUI textoNivel; // Texto donde se mostrará el nivel
    
    /// <summary>
    /// Cantidad actual de experiencia acumulada en el nivel actual.
    /// </summary>
    public int experienciaActual = 0;
    
    /// <summary>
    /// Cantidad de experiencia necesaria para subir al siguiente nivel.
    /// </summary>
    public int experienciaMaxima = 100;
    
    /// <summary>
    /// Cantidad de experiencia que se gana al derrotar a un enemigo.
    /// </summary>
    public int experienciaPorEnemigo = 10;

    /// <summary>
    /// Nivel actual del jugador.
    /// </summary>
    private int nivel = 1;
    
    /// <summary>
    /// Referencia al componente que gestiona las habilidades del jugador.
    /// </summary>
    private GestorHabilidades gestorHabilidades;
    
    /// <summary>
    /// Referencia al componente que gestiona la vida del jugador.
    /// </summary>
    private VidaJugador vidaJugador;

    /// <summary>
    /// Inicializa la barra de experiencia y busca componentes necesarios.
    /// </summary>
    void Start()
    {
        // Asegura que la barra comience en la posición correcta
        ActualizarBarra();
        
        // Buscar el gestor de habilidades para aplicar mejoras al subir de nivel
        gestorHabilidades = GetComponent<GestorHabilidades>();
        vidaJugador = GetComponent<VidaJugador>();
    }

    /// <summary>
    /// Aumenta la experiencia del jugador y gestiona la subida de nivel.
    /// </summary>
    /// <remarks>
    /// Se llama cuando el jugador derrota a un enemigo, añadiendo experiencia
    /// y verificando si debe subir de nivel.
    /// </remarks>
    public void GanarExperiencia()
    {
        experienciaActual += experienciaPorEnemigo;

        if (experienciaActual >= experienciaMaxima)
        {
            experienciaActual = 0; // Reinicia la experiencia al subir de nivel
            nivel++;
            textoNivel.text = nivel.ToString();
            
            // Aplicar bonificaciones por subir de nivel
            AplicarBonificacionesDeNivel();
            
            // Aumentar la experiencia necesaria para el próximo nivel
            experienciaMaxima = Mathf.RoundToInt(experienciaMaxima * 1.2f);
        }

        ActualizarBarra();
    }

    /// <summary>
    /// Actualiza la barra visual de experiencia según la experiencia actual.
    /// </summary>
    void ActualizarBarra()
    {
        barraExp.value = (float)experienciaActual / experienciaMaxima;
    }
    
    /// <summary>
    /// Aplica mejoras automáticas al jugador cuando sube de nivel.
    /// </summary>
    /// <remarks>
    /// Estas mejoras incluyen curación completa, aumento de vida y,
    /// dependiendo del nivel alcanzado, mejoras de daño, crítico y recarga.
    /// </remarks>
    void AplicarBonificacionesDeNivel()
    {
        // Cada vez que el jugador sube de nivel, recibe algunas mejoras automáticas
        
        // Curar completamente al jugador
        if (vidaJugador != null)
        {
            vidaJugador.Curar(vidaJugador.ObtenerSaludMaxima());
        }
        
        // Si hay gestor de habilidades, aplicar mejoras aleatorias
        if (gestorHabilidades != null)
        {
            // Aplicar mejora automática de vida cada nivel
            gestorHabilidades.AplicarHabilidadPorID(1); // Aumentar vida
            
            // Cada 3 niveles, aplicar una mejora de daño
            if (nivel % 3 == 0)
            {
                gestorHabilidades.AplicarHabilidadPorID(4); // Aumentar daño general
            }
            
            // Cada 5 niveles, mejorar critico y velocidad de recarga
            if (nivel % 5 == 0)
            {
                gestorHabilidades.AplicarHabilidadPorID(8); // Aumentar probabilidad crítico
                gestorHabilidades.AplicarHabilidadPorID(7); // Reducir recarga
            }
            
            Debug.Log($"¡Nivel {nivel} alcanzado! Bonificaciones aplicadas.");
        }
    }
    
    /// <summary>
    /// Devuelve el nivel actual del jugador.
    /// </summary>
    /// <returns>Nivel actual del jugador.</returns>
    public int ObtenerNivelActual()
    {
        return nivel;
    }
}
