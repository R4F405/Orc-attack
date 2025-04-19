using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarraExperiencia : MonoBehaviour
{
    public Slider barraExp;
    public TextMeshProUGUI textoNivel; // Texto donde se mostrará el nivel
    public int experienciaActual = 0;
    public int experienciaMaxima = 100;
    public int experienciaPorEnemigo = 10;

    private int nivel = 1;
    private GestorHabilidades gestorHabilidades;
    private VidaJugador vidaJugador;

    void Start()
    {
        // Asegura que la barra comience en la posición correcta
        ActualizarBarra();
        
        // Buscar el gestor de habilidades para aplicar mejoras al subir de nivel
        gestorHabilidades = GetComponent<GestorHabilidades>();
        vidaJugador = GetComponent<VidaJugador>();
    }

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

    void ActualizarBarra()
    {
        barraExp.value = (float)experienciaActual / experienciaMaxima;
    }
    
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
    
    // Para acceder al nivel desde otros scripts
    public int ObtenerNivelActual()
    {
        return nivel;
    }
}
