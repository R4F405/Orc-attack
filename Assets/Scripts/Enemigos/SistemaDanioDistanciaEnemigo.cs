using UnityEngine;

/// <summary>
/// Controla el sistema de ataque a distancia de un enemigo mediante proyectiles.
/// </summary>
/// <remarks>
/// Esta clase permite que un enemigo dispare proyectiles hacia el jugador
/// a intervalos regulares. Gestiona la creación, dirección y configuración
/// de los proyectiles lanzados.
/// </remarks>
public class SistemaDanioDistanciaEnemigo : MonoBehaviour
{
    /// <summary>
    /// Prefab del proyectil que será instanciado al disparar.
    /// </summary>
    public GameObject proyectilPrefab;
    
    /// <summary>
    /// Tiempo en segundos entre disparos consecutivos.
    /// </summary>
    public float tiempoEntreDisparos = 2f;
    
    /// <summary>
    /// Cantidad de daño que cada proyectil inflige al jugador al impactar.
    /// </summary>
    public int daño = 1;

    /// <summary>
    /// Marca de tiempo para el próximo disparo disponible.
    /// </summary>
    private float proximoDisparo = 0f;
    
    /// <summary>
    /// Referencia al objeto del jugador para calcular la dirección del disparo.
    /// </summary>
    private GameObject jugador;
    
    /// <summary>
    /// Posición desde la que se origina el disparo.
    /// </summary>
    private Vector3 puntoDisparo;

    /// <summary>
    /// Inicializa el sistema encontrando al jugador en la escena.
    /// </summary>
    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Jugador");
    }

    /// <summary>
    /// Comprueba si es momento de disparar y actualiza el punto de origen del proyectil.
    /// </summary>
    private void Update()
    {
        puntoDisparo = GetComponent<Collider2D>().bounds.center;

        if (jugador != null && Time.time >= proximoDisparo)
        {
            Disparar();
            proximoDisparo = Time.time + tiempoEntreDisparos;
        }
    }

    /// <summary>
    /// Crea y configura un proyectil dirigido hacia el jugador.
    /// </summary>
    /// <remarks>
    /// Instancia un proyectil en la posición del enemigo y lo configura
    /// para que se dirija hacia la posición actual del jugador.
    /// </remarks>
    private void Disparar()
    {
        // Ajusta la posición del punto de disparo para que esté un poco más arriba
        Vector3 puntoDisparoAjustado = new Vector3(puntoDisparo.x, puntoDisparo.y, puntoDisparo.z);

        GameObject proyectil = Instantiate(proyectilPrefab, puntoDisparoAjustado, Quaternion.identity);
        
        // Asegurarse que el proyectil esté en la capa "ProyectilesEnemigos" o crear una si no existe
        proyectil.layer = LayerMask.NameToLayer("ProyectilesEnemigos");
        
        Vector2 direccion = (jugador.transform.position - transform.position).normalized;
        
        ConfiguradorProyectilEnemigo scriptProyectil = proyectil.GetComponent<ConfiguradorProyectilEnemigo>();
        if (scriptProyectil != null)
        {
            scriptProyectil.Configurar(direccion, daño);
        }
    }
}
