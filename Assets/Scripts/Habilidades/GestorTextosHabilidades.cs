using UnityEngine;
using TMPro;

public class GestorTextosHabilidades : MonoBehaviour
{
    [Header("Textos Habilidades")]
    public TextMeshProUGUI textoTiendaVidaMaxima;
    public TextMeshProUGUI textoTiendaRecuperacionVida;
    public TextMeshProUGUI textoTiendaRoboSalud;
    public TextMeshProUGUI textoTiendaDanioGeneral;
    public TextMeshProUGUI textoTiendaDanioMelee;
    public TextMeshProUGUI textoTiendaDanioDistancia;
    public TextMeshProUGUI textoTiendaTiempoRecarga;
    public TextMeshProUGUI textoTiendaProbabilidadCritico;
    public TextMeshProUGUI textoTiendaGeneracionCajas;
    public TextMeshProUGUI textoTiendaMultiplicadorCalaveras;

    private GestorHabilidades gestorHabilidades;

    private void Start()
    {
        // Buscar el GestorHabilidades del jugador
        gestorHabilidades = FindAnyObjectByType<GestorHabilidades>();
        ActualizarTextos();
    }

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