using UnityEngine;
using System.Collections;

/// <summary>
/// Controla el comportamiento de las armas cuerpo a cuerpo (melee) en el juego.
/// </summary>
/// <remarks>
/// Esta clase gestiona la detección de enemigos y cajas, los ataques cuerpo a cuerpo,
/// y la aplicación de daño y efectos. Soporta dos tipos de ataques: pinchados (movimiento
/// hacia el objetivo) y normales.
/// </remarks>
public class ArmasMelee : MonoBehaviour
{
    /// <summary>
    /// Daño base del arma que solo debe modificarse desde el inspector.
    /// </summary>
    public int danioBase = 0;
    
    /// <summary>
    /// Daño actual del arma después de aplicar modificadores.
    /// </summary>
    [HideInInspector] public int danio = 0;

    /// <summary>
    /// Tiempo de recarga base entre ataques que solo debe modificarse desde el inspector.
    /// </summary>
    public float recargaBase = 0f;
    
    /// <summary>
    /// Tiempo de recarga actual entre ataques después de aplicar modificadores.
    /// </summary>
    [HideInInspector]public float recarga = 0f;

    /// <summary>
    /// Efecto de sonido que se reproduce al golpear.
    /// </summary>
    public AudioClip sonidoGolpe;
    
    /// <summary>
    /// Probabilidad (en porcentaje) de que un ataque sea crítico y cause daño doble.
    /// </summary>
    public int probabilidadCritico = 0;
    
    /// <summary>
    /// Distancia máxima a la que el arma puede alcanzar objetivos.
    /// </summary>
    public float alcance = 0f;
    
    /// <summary>
    /// Determina si el arma utiliza el tipo de ataque de pinchado (movimiento hacia el objetivo).
    /// </summary>
    public bool pincha;
    
    /// <summary>
    /// Probabilidad (en porcentaje) de recuperar 1 de vida al acertar un golpe.
    /// </summary>
    public int probabilidadRobarVida = 0;
    
    /// <summary>
    /// Capa de colisión que define qué objetos son considerados enemigos.
    /// </summary>
    public LayerMask capaEnemigos;
    
    /// <summary>
    /// Capa de colisión que define qué objetos son considerados cajas destructibles.
    /// </summary>
    public LayerMask capaCajas;

    /// <summary>
    /// Componente AudioSource utilizado para reproducir efectos de sonido.
    /// </summary>
    private AudioSource audioSource;
    
    /// <summary>
    /// Componente Animator para controlar las animaciones del arma.
    /// </summary>
    private Animator animador;
    
    /// <summary>
    /// Marca de tiempo para el próximo ataque disponible.
    /// </summary>
    private float tiempoSiguienteAtaque = 0f;
    
    /// <summary>
    /// Indica si el arma está actualmente ejecutando un ataque.
    /// </summary>
    private bool atacando = false; 
    
    /// <summary>
    /// Posición a la que debe volver el arma después de un ataque de tipo pinchado.
    /// </summary>
    private Vector3 posicionInicial;
    
    /// <summary>
    /// Referencia al componente de vida del jugador para la funcionalidad de robar vida.
    /// </summary>
    private VidaJugador vidaJugador;
    
    /// <summary>
    /// Referencia al objeto del jugador.
    /// </summary>
    private GameObject jugador;
    
    /// <summary>
    /// Indica si el próximo ataque será crítico.
    /// </summary>
    private bool esCritico = false;
    
    /// <summary>
    /// Valor de daño cuando un ataque es crítico.
    /// </summary>
    private int danioCritico;

    /// <summary>
    /// Inicializa los componentes necesarios y aplica las mejoras persistentes al inicio.
    /// </summary>
    private void Start()
    {
        // Reiniciar los valores a los valores base
        danio = danioBase;
        recarga = recargaBase;

        // Obtener o crear AudioSource
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // Aplicar mejoras persistentes
        GestorMejorasArmas gestorMejoras = GestorMejorasArmas.instancia;
        if (gestorMejoras != null)
        {
            AumentarDanioPorPocentaje(gestorMejoras.ObtenerAumentoDanioPorcentaje());
            AumentarProbabilidadCritico(gestorMejoras.ObtenerAumentoProbabilidadCritico());
            AumentarProbabilidadRobarVida(gestorMejoras.ObtenerAumentoProbabilidadRobarVida());
            DisminuirRecargaPorPocentaje(gestorMejoras.ObtenerDisminucionRecargaPorcentaje());
            
            Debug.Log("Arma Melee inicializada: Daño base = " + danioBase + ", Daño final = " + danio);
        }

        ObtenerJugador();
        if (jugador != null)
        {
            vidaJugador = jugador.GetComponent<VidaJugador>();
        }

        animador = GetComponent<Animator>();
        posicionInicial = transform.position; // Guardar la posición inicial del arma
    }

    /// <summary>
    /// Controla la detección de objetivos y ataques automáticos.
    /// </summary>
    private void Update()
    {   
        if (jugador == null) 
        {
            ObtenerJugador();
        }

        if (!atacando && Time.time >= tiempoSiguienteAtaque)
        {
            DetectarEnemigos();
            DetectarCajas();
        }
    }

    /// <summary>
    /// Detecta enemigos dentro del alcance del arma e inicia un ataque contra el más cercano.
    /// </summary>
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

            // Obtener posición inicial antes del primer ataque
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
                if (animador != null)
                {
                    animador.SetTrigger("Atacar");
                }
                Invoke("AplicarDaño", 0.1f);
            }

            tiempoSiguienteAtaque = Time.time + recarga;
        }
    }

    /// <summary>
    /// Detecta cajas destructibles dentro del alcance del arma e inicia un ataque contra ellas.
    /// </summary>
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
                    if (animador != null)
                    {
                        animador.SetTrigger("Atacar");
                    }
                    cajaScript.RecibirGolpe();
                }
            }
        }
    }

    /// <summary>
    /// Ejecuta un ataque de tipo pinchado hacia un enemigo.
    /// </summary>
    /// <param name="posicionEnemigo">Posición del enemigo objetivo.</param>
    /// <returns>Corrutina que gestiona el movimiento del arma hacia el enemigo y su regreso.</returns>
    private IEnumerator AtaquePinchado(Vector3 posicionEnemigo)
    {
        // Obtener la referencia del script de giro del arma
        GirarArmaHaciaEnemigo girarArma = GetComponent<GirarArmaHaciaEnemigo>();

        // Bloquear la rotación del arma mientras ataca
        if (girarArma != null)
        {
            girarArma.SetAtacando(true);
        }

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

        // Actualizar la posición inicial antes de volver
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

        // Permitir que el arma vuelva a girar después del ataque
        if (girarArma != null)
        {
            girarArma.SetAtacando(false);
        }
    }

    /// <summary>
    /// Ejecuta un ataque de tipo pinchado hacia una caja destructible.
    /// </summary>
    /// <param name="posicionCaja">Posición de la caja objetivo.</param>
    /// <param name="cajaScript">Referencia al script de la caja para aplicar daño.</param>
    /// <returns>Corrutina que gestiona el movimiento del arma hacia la caja y su regreso.</returns>
    private IEnumerator AtaquePinchadoCaja(Vector3 posicionCaja, RomperCaja cajaScript)
    {
        GirarArmaHaciaEnemigo girarArma = GetComponent<GirarArmaHaciaEnemigo>();

        if (girarArma != null)
        {
            girarArma.SetAtacando(true);
        }

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

        if (cajaScript != null)
        {
            cajaScript.RecibirGolpe();
        }

        tiempo = 0f;
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            transform.position = Vector3.Lerp(posicionObjetivo, posicionInicial, tiempo / duracion);
            yield return null;
        }

        atacando = false;

        if (girarArma != null)
        {
            girarArma.SetAtacando(false);
        }
    }

    /// <summary>
    /// Aplica daño a todos los enemigos dentro del alcance del arma.
    /// </summary>
    private void AplicarDaño()
    {
        Collider2D[] enemigosGolpeados = Physics2D.OverlapCircleAll(transform.position, alcance, capaEnemigos);

        // Reproducir sonido de golpe
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
                if (esCritico)
                {
                    vidaEnemigo.RecibirDaño(danioCritico);
                    Debug.Log("Daño crítico: " + danioCritico);
                }
                else 
                {
                    vidaEnemigo.RecibirDaño(danio);
                    Debug.Log("Daño normal: " + danio);
                }
                RobarVida();
            }
        }
        
        esCritico = false;
    }

    /// <summary>
    /// Intenta recuperar vida para el jugador basado en la probabilidad de robar vida.
    /// </summary>
    private void RobarVida() 
    {
        // Genera un número aleatorio entre 0 y 100
        int probabilidad = Random.Range(0, 100);
        
        // Si el número aleatorio es menor que la probabilidad de robar vida, se cura
        if (probabilidad < probabilidadRobarVida && vidaJugador != null)
        {
            vidaJugador.Curar(1); // Recupera 1 de vida
        }    
    }

    /// <summary>
    /// Determina si el próximo ataque será crítico basándose en la probabilidad de crítico.
    /// </summary>
    private void ProbabilidadCritico() 
    {
        int probabilidad = Random.Range(0, 100);
        if (probabilidad < probabilidadCritico)
        {
            danioCritico = danio * 2;
            esCritico = true;
        }   
    }

    /// <summary>
    /// Obtiene una referencia al objeto del jugador y su componente de vida.
    /// </summary>
    private void ObtenerJugador()
    {
        // Intentar obtener el jugador con el tag 'Jugador'
        jugador = GameObject.FindWithTag("Jugador");

        if (jugador != null)
        {
            vidaJugador = jugador.GetComponent<VidaJugador>();
        }
    }

    /// <summary>
    /// Aumenta la probabilidad de robar vida al acertar un golpe.
    /// </summary>
    /// <param name="cantidad">Cantidad a aumentar en puntos porcentuales.</param>
    public void AumentarProbabilidadRobarVida(int cantidad) 
    {
        probabilidadRobarVida += cantidad;
    }

    /// <summary>
    /// Aumenta el daño del arma en un porcentaje dado.
    /// </summary>
    /// <param name="porcentaje">Porcentaje de aumento del daño.</param>
    public void AumentarDanioPorPocentaje(int porcentaje)
    {
        float porcentajeDecimal = porcentaje / 100f;
        int aumento = Mathf.RoundToInt(danioBase * porcentajeDecimal);
        danio += aumento;
    }

    /// <summary>
    /// Disminuye el tiempo de recarga del arma en un porcentaje dado.
    /// </summary>
    /// <param name="porcentaje">Porcentaje de disminución del tiempo de recarga.</param>
    /// <remarks>
    /// El tiempo de recarga nunca será menor a 0.1 segundos.
    /// </remarks>
    public void DisminuirRecargaPorPocentaje(int porcentaje)
    {
        float porcentajeDecimal = porcentaje / 100f;
        float disminucion = recargaBase * porcentajeDecimal;
        recarga -= disminucion;
        
        // Asegurar un valor mínimo de recarga
        if (recarga < 0.1f) recarga = 0.1f;
        
        // Redondear a 2 decimales para mayor precisión
        recarga = Mathf.Round(recarga * 100f) / 100f;
        
        Debug.Log("Arma Melee: Recarga base = " + recargaBase + ", Porcentaje = " + porcentaje + "%, Disminución = " + disminucion + ", Recarga final = " + recarga);
    }

    /// <summary>
    /// Aumenta la probabilidad de golpe crítico.
    /// </summary>
    /// <param name="cantidad">Cantidad a aumentar en puntos porcentuales.</param>
    public void AumentarProbabilidadCritico(int cantidad)
    {
       probabilidadCritico = probabilidadCritico + cantidad;     
    }
}
