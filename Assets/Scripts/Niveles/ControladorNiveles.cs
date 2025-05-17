using UnityEngine;
using TMPro;

/// <summary>
/// Controla el sistema de niveles del juego.
/// </summary>
/// <remarks>
/// Esta clase se encarga de gestionar el progreso entre niveles, incluyendo el temporizador,
/// la generación de enemigos, la actualización de la interfaz y la transición entre niveles.
/// </remarks>
public class ControladorNiveles : MonoBehaviour
{
    /// <summary>
    /// Nivel actual en el que se encuentra el jugador.
    /// </summary>
    public int nivelActual = 1;
    
    /// <summary>
    /// Tiempo restante del nivel actual en segundos.
    /// </summary>
    public float tiempoRestante = 40f;
    
    /// <summary>
    /// Panel que contiene la tienda entre niveles.
    /// </summary>
    public GameObject panelTienda;
    
    /// <summary>
    /// Panel que muestra las mejoras disponibles al subir de nivel.
    /// </summary>
    public GameObject panelMejorasNivel;
    
    /// <summary>
    /// Texto que muestra el nivel actual en la interfaz.
    /// </summary>
    public TextMeshProUGUI textoNivel;
    
    /// <summary>
    /// Texto que muestra el tiempo restante en la interfaz.
    /// </summary>
    public TextMeshProUGUI textoTiempo;
    
    /// <summary>
    /// Generador de enemigos orcos.
    /// </summary>
    public GameObject GeneradorOrcos;
    
    /// <summary>
    /// Generador de enemigos magos (activo a partir del nivel 3).
    /// </summary>
    public GameObject GeneradorMagos;
    
    /// <summary>
    /// Generador de cajas destructibles.
    /// </summary>
    public GameObject GeneradorCajas;
    
    /// <summary>
    /// Sonido que se reproduce al finalizar un nivel.
    /// </summary>
    public AudioClip sonidoFinNivel;

    /// <summary>
    /// Indica si el nivel está actualmente en curso.
    /// </summary>
    private bool nivelEnCurso = true;
    
    /// <summary>
    /// Referencia al componente que gestiona las mejoras de nivel.
    /// </summary>
    private PanelMejorasNivel gestorMejorasNivel;
    
    /// <summary>
    /// Componente para reproducir efectos de sonido.
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// Inicializa el controlador de niveles y configura el primer nivel.
    /// </summary>
    void Start()
    {
        // Obtener o crear componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        ActualizarUI();
        panelTienda.SetActive(false); // Asegurar que la tienda esté desactivada al inicio
        
        if (panelMejorasNivel != null)
        {
            gestorMejorasNivel = panelMejorasNivel.GetComponent<PanelMejorasNivel>();
            panelMejorasNivel.SetActive(false);
        }
        
        // Desactivar GeneradorMagos en niveles antes del 3
        if (GeneradorMagos != null)
            GeneradorMagos.SetActive(nivelActual >= 3);
    }

    /// <summary>
    /// Actualiza el temporizador del nivel y verifica si ha terminado.
    /// </summary>
    void Update()
    {
        if (nivelEnCurso)
        {
            tiempoRestante -= Time.deltaTime;
            ActualizarUI();

            if (tiempoRestante <= 0)
            {
                FinalizarNivel();
            }
        }
    }

    /// <summary>
    /// Finaliza el nivel actual, desactiva los generadores y muestra las opciones de mejora.
    /// </summary>
    /// <remarks>
    /// Desactiva todos los generadores, elimina enemigos y objetos del nivel,
    /// reproduce el sonido de fin de nivel y prepara las pantallas de mejora.
    /// </remarks>
    void FinalizarNivel()
    {
        Debug.Log("Finalizando nivel...");
        GeneradorCajas.SetActive(false); //Desactiva generador
        GeneradorOrcos.SetActive(false); //Desactiva generador
        GeneradorMagos.SetActive(false); //Desactiva generador
        nivelEnCurso = false;
        tiempoRestante = 0;
        DestruirObjetosPorCapa("Enemigo");
        DestruirObjetosPorCapa("Caja");
        DestruirObjetosPorCapa("Calavera");
        DestruirObjetosPorCapa("ProyectilesEnemigos");
        DestruirObjetosPorCapa("Balas");
        
        // Reproducir sonido de fin de nivel
        if (sonidoFinNivel != null && audioSource != null)
        {
            audioSource.ReproducirConVolumenGlobal(sonidoFinNivel, 1.0f, TipoAudio.Efectos);
        }
        
        GameObject jugador = GameObject.FindGameObjectWithTag("Jugador");
        jugador.transform.position = Vector2.zero; // Coloca al jugador en (0,0,0)
        
        // Verificar si el panel de mejoras está configurado
        if (panelMejorasNivel != null && panelMejorasNivel.GetComponent<PanelMejorasNivel>() != null)
        {
            Debug.Log("Intentando mostrar panel de mejoras...");
            MostrarPanelMejorasNivel();
        }
        else
        {
            Debug.LogWarning("Panel de mejoras no configurado. Mostrando tienda directamente.");
            MostrarTienda();
        }
    }
    
    /// <summary>
    /// Muestra el panel de mejoras disponibles al finalizar un nivel.
    /// </summary>
    /// <remarks>
    /// Si el panel o el gestor no están disponibles, muestra la tienda como alternativa.
    /// </remarks>
    void MostrarPanelMejorasNivel()
    {
        if (panelMejorasNivel != null && gestorMejorasNivel != null)
        {
            Debug.Log("Panel y gestor encontrados, mostrando panel de mejoras");
            gestorMejorasNivel.MostrarPanel();
        }
        else
        {
            Debug.LogWarning("Panel o gestor no encontrados: panelMejorasNivel=" + 
                            (panelMejorasNivel != null ? "EXISTE" : "NULL") + 
                            ", gestorMejorasNivel=" + 
                            (gestorMejorasNivel != null ? "EXISTE" : "NULL"));
            MostrarTienda();
        }
    }
    
    /// <summary>
    /// Muestra la tienda entre niveles y pausa el juego.
    /// </summary>
    public void MostrarTienda()
    {
        // Pausar el juego al mostrar la tienda
        Time.timeScale = 0;
        
        panelTienda.SetActive(true);
    }

    /// <summary>
    /// Configura e inicia el siguiente nivel del juego.
    /// </summary>
    /// <remarks>
    /// Incrementa el contador de nivel, reinicia el temporizador, activa los generadores
    /// apropiados según el nivel, y restaura la salud del jugador al máximo.
    /// </remarks>
    public void IniciarSiguienteNivel()
    {
        // Reanudar el juego al iniciar el siguiente nivel
        Time.timeScale = 1;
        
        nivelActual++;
        tiempoRestante = 40f; // Tiempo fijo de 40 segundos para todos los niveles
        nivelEnCurso = true;
        panelTienda.SetActive(false); // Ocultar tienda
        if (panelMejorasNivel != null)
        {
            panelMejorasNivel.SetActive(false);
        }
        GeneradorCajas.SetActive(true);
        GeneradorOrcos.SetActive(true);
        // Activar GeneradorMagos solo a partir del nivel 3
        GeneradorMagos.SetActive(nivelActual >= 3);
        ActualizarUI();
        
        // Curar al jugador al máximo al iniciar un nuevo nivel
        GameObject jugador = GameObject.FindGameObjectWithTag("Jugador");
        if (jugador != null)
        {
            VidaJugador vidaJugador = jugador.GetComponent<VidaJugador>();
            if (vidaJugador != null)
            {
                vidaJugador.Curar(vidaJugador.ObtenerSaludMaxima());
                Debug.Log("Jugador curado al máximo: " + vidaJugador.ObtenerSalud() + "/" + vidaJugador.ObtenerSaludMaxima());
            }
        }
    }

    /// <summary>
    /// Elimina todos los objetos de una capa específica.
    /// </summary>
    /// <param name="nombreCapa">Nombre de la capa cuyos objetos se eliminarán.</param>
    void DestruirObjetosPorCapa(string nombreCapa)
    {
        int capa = LayerMask.NameToLayer(nombreCapa);
        GameObject[] objetos = GameObject.FindObjectsByType<GameObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (GameObject obj in objetos)
        {
            if (obj.layer == capa)
            {
                Destroy(obj);
            }
        }
    }

    /// <summary>
    /// Actualiza los elementos de la interfaz de usuario con la información del nivel.
    /// </summary>
    void ActualizarUI()
    {
        textoNivel.text = "Nivel " + nivelActual;
        textoTiempo.text = Mathf.Ceil(tiempoRestante).ToString();
    }
}
