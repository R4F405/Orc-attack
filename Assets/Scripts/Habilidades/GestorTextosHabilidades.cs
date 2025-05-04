using UnityEngine;
using TMPro;

/// <summary>
/// Gestiona la actualización y visualización de los textos informativos de las habilidades en la interfaz.
/// </summary>
/// <remarks>
/// Esta clase se encarga de mantener actualizados los textos que muestran las estadísticas
/// y mejoras del jugador en la interfaz de usuario, obteniendo los datos desde el GestorHabilidades
/// y otros componentes relacionados.
/// </remarks>
public class GestorTextosHabilidades : MonoBehaviour
{
    [Header("Textos Habilidades")]
    /// <summary>
    /// Texto que muestra la salud máxima actual del jugador.
    /// </summary>
    public TextMeshProUGUI textoTiendaVidaMaxima;
    /// <summary>
    /// Texto que muestra el tiempo de recuperación de vida del jugador.
    /// </summary>
    public TextMeshProUGUI textoTiendaRecuperacionVida;
    /// <summary>
    /// Texto que muestra la probabilidad de robo de salud.
    /// </summary>
    public TextMeshProUGUI textoTiendaRoboSalud;
    /// <summary>
    /// Texto que muestra el porcentaje de aumento de daño general.
    /// </summary>
    public TextMeshProUGUI textoTiendaDanioGeneral;
    /// <summary>
    /// Texto que muestra el porcentaje de aumento de daño para armas cuerpo a cuerpo.
    /// </summary>
    public TextMeshProUGUI textoTiendaDanioMelee;
    /// <summary>
    /// Texto que muestra el porcentaje de aumento de daño para armas a distancia.
    /// </summary>
    public TextMeshProUGUI textoTiendaDanioDistancia;
    /// <summary>
    /// Texto que muestra la reducción en el tiempo de recarga de las armas.
    /// </summary>
    public TextMeshProUGUI textoTiendaTiempoRecarga;
    /// <summary>
    /// Texto que muestra la probabilidad de golpes críticos.
    /// </summary>
    public TextMeshProUGUI textoTiendaProbabilidadCritico;
    /// <summary>
    /// Texto que muestra el tiempo de generación de cajas de recursos.
    /// </summary>
    public TextMeshProUGUI textoTiendaGeneracionCajas;
    /// <summary>
    /// Texto que muestra el multiplicador de calaveras actual.
    /// </summary>
    public TextMeshProUGUI textoTiendaMultiplicadorCalaveras;

    /// <summary>
    /// Referencia al gestor de habilidades del jugador.
    /// </summary>
    private GestorHabilidades gestorHabilidades;

    /// <summary>
    /// Inicializa las referencias y actualiza los textos al inicio.
    /// </summary>
    private void Start()
    {
        // Buscar el GestorHabilidades del jugador
        gestorHabilidades = FindAnyObjectByType<GestorHabilidades>();
        ActualizarTextos();
    }

    /// <summary>
    /// Actualiza todos los textos con los valores actuales de las habilidades.
    /// </summary>
    /// <remarks>
    /// Este método obtiene los valores actuales de diferentes componentes del juego
    /// y actualiza los textos correspondientes en la interfaz de usuario con la
    /// información más reciente sobre las habilidades y mejoras del jugador.
    /// </remarks>
    public void ActualizarTextos()
    {
        if (gestorHabilidades == null)
        {
            gestorHabilidades = FindAnyObjectByType<GestorHabilidades>();
            if (gestorHabilidades == null)
            {
                Debug.LogWarning("No se encontró el GestorHabilidades");
                return;
            }
        }

        // Obtener componentes necesarios
        GameObject jugador = gestorHabilidades.gameObject;
        VidaJugador vidaJugador = gestorHabilidades.GetComponent<VidaJugador>();
        ArmasMelee armasMelee = FindAnyObjectByType<ArmasMelee>();
        ArmasDistancia armasDistancia = FindAnyObjectByType<ArmasDistancia>();
        GeneradorCajas generadorCajas = FindAnyObjectByType<GeneradorCajas>();
        InventarioJugador inventarioJugador = jugador.GetComponent<InventarioJugador>();

        // 1. Actualizar texto de Salud Máxima
        if (textoTiendaVidaMaxima != null && vidaJugador != null)
        {
            textoTiendaVidaMaxima.text = "Salud maxima: " + vidaJugador.ObtenerSaludMaxima() + " ps";
        }

        // 2. Actualizar texto de Recuperación de Vida
        if (textoTiendaRecuperacionVida != null && vidaJugador != null)
        {
            float tiempoRecuperacion = vidaJugador.tiempoEntreRecuperaciones;
            textoTiendaRecuperacionVida.text = "Recuperacion vida: " + tiempoRecuperacion.ToString("F1") + "s";
        }

        // 3. Actualizar texto de Robo de Salud
        if (textoTiendaRoboSalud != null && gestorHabilidades != null)
        {
            // Obtenemos el aumento acumulado usando el nuevo método
            int aumentoRoboSalud = gestorHabilidades.ObtenerAumentoProbabilidadRoboVida();
            textoTiendaRoboSalud.text = "Robo salud: +" + aumentoRoboSalud + "%";
        }

        // 4. Actualizar texto de Daño General
        if (textoTiendaDanioGeneral != null && gestorHabilidades != null)
        {
            int porcentajeAumento = gestorHabilidades.ObtenerAumentoDanioPorcentaje();
            textoTiendaDanioGeneral.text = "Daño general: +" + porcentajeAumento + "%";
        }

        // 5. Actualizar texto de Daño Melee
        if (textoTiendaDanioMelee != null && gestorHabilidades != null)
        {
            int porcentajeAumentoMelee = gestorHabilidades.ObtenerAumentoDanioMeleePorcentaje();
            textoTiendaDanioMelee.text = "Daño melee: +" + porcentajeAumentoMelee + "%";
        }

        // 6. Actualizar texto de Daño Distancia
        if (textoTiendaDanioDistancia != null && gestorHabilidades != null)
        {
            int porcentajeAumentoDistancia = gestorHabilidades.ObtenerAumentoDanioDistanciaPorcentaje();
            textoTiendaDanioDistancia.text = "Daño distancia: +" + porcentajeAumentoDistancia + "%";
        }

        // 7. Actualizar texto de Tiempo de Recarga
        if (textoTiendaTiempoRecarga != null && gestorHabilidades != null)
        {
            int disminucionRecargaPorcentaje = gestorHabilidades.ObtenerDisminucionRecargaPorcentaje();
            textoTiendaTiempoRecarga.text = "Recarga armas: -" + disminucionRecargaPorcentaje + "%";
        }

        // 8. Actualizar texto de Probabilidad de Crítico
        if (textoTiendaProbabilidadCritico != null && gestorHabilidades != null)
        {
            int aumentoProbabilidadCritico = gestorHabilidades.ObtenerAumentoProbabilidadCritico();
            textoTiendaProbabilidadCritico.text = "Probabilidad critico: +" + aumentoProbabilidadCritico + "%";
        }

        // 9. Actualizar texto de Generación de Cajas
        if (textoTiendaGeneracionCajas != null && gestorHabilidades != null)
        {
            float disminucionTiempo = gestorHabilidades.ObtenerDisminucionTiempoGeneracionCajas();
            textoTiendaGeneracionCajas.text = "Generacion cajas: -" + disminucionTiempo.ToString("F1") + "s";
        }

        // 10. Actualizar texto de Multiplicador de Calaveras
        if (textoTiendaMultiplicadorCalaveras != null && gestorHabilidades != null)
        {
            int multiplicador = gestorHabilidades.ObtenerMultiplicadorCalaveras();
            // Si el multiplicador es 0, mostramos 1 (sin multiplicación)
            if (multiplicador == 0) multiplicador = 1;
            textoTiendaMultiplicadorCalaveras.text = "Calaveras: x" + multiplicador;
        }
    }
} 