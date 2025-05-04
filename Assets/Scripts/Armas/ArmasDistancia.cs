using UnityEngine;

/// <summary>
/// Controla el comportamiento de las armas a distancia en el juego.
/// </summary>
/// <remarks>
/// Esta clase gestiona la generación de proyectiles, la detección de enemigos,
/// el cálculo de daño y la aplicación de mejoras a las armas a distancia.
/// Las armas a distancia buscan automáticamente objetivos dentro de su alcance
/// y disparan proyectiles hacia ellos.
/// </remarks>
public class ArmasDistancia : MonoBehaviour
{
    /// <summary>
    /// Prefab del proyectil que será instanciado al disparar.
    /// </summary>
    public GameObject balaPrefab;

    /// <summary>
    /// Daño base del arma que solo se debe modificar desde el inspector.
    /// </summary>
    public int danioBase = 0;
    
    /// <summary>
    /// Daño actual del arma después de aplicar modificadores.
    /// </summary>
    [HideInInspector] public int danio = 0;

    /// <summary>
    /// Tiempo de recarga base entre disparos que solo se debe modificar desde el inspector.
    /// </summary>
    public float recargaBase = 0f;
    
    /// <summary>
    /// Tiempo de recarga actual entre disparos después de aplicar modificadores.
    /// </summary>
    [HideInInspector]public float recarga = 0f;

    /// <summary>
    /// Probabilidad (en porcentaje) de que el disparo sea crítico y cause daño doble.
    /// </summary>
    public int probabilidadCritico = 0;
    
    /// <summary>
    /// Velocidad a la que se moverá el proyectil después de ser disparado.
    /// </summary>
    public float velocidadBala = 0f;
    
    /// <summary>
    /// Distancia máxima a la que el arma puede detectar enemigos.
    /// </summary>
    public float alcance = 0f;
    
    /// <summary>
    /// Probabilidad (en porcentaje) de robar 1 de vida al impactar a un enemigo.
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
    /// Efecto de sonido que se reproduce al disparar.
    /// </summary>
    public AudioClip sonidoDisparo;

    /// <summary>
    /// Componente AudioSource utilizado para reproducir efectos de sonido.
    /// </summary>
    private AudioSource audioSource;
    
    /// <summary>
    /// Referencia al collider del jugador para evitar colisiones con los proyectiles propios.
    /// </summary>
    private Collider2D colliderJugador;
    
    /// <summary>
    /// Marca de tiempo para el próximo disparo disponible.
    /// </summary>
    private float tiempoSiguienteDisparo = 0f;
    
    /// <summary>
    /// Indica si el próximo disparo será crítico.
    /// </summary>
    private bool esCritico = false;
    
    /// <summary>
    /// Valor de daño cuando un disparo es crítico.
    /// </summary>
    private int danioCritico;

    /// <summary>
    /// Inicializa los componentes y aplica las mejoras persistentes al inicio.
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
            
            Debug.Log("Arma Distancia inicializada: Daño base = " + danioBase + ", Daño final = " + danio);
        }

        if (colliderJugador == null)
        {
            colliderJugador = GameObject.FindWithTag("Jugador")?.GetComponent<Collider2D>();
        }
    }

    /// <summary>
    /// Controla el disparo automático basado en el tiempo de recarga.
    /// </summary>
    private void Update()
    {
        if (Time.time >= tiempoSiguienteDisparo)
        {
            Disparar();
        }
    }

    /// <summary>
    /// Busca objetivos dentro del alcance y dispara un proyectil hacia el primer objetivo encontrado.
    /// </summary>
    private void Disparar()
    {
        Collider2D[] objetivos = Physics2D.OverlapCircleAll(transform.position, alcance, capaEnemigos | capaCajas);
        if (objetivos.Length > 0)
        {
            Transform objetivo = objetivos[0].transform;

            // Reproducir sonido de disparo
            if (audioSource != null && sonidoDisparo != null)
            {
                audioSource.ReproducirConVolumenGlobal(sonidoDisparo, 1.0f, TipoAudio.Efectos);
            }

            GameObject bala = Instantiate(balaPrefab, ObtenerPuntoDisparo(), Quaternion.identity);
            Bala scriptBala = bala.GetComponent<Bala>();
            if (scriptBala != null)
            {
                ProbabilidadCritico();
                if (esCritico) 
                {
                    scriptBala.ConfigurarBala(danioCritico, velocidadBala, capaEnemigos, capaCajas, colliderJugador, objetivo, probabilidadRobarVida);
                }
                else 
                {
                    scriptBala.ConfigurarBala(danio, velocidadBala, capaEnemigos, capaCajas, colliderJugador, objetivo, probabilidadRobarVida);
                }
                esCritico = false;
            }

            tiempoSiguienteDisparo = Time.time + recarga;
        }
    }

    /// <summary>
    /// Calcula el punto desde donde se origina el disparo.
    /// </summary>
    /// <returns>Vector2 con la posición desde donde se debe instanciar el proyectil.</returns>
    private Vector2 ObtenerPuntoDisparo()
    {
        return new Vector2(transform.position.x, transform.position.y + (GetComponent<SpriteRenderer>().bounds.size.y / 2));
    }

    /// <summary>
    /// Determina si el próximo disparo será crítico basándose en la probabilidad de crítico.
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
    /// Aumenta la probabilidad de robar vida.
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
    /// <remarks>
    /// El aumento se calcula multiplicando el daño actual por el porcentaje
    /// dividido entre 100, y luego añadiendo ese valor al daño actual.
    /// </remarks>
    public void AumentarDanioPorPocentaje(int porcentaje)
    {
        float porcentajeDecimal = porcentaje / 100f;
        int aumento = Mathf.RoundToInt(danioBase * porcentajeDecimal);
        danio += aumento;
        
        Debug.Log("Arma Distancia: Daño base = " + danioBase + ", Porcentaje = " + porcentaje + "%, Aumento = " + aumento + ", Daño final = " + danio);
    }

    /// <summary>
    /// Disminuye el tiempo de recarga del arma en un porcentaje dado.
    /// </summary>
    /// <param name="porcentaje">Porcentaje de disminución del tiempo de recarga.</param>
    /// <remarks>
    /// La disminución se calcula multiplicando el tiempo de recarga base por el porcentaje
    /// dividido entre 100, y luego restando ese valor del tiempo de recarga actual.
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
        
        Debug.Log("Arma Distancia: Recarga base = " + recargaBase + ", Porcentaje = " + porcentaje + "%, Disminución = " + disminucion + ", Recarga final = " + recarga);
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
