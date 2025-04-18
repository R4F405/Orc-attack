using UnityEngine;
using TMPro;

public class ControladorNiveles : MonoBehaviour
{
    public int nivelActual = 1;
    public float tiempoRestante = 20f;
    public GameObject panelTienda;
    public GameObject panelMejorasNivel;
    public TextMeshProUGUI textoNivel;
    public TextMeshProUGUI textoTiempo;
    public GameObject GeneradorOrcos;
    public GameObject GeneradorMagos;
    public GameObject GeneradorCajas;

    private bool nivelEnCurso = true;
    private PanelMejorasNivel gestorMejorasNivel;

    void Start()
    {
        ActualizarUI();
        panelTienda.SetActive(false); // Asegurar que la tienda esté desactivada al inicio
        
        if (panelMejorasNivel != null)
        {
            gestorMejorasNivel = panelMejorasNivel.GetComponent<PanelMejorasNivel>();
            panelMejorasNivel.SetActive(false);
        }
    }

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
    
    public void MostrarTienda()
    {
        // Pausar el juego al mostrar la tienda
        Time.timeScale = 0;
        
        panelTienda.SetActive(true);
    }

    public void IniciarSiguienteNivel()
    {
        // Reanudar el juego al iniciar el siguiente nivel
        Time.timeScale = 1;
        
        nivelActual++;
        tiempoRestante = 20 + (nivelActual - 1) * 5; // Se suma 5 segundos por nivel
        nivelEnCurso = true;
        panelTienda.SetActive(false); // Ocultar tienda
        if (panelMejorasNivel != null)
        {
            panelMejorasNivel.SetActive(false);
        }
        GeneradorCajas.SetActive(true);
        GeneradorOrcos.SetActive(true);
        GeneradorMagos.SetActive(true);
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

    void ActualizarUI()
    {
        textoNivel.text = "Nivel " + nivelActual;
        textoTiempo.text = Mathf.Ceil(tiempoRestante).ToString();
    }
}
