using UnityEngine;
using System.Collections;

public class ArmasMelee : MonoBehaviour
{
    public int danio = 10; 
    public float alcance = 2f; 
    public float recarga = 1f; 
    public LayerMask capaEnemigos; 
    public bool pincha; // Determina si el arma pincha o no

    private Animator animador;
    private float tiempoSiguienteAtaque = 0f;  
    private bool atacando = false; 
    private Vector3 posicionInicial;

    private void Start()
    {
        animador = GetComponent<Animator>(); 
        posicionInicial = transform.position; // Guardar la posición inicial del arma
    }

    private void Update()
    {
        if (!atacando && Time.time >= tiempoSiguienteAtaque)
        {
            DetectarEnemigos();
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

            // **Obtener posición inicial antes del primer ataque**
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
                if (animador != null)
                {
                    animador.SetTrigger("Atacar");
                }
                Invoke("AplicarDaño", 0.1f);
            }

            tiempoSiguienteAtaque = Time.time + recarga;
        }
    }

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

    private void AplicarDaño()
    {
        Collider2D[] enemigosGolpeados = Physics2D.OverlapCircleAll(transform.position, alcance, capaEnemigos);

        foreach (Collider2D enemigo in enemigosGolpeados)
        {
            VidaEnemigo salud = enemigo.GetComponent<VidaEnemigo>();
            if (salud != null)
            {
                salud.RecibirDaño(danio);
                atacando = false;
            }
        }
    }
}
