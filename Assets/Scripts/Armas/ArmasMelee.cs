using UnityEngine;
using System.Collections;

/// <summary>
/// Controla el comportamiento de las armas cuerpo a cuerpo.
/// Detecta enemigos y cajas en alcance y ataca automáticamente.
/// Se inicializa con ArmaInstancia para recibir stats escalados por nivel.
/// </summary>
public class ArmasMelee : MonoBehaviour
{
    [Header("Configuración del Arma")]
    public bool pincha;
    public float alcance = 2f;
    public LayerMask capaEnemigos;
    public LayerMask capaCajas;
    public AudioClip sonidoGolpe;

    [Header("Estadísticas Base (se sobreescriben con Inicializar)")]
    public int danioBase = 1;
    public float recargaBase = 1f;
    public int probabilidadCritico = 0;
    public int probabilidadRobarVida = 0;

    [HideInInspector] public int danio = 0;
    [HideInInspector] public float recarga = 0f;

    private AudioSource audioSource;
    private Animator animador;
    private float tiempoSiguienteAtaque = 0f;
    private bool atacando = false;
    private Vector3 posicionInicial;
    private VidaJugador vidaJugador;
    private GameObject jugador;
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

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        AplicarMejorasGlobales();

        ObtenerJugador();
        if (jugador != null)
        {
            vidaJugador = jugador.GetComponent<VidaJugador>();
        }

        animador = GetComponent<Animator>();
        posicionInicial = transform.position;
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
        if (jugador == null) ObtenerJugador();

        if (!atacando && Time.time >= tiempoSiguienteAtaque)
        {
            DetectarEnemigos();
            DetectarCajas();
        }
    }

    private void DetectarEnemigos()
    {
        Collider2D[] enemigosEnRango = Physics2D.OverlapCircleAll(transform.position, alcance, capaEnemigos);

        if (enemigosEnRango.Length > 0)
        {
            atacando = true;

            Transform enemigoMasCercano = enemigosEnRango[0].transform;
            float distanciaMinima = Vector2.Distance(transform.position, enemigoMasCercano.position);

            foreach (Collider2D enemigo in enemigosEnRango)
            {
                float distancia = Vector2.Distance(transform.position, enemigo.transform.position);
                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    enemigoMasCercano = enemigo.transform;
                }
            }

            PosicionarArmasJugador posicionador = FindFirstObjectByType<PosicionarArmasJugador>();
            if (posicionador != null)
            {
                posicionInicial = posicionador.ObtenerPosicionActualDelArma(gameObject);
            }

            if (pincha)
            {
                StartCoroutine(AtaquePinchado(enemigoMasCercano.position));
            }
            else
            {
                atacando = false;
                if (animador != null) animador.SetTrigger("Atacar");
                Invoke("AplicarDaño", 0.1f);
            }

            tiempoSiguienteAtaque = Time.time + recarga;
        }
    }

    private void DetectarCajas()
    {
        Collider2D[] cajasEnRango = Physics2D.OverlapCircleAll(transform.position, alcance, capaCajas);

        foreach (Collider2D caja in cajasEnRango)
        {
            RomperCaja cajaScript = caja.GetComponent<RomperCaja>();
            if (cajaScript != null)
            {
                if (pincha)
                {
                    StartCoroutine(AtaquePinchadoCaja(caja.transform.position, cajaScript));
                }
                else
                {
                    if (animador != null) animador.SetTrigger("Atacar");
                    cajaScript.RecibirGolpe();
                }
            }
        }
    }

    private IEnumerator AtaquePinchado(Vector3 posicionEnemigo)
    {
        GirarArmaHaciaEnemigo girarArma = GetComponent<GirarArmaHaciaEnemigo>();
        if (girarArma != null) girarArma.SetAtacando(true);

        Vector3 posicionObjetivo = (posicionEnemigo - transform.position).normalized * alcance + transform.position;
        float duracion = 0.15f;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            transform.position = Vector3.Lerp(posicionInicial, posicionObjetivo, tiempo / duracion);
            yield return null;
        }

        AplicarDaño();

        PosicionarArmasJugador posicionador = FindAnyObjectByType<PosicionarArmasJugador>();
        if (posicionador != null)
        {
            posicionInicial = posicionador.ObtenerPosicionActualDelArma(gameObject);
        }

        tiempo = 0f;
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            transform.position = Vector3.Lerp(posicionObjetivo, posicionInicial, tiempo / duracion);
            yield return null;
        }

        atacando = false;
        if (girarArma != null) girarArma.SetAtacando(false);
    }

    private IEnumerator AtaquePinchadoCaja(Vector3 posicionCaja, RomperCaja cajaScript)
    {
        GirarArmaHaciaEnemigo girarArma = GetComponent<GirarArmaHaciaEnemigo>();
        if (girarArma != null) girarArma.SetAtacando(true);

        Vector3 posicionObjetivo = (posicionCaja - transform.position).normalized * alcance + transform.position;
        float duracion = 0.15f;
        float tiempo = 0f;

        PosicionarArmasJugador posicionador = FindAnyObjectByType<PosicionarArmasJugador>();
        if (posicionador != null)
        {
            posicionInicial = posicionador.ObtenerPosicionActualDelArma(gameObject);
        }

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            transform.position = Vector3.Lerp(posicionInicial, posicionObjetivo, tiempo / duracion);
            yield return null;
        }

        if (cajaScript != null) cajaScript.RecibirGolpe();

        tiempo = 0f;
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            transform.position = Vector3.Lerp(posicionObjetivo, posicionInicial, tiempo / duracion);
            yield return null;
        }

        atacando = false;
        if (girarArma != null) girarArma.SetAtacando(false);
    }

    private void AplicarDaño()
    {
        Collider2D[] enemigosGolpeados = Physics2D.OverlapCircleAll(transform.position, alcance, capaEnemigos);

        if (audioSource != null && sonidoGolpe != null)
        {
            audioSource.ReproducirConVolumenGlobal(sonidoGolpe, 1.0f, TipoAudio.Efectos);
        }

        ProbabilidadCritico();

        foreach (Collider2D enemigo in enemigosGolpeados)
        {
            VidaEnemigo vidaEnemigo = enemigo.GetComponent<VidaEnemigo>();
            if (vidaEnemigo != null)
            {
                vidaEnemigo.RecibirDaño(esCritico ? danioCritico : danio);
                RobarVida();
            }
        }

        esCritico = false;
    }

    private void RobarVida()
    {
        int probabilidad = Random.Range(0, 100);
        if (probabilidad < probabilidadRobarVida && vidaJugador != null)
        {
            vidaJugador.Curar(1);
        }
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

    private void ObtenerJugador()
    {
        jugador = GameObject.FindWithTag("Jugador");
        if (jugador != null)
        {
            vidaJugador = jugador.GetComponent<VidaJugador>();
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
