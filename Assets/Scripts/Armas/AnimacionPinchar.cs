using UnityEngine;
using System.Collections;

public class AnimacionPinchar : MonoBehaviour
{
    public float rangoAtaque = 2f;      // Distancia para detectar enemigos
    public float distanciaAtaque = 1f;  // Distancia que se moverá hacia el enemigo
    public float tiempoMovimiento = 0.2f; // Tiempo que tarda en moverse y regresar
    public LayerMask capaEnemigos;      // Capa de los enemigos
    public bool atacando = false;       // Controla si el arma está atacando

    private ArmasMelee armasMelee;
    private Vector3 posicionInicial;
    private bool puedeAtacar = true; // Controla si se puede buscar un enemigo

    private void Start()
    {
        armasMelee = GetComponent<ArmasMelee>();
        posicionInicial = transform.localPosition;
    }

    private void Update()
    {
        if (!atacando && puedeAtacar)
        {
            Transform enemigoCercano = BuscarEnemigoMasCercano();
            if (enemigoCercano != null)
            {
                StartCoroutine(Pinchar(enemigoCercano));
            }
        }
    }

    private Transform BuscarEnemigoMasCercano()
    {
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(transform.position, rangoAtaque, capaEnemigos);

        if (enemigos.Length == 0) return null;

        Transform enemigoMasCercano = null;
        float distanciaMinima = Mathf.Infinity;

        foreach (Collider2D enemigo in enemigos)
        {
            float distancia = Vector2.Distance(transform.position, enemigo.transform.position);
            if (distancia < distanciaMinima)
            {
                distanciaMinima = distancia;
                enemigoMasCercano = enemigo.transform;
            }
        }

        return enemigoMasCercano;
    }

    private IEnumerator Pinchar(Transform enemigoCercano)
    {
        atacando = true;
        puedeAtacar = false; // Deshabilitar búsqueda de enemigos mientras ataca

        Vector3 direccion = (enemigoCercano.position - transform.position).normalized; 
        Vector3 posicionObjetivo = transform.position + (direccion * distanciaAtaque);

        // Mover hacia el enemigo
        float tiempo = 0;
        Vector3 posicionInicial = transform.position;
        while (tiempo < tiempoMovimiento)
        {
            transform.position = Vector3.Lerp(posicionInicial, posicionObjetivo, tiempo / tiempoMovimiento);
            tiempo += Time.deltaTime;
            yield return null;
        }
        transform.position = posicionObjetivo;

        // Regresar a la posición original
        tiempo = 0;
        while (tiempo < tiempoMovimiento)
        {
            transform.position = Vector3.Lerp(posicionObjetivo, posicionInicial, tiempo / tiempoMovimiento);
            tiempo += Time.deltaTime;
            yield return null;
        }
        transform.position = posicionInicial;

        atacando = false;

        puedeAtacar = true; // Ahora puede buscar enemigos otra vez
    }
}
