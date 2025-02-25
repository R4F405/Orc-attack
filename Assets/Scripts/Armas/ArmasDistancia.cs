using UnityEngine;

public class ArmasDistancia : MonoBehaviour
{
    public GameObject balaPrefab; // Prefab de la bala

    public int danioBase = 0; // Daño base de la bala (Solo se modifica desde el inspector)
    [HideInInspector] public int danio = 0; // Daño de la bala


    public float recargaBase = 0f; // Tiempo de recarga base entre disparos (Solo se modifica desde el inspector)
    [HideInInspector]public float recarga = 0f; // Tiempo de recarga entre disparos

    public int probabilidadCritico = 0; //Probabilidad de critico 
    public float velocidadBala = 0f; // Velocidad de la bala
    public float alcance = 0f;
    public int probabilidadRobarVida = 0; // Probabilidad de robar 1 de vida tras golpe en %
    public LayerMask capaEnemigos; // Capa de los enemigos
    public LayerMask capaCajas; // Nueva capa para detectar cajas

    private Collider2D colliderJugador; // Referencia al collider del jugador
    private float tiempoSiguienteDisparo = 0f;
    private bool esCritico = false;
    private int danioCritico;

    private void Start()
    {
        danio = danioBase; // Inicializar el daño con el valor base
        recarga = recargaBase; // Inicializar la recarga con el valor base

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
        Collider2D[] objetivos = Physics2D.OverlapCircleAll(transform.position, alcance, capaEnemigos | capaCajas);
        if (objetivos.Length > 0)
        {
            Transform objetivo = objetivos[0].transform;

            GameObject bala = Instantiate(balaPrefab, ObtenerPuntoDisparo(), Quaternion.identity);
            Bala scriptBala = bala.GetComponent<Bala>();
            if (scriptBala != null)
            {
                ProbabilidadCritico();
                if (esCritico) 
                {
                    Debug.Log("Golpe critico distancia, daño: " + danioCritico);
                    scriptBala.ConfigurarBala(danioCritico, velocidadBala, capaEnemigos, capaCajas, colliderJugador, objetivo,probabilidadRobarVida);
                }
                else 
                {
                    scriptBala.ConfigurarBala(danio, velocidadBala, capaEnemigos, capaCajas, colliderJugador, objetivo,probabilidadRobarVida);
                    Debug.Log("Golpe normal distancia, daño: " + danio);
                }
                esCritico = false;
            }

            tiempoSiguienteDisparo = Time.time + recarga;
        }
    }

    private Vector2 ObtenerPuntoDisparo()
    {
        return new Vector2(transform.position.x, transform.position.y + (GetComponent<SpriteRenderer>().bounds.size.y / 2));
    }

    private void ProbabilidadCritico() 
    {
        int probabilidad = Random.Range(0, 100);
         Debug.Log("probabilidad de critico distancia" + probabilidad + " " + probabilidadCritico);
        if (probabilidad < probabilidadCritico)
        {
            danioCritico = danio * 2;
            esCritico = true;
        }   
    }

    public void AumentarProbabilidadRobarVida (int cantidad) 
    {
        probabilidadRobarVida += cantidad;
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
