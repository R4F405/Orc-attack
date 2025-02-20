using UnityEngine;

public class ArmasDistancia : MonoBehaviour
{
    public GameObject balaPrefab; // Prefab de la bala
    public int danio = 10; // Daño de la bala
    public float velocidadBala = 10f; // Velocidad de la bala
    public float recarga = 1f; // Tiempo de recarga entre disparos
    public LayerMask capaEnemigos; // Capa de los enemigos
    public float alcance = 2f;

    private Collider2D colliderJugador; // Referencia al collider del jugador
    private float tiempoSiguienteDisparo = 0f;

    private void Start()
{
    if (colliderJugador == null)
    {
        colliderJugador = GameObject.FindWithTag("Jugador")?.GetComponent<Collider2D>();
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
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(transform.position, alcance, capaEnemigos);
        if (enemigos.Length > 0)
        {
            Transform objetivo = enemigos[0].transform;

            GameObject bala = Instantiate(balaPrefab, ObtenerPuntoDisparo(), Quaternion.identity);
            Bala scriptBala = bala.GetComponent<Bala>();
            if (scriptBala != null)
            {
                scriptBala.ConfigurarBala(danio, velocidadBala, capaEnemigos, colliderJugador, objetivo);
            }

            tiempoSiguienteDisparo = Time.time + recarga;
        }
    }



    private Vector2 ObtenerPuntoDisparo()
    {
        return new Vector2(transform.position.x, transform.position.y + (GetComponent<SpriteRenderer>().bounds.size.y / 2));
    }
}
