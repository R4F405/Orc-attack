using UnityEngine;
using System.Collections;

public class ArmasMelee : MonoBehaviour
{
    public int danioBase = 0; // Daño base del arma (Solo se modifica desde el inspector)
    [HideInInspector] public int danio = 0; //Daño del arma

    public float recargaBase = 0f; // Tiempo de recarga base entre ataques (Solo se modifica desde el inspector)
    [HideInInspector]public float recarga = 0f; //Tiempo de recarga entre ataques

    public int probabilidadCritico = 0; //Probabilidad de critico 
    public float alcance = 0f; //Alcance del arma
    public bool pincha; // Determina si el arma pincha o no
    public int probabilidadRobarVida = 0; //probabilidad en %
    public LayerMask capaEnemigos; //capa de enemgos
    public LayerMask capaCajas; // Capa de las cajas

    private Animator animador;
    private float tiempoSiguienteAtaque = 0f;  
    private bool atacando = false; 
    private Vector3 posicionInicial;
    private VidaJugador vidaJugador;
    private GameObject jugador;
    private bool esCritico = false;
    private int danioCritico;

    private void Start()
    {
        danio = danioBase; // Inicializar el daño con el valor base
        recarga = recargaBase; // Inicializar la recarga con el valor base
        
        ObtenerJugador();
        if (jugador != null)
        {
            vidaJugador = jugador.GetComponent<VidaJugador>();
        }

        animador = GetComponent<Animator>(); 
        posicionInicial = transform.position; // Guardar la posición inicial del arma
    }

    private void Update()
    {   
        if (jugador == null) 
        {
            ObtenerJugador();
        }

        if (!atacando && Time.time >= tiempoSiguienteAtaque)
        {
            DetectarEnemigos();
            DetectarCajas(); // Llamamos a la nueva función para detectar cajas
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
                    StartCoroutine(AtaquePinchadoCaja(caja.transform.position, cajaScript)); // Ahora pasa la caja correcta
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

        // **Aquí usamos la caja correcta**
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

    private void AplicarDaño()
    {
        Collider2D[] enemigosGolpeados = Physics2D.OverlapCircleAll(transform.position, alcance, capaEnemigos);

        foreach (Collider2D enemigo in enemigosGolpeados)
        {
            VidaEnemigo salud = enemigo.GetComponent<VidaEnemigo>();
            if (salud != null)
            {
                ProbabilidadCritico();
                if (esCritico) 
                {
                    salud.RecibirDaño(danioCritico);
                }
                else 
                {
                    salud.RecibirDaño(danio);
                    Debug.Log("Daño: " + danio);
                }
                esCritico = false;
                atacando = false;
                RobarVida();
            } else atacando = false; //False para evitar fallos, por si algun otra arma ha matado ya ese enemigo
        }
    }

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
        // Intentar obtener el jugador con el tag 'Jugador'
         jugador = GameObject.FindWithTag("Jugador");

        if (jugador != null)
        {
            vidaJugador = jugador.GetComponent<VidaJugador>();
        }
    }

    public void AumentarProbabilidadRobarVida (int cantidad) 
    {
        probabilidadRobarVida += cantidad; //Aumenta la probabilidad de robar vida
    }

    public void AumentarDanioPorPocentaje(int porcentaje)
    {
        danio = danio + Mathf.RoundToInt(danioBase * (porcentaje / 100f)); //Aumenta el daño con porcentajes tieniendo en cuenta el daño base
    }

    public void DisminuirRecargaPorPocentaje(int porcentaje)
    {
        recarga -= recargaBase * (porcentaje / 100f); // //Disminuye la recarga con porcentajes tieniendo en cuenta la recarga base base
        recarga = Mathf.Round(recarga * 100f) / 100f; // Redondea a 2 decimales para mayor precisión 
    }

    public void AumentarProbabilidadCritico(int cantidad)
    {
       probabilidadCritico = probabilidadCritico + cantidad;     
    }
}
