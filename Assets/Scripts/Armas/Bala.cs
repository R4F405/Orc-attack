using UnityEngine;

/// <summary>
/// Controla el comportamiento de los proyectiles disparados por armas a distancia.
/// </summary>
/// <remarks>
/// Este script gestiona el movimiento del proyectil hacia un objetivo,
/// la detección de colisiones con enemigos y cajas, y la aplicación de daño.
/// También incluye funcionalidad para robar vida al acertar impactos.
/// </remarks>
public class Bala : MonoBehaviour
{
    /// <summary>
    /// Capa de colisión que define qué objetos son considerados enemigos.
    /// </summary>
    private LayerMask capaEnemigos;
    
    /// <summary>
    /// Capa de colisión que define qué objetos son considerados cajas destructibles.
    /// </summary>
    private LayerMask capaCajas;
    
    /// <summary>
    /// Cantidad de daño que inflige este proyectil al impactar.
    /// </summary>
    private int danio;
    
    /// <summary>
    /// Referencia al objetivo hacia el que se moverá el proyectil.
    /// </summary>
    private Transform objetivo;
    
    /// <summary>
    /// Componente Rigidbody2D utilizado para mover el proyectil.
    /// </summary>
    private Rigidbody2D rb;
    
    /// <summary>
    /// Velocidad a la que se mueve el proyectil.
    /// </summary>
    private float velocidad;
    
    /// <summary>
    /// Referencia al componente de vida del jugador para la funcionalidad de robar vida.
    /// </summary>
    private VidaJugador vidaJugador;
    
    /// <summary>
    /// Referencia al objeto del jugador.
    /// </summary>
    private GameObject jugador;
    
    /// <summary>
    /// Probabilidad (en porcentaje) de recuperar 1 de vida al acertar un impacto.
    /// </summary>
    private int probabilidadRobarVida = 5;
    
    /// <summary>
    /// Inicializa los componentes necesarios y configura el comportamiento inicial del proyectil.
    /// </summary>
    private void Start()
    {
        ObtenerJugador();
        if (jugador != null)
        {
            vidaJugador = jugador.GetComponent<VidaJugador>();
        }

        rb = GetComponent<Rigidbody2D>();
        
        // Destruir la bala después de x segundos si no impacta con nada
        Destroy(gameObject, 3f);

        // Ignorar colisión entre balas
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Balas"), LayerMask.NameToLayer("Balas"));
    }

    /// <summary>
    /// Configura las propiedades del proyectil.
    /// </summary>
    /// <param name="nuevoDaño">Cantidad de daño que infligirá el proyectil.</param>
    /// <param name="nuevaVelocidad">Velocidad a la que se moverá el proyectil.</param>
    /// <param name="nuevaCapaEnemigos">Capa de colisión de los enemigos.</param>
    /// <param name="nuevaCapaCajas">Capa de colisión de las cajas destructibles.</param>
    /// <param name="colliderJugador">Collider del jugador para evitar colisiones con él.</param>
    /// <param name="enemigo">Objetivo hacia el que se moverá el proyectil.</param>
    /// <param name="nuevaProbabilidad">Probabilidad de robar vida al acertar un impacto.</param>
    public void ConfigurarBala(int nuevoDaño, float nuevaVelocidad, LayerMask nuevaCapaEnemigos, LayerMask nuevaCapaCajas, Collider2D colliderJugador, Transform enemigo, int nuevaProbabilidad)
    {
        danio = nuevoDaño;
        velocidad = nuevaVelocidad;
        capaEnemigos = nuevaCapaEnemigos;
        capaCajas = nuevaCapaCajas;
        objetivo = enemigo;
        probabilidadRobarVida = nuevaProbabilidad;

        // Ignorar colisión con el jugador
        Collider2D miCollider = GetComponent<Collider2D>();
        if (miCollider != null && colliderJugador != null)
        {
            Physics2D.IgnoreCollision(miCollider, colliderJugador);
        }
    }

    /// <summary>
    /// Actualiza el movimiento del proyectil hacia su objetivo en cada frame.
    /// </summary>
    private void Update()
    {
        if (objetivo != null)
        {
            // Calcular la dirección hacia el enemigo
            Vector2 direccion = ((Vector2)objetivo.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = direccion * velocidad;
        }
    }

    /// <summary>
    /// Detecta y maneja las colisiones con enemigos y cajas destructibles.
    /// </summary>
    /// <param name="otro">El collider con el que ha colisionado el proyectil.</param>
    private void OnTriggerEnter2D(Collider2D otro)
    {
        int capaOtro = 1 << otro.gameObject.layer;

        if ((capaOtro & capaEnemigos) != 0)
        {
            VidaEnemigo salud = otro.GetComponent<VidaEnemigo>();
            if (salud != null)
            {
                salud.RecibirDaño(danio);
                RobarVida();
            }
            Destroy(gameObject); // Destruir la bala al impactar con un enemigo
        }
        else if ((capaOtro & capaCajas) != 0)
        {
            RomperCaja caja = otro.GetComponent<RomperCaja>();
            if (caja != null)
            {
                caja.RecibirGolpe();
            }
            Destroy(gameObject); // La bala también desaparece al impactar la caja
        }
    }

    /// <summary>
    /// Intenta recuperar vida para el jugador basado en la probabilidad de robar vida.
    /// </summary>
    /// <remarks>
    /// Si el número aleatorio generado es menor que la probabilidad de robar vida,
    /// el jugador recupera 1 punto de vida.
    /// </remarks>
    private void RobarVida() 
    {
        // Genera un número aleatorio entre 0 y 100
        int probabilidad = Random.Range(0, 100);
        // Si el número aleatorio es menor que la probabilidad de robar vida, se cura
        if (probabilidad < probabilidadRobarVida)
        {
            if (vidaJugador != null)
            {
                vidaJugador.Curar(1); // Recupera 1 de vida
            }
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
}
