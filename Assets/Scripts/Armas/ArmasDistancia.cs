using UnityEngine;

/// <summary>
/// Controla el comportamiento de las armas a distancia.
/// Busca enemigos en alcance y dispara proyectiles automáticamente.
/// Se inicializa con ArmaInstancia para recibir stats escalados por nivel.
/// </summary>
public class ArmasDistancia : MonoBehaviour
{
    [Header("Configuración del Arma")]
    public GameObject balaPrefab;
    public float velocidadBala = 10f;
    public float alcance = 5f;
    public LayerMask capaEnemigos;
    public LayerMask capaCajas;
    public AudioClip sonidoDisparo;

    [Header("Estadísticas Base (se sobreescriben con Inicializar)")]
    public int danioBase = 1;
    public float recargaBase = 1f;
    public int probabilidadCritico = 0;
    public int probabilidadRobarVida = 0;

    [HideInInspector] public int danio = 0;
    [HideInInspector] public float recarga = 0f;

    private AudioSource audioSource;
    private Collider2D colliderJugador;
    private SpriteRenderer spriteRenderer;
    private float tiempoSiguienteDisparo = 0f;
    private bool esCritico = false;
    private int danioCritico;

    /// <summary>
    /// Inicializa el arma con datos de una ArmaInstancia (nivel, stats escalados).
    /// Debe llamarse justo después de Instantiate(), antes de Start().
    /// </summary>
    public void Inicializar(ArmaInstancia instancia)
    {
        danioBase = instancia.Danio;
        recargaBase = instancia.Recarga;
        probabilidadCritico = instancia.Critico;
        probabilidadRobarVida = instancia.RoboVida;

        // Color de nivel en el sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = NivelArma.ObtenerColor(instancia.nivel);
        }
    }

    private void Start()
    {
        danio = danioBase;
        recarga = recargaBase;

        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (colliderJugador == null)
        {
            colliderJugador = GameObject.FindWithTag("Jugador")?.GetComponent<Collider2D>();
        }

        AplicarMejorasGlobales();
    }

    private void AplicarMejorasGlobales()
    {
        GestorMejorasArmas gestorMejoras = GestorMejorasArmas.instancia;
        if (gestorMejoras != null)
        {
            AumentarDanioPorPocentaje(gestorMejoras.ObtenerAumentoDanioPorcentaje());
            AumentarProbabilidadCritico(gestorMejoras.ObtenerAumentoProbabilidadCritico());
            AumentarProbabilidadRobarVida(gestorMejoras.ObtenerAumentoProbabilidadRobarVida());
            DisminuirRecargaPorPocentaje(gestorMejoras.ObtenerDisminucionRecargaPorcentaje());
        }
    }

    private void Update()
    {
        if (Time.time >= tiempoSiguienteDisparo)
        {
            Disparar();
        }
    }

    private void Disparar()
    {
        Collider2D[] objetivos = Physics2D.OverlapCircleAll(transform.position, alcance, capaEnemigos | capaCajas);
        if (objetivos.Length > 0)
        {
            Transform objetivo = objetivos[0].transform;

            if (audioSource != null && sonidoDisparo != null)
            {
                audioSource.ReproducirConVolumenGlobal(sonidoDisparo, 1.0f, TipoAudio.Efectos);
            }

            GameObject bala = Instantiate(balaPrefab, ObtenerPuntoDisparo(), Quaternion.identity);
            Bala scriptBala = bala.GetComponent<Bala>();
            if (scriptBala != null)
            {
                ProbabilidadCritico();
                int danioFinal = esCritico ? danioCritico : danio;
                scriptBala.ConfigurarBala(danioFinal, velocidadBala, capaEnemigos, capaCajas, colliderJugador, objetivo, probabilidadRobarVida);
                esCritico = false;
            }

            tiempoSiguienteDisparo = Time.time + recarga;
        }
    }

    private Vector2 ObtenerPuntoDisparo()
    {
        float offsetY = spriteRenderer != null ? spriteRenderer.bounds.size.y / 2f : 0.5f;
        return new Vector2(transform.position.x, transform.position.y + offsetY);
    }

    private void ProbabilidadCritico()
    {
        int probabilidad = Random.Range(0, 100);
        if (probabilidad < probabilidadCritico)
        {
            danioCritico = danio * 2;
            esCritico = true;
        }
    }

    public void AumentarProbabilidadRobarVida(int cantidad)
    {
        probabilidadRobarVida += cantidad;
    }

    public void AumentarDanioPorPocentaje(int porcentaje)
    {
        float porcentajeDecimal = porcentaje / 100f;
        int aumento = Mathf.RoundToInt(danioBase * porcentajeDecimal);
        danio += aumento;
    }

    public void DisminuirRecargaPorPocentaje(int porcentaje)
    {
        float porcentajeDecimal = porcentaje / 100f;
        float disminucion = recargaBase * porcentajeDecimal;
        recarga -= disminucion;
        if (recarga < 0.1f) recarga = 0.1f;
        recarga = Mathf.Round(recarga * 100f) / 100f;
    }

    public void AumentarProbabilidadCritico(int cantidad)
    {
        probabilidadCritico += cantidad;
    }
}
