using UnityEngine;

public class SistemaDanioDistanciaEnemigo : MonoBehaviour
{
    public GameObject proyectilPrefab;
    public float tiempoEntreDisparos = 2f;
    public int daño = 1; // Daño del proyectil, ajustable desde el inspector

    private float proximoDisparo = 0f;
    private GameObject jugador;
    private Vector3 puntoDisparo; // Punto de disparo centrado

    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Jugador");
    }

    private void Update()
    {
        puntoDisparo = GetComponent<Collider2D>().bounds.center; // Calcula el punto de disparo centrado en el enemigo

        if (jugador != null && Time.time >= proximoDisparo)
        {
            Disparar();
            proximoDisparo = Time.time + tiempoEntreDisparos;
        }
    }

    private void Disparar()
    {
        // Ajusta la posición del punto de disparo para que esté un poco más arriba
        Vector3 puntoDisparoAjustado = new Vector3(puntoDisparo.x, puntoDisparo.y, puntoDisparo.z);

        GameObject proyectil = Instantiate(proyectilPrefab, puntoDisparoAjustado, Quaternion.identity);
        Vector2 direccion = (jugador.transform.position - transform.position).normalized;
        
        ConfiguradorProyectilEnemigo scriptProyectil = proyectil.GetComponent<ConfiguradorProyectilEnemigo>();
        if (scriptProyectil != null)
        {
            scriptProyectil.Configurar(direccion, daño); // Pasar la dirección y el daño
        }
    }
}
