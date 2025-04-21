using UnityEngine;

public class GeneradorOrcos : MonoBehaviour
{
    public GameObject orcoPrefab; // Prefabricado de Orco
    public Vector2 limiteInferior; // Límite inferior de la zona de generación
    public Vector2 limiteSuperior; // Límite superior de la zona de generación
    public int enemigosPorOleada = 5; // Número base de enemigos por oleada
    public float tiempoEntreOleadas = 3f; // Tiempo base entre oleadas
    public float radioSeguridad = 2f; // Radio alrededor del jugador donde no pueden aparecer enemigos

    private Transform jugador; // Referencia al jugador
    private float temporizador; // Temporizador para gestionar las oleadas
    private ControladorNiveles controladorNiveles; // Referencia al controlador de niveles
    private int enemigosPorOleadaActual;
    private float tiempoEntreOleadasActual;

    private void Start()
    {
        BuscarJugador();
        BuscarControladorNiveles();
        AjustarDificultadSegunNivel();
        temporizador = tiempoEntreOleadasActual; // Inicia el temporizador para la primera oleada
    }

    private void Update()
    {
        // Si el jugador no ha sido encontrado, seguir buscándolo
        if (jugador == null)
        {
            BuscarJugador();
            return; // No generar enemigos hasta que el jugador sea encontrado
        }

        // Si no tenemos referencia al controlador de niveles, seguir buscándolo
        if (controladorNiveles == null)
        {
            BuscarControladorNiveles();
        }

        temporizador -= Time.deltaTime;

        if (temporizador <= 0)
        {
            GenerarOleada();
            temporizador = tiempoEntreOleadasActual; // Reiniciar temporizador para la siguiente oleada
        }
    }

    void BuscarJugador()
    {
        GameObject jugadorObjeto = GameObject.FindGameObjectWithTag("Jugador");
        if (jugadorObjeto != null)
        {
            jugador = jugadorObjeto.transform;
            Debug.Log("Jugador encontrado correctamente.{GeneradorOrcos}");
        }
    }

    void BuscarControladorNiveles()
    {
        controladorNiveles = FindAnyObjectByType<ControladorNiveles>();
        if (controladorNiveles != null)
        {
            AjustarDificultadSegunNivel();
        }
    }

    void AjustarDificultadSegunNivel()
    {
        if (controladorNiveles == null) return;

        int nivelActual = controladorNiveles.nivelActual;
        
        // Ajustar cantidad de enemigos por oleada (aumenta con el nivel)
        enemigosPorOleadaActual = enemigosPorOleada + Mathf.FloorToInt(nivelActual / 2);
        
        // Ajustar tiempo entre oleadas (disminuye con el nivel, pero tiene un mínimo)
        tiempoEntreOleadasActual = Mathf.Max(2.0f, tiempoEntreOleadas - (nivelActual * 0.2f));
        
        Debug.Log($"Nivel {nivelActual}: {enemigosPorOleadaActual} orcos por oleada, cada {tiempoEntreOleadasActual} segundos");
    }

    void GenerarOleada()
    {
        // Actualizar dificultad antes de generar la oleada
        if (controladorNiveles != null)
        {
            AjustarDificultadSegunNivel();
        }

        for (int i = 0; i < enemigosPorOleadaActual; i++)
        {
            GenerarEnemigo();
        }
    }

    void GenerarEnemigo()
    {
        if (jugador == null) return; // Evitar errores si el jugador no se ha encontrado

        Vector3 posicionGeneracion;
        bool posicionValida = false;

        // Generar posición hasta encontrar una válida fuera del radio de seguridad
        do
        {
            float posX = Random.Range(limiteInferior.x, limiteSuperior.x);
            float posY = Random.Range(limiteInferior.y, limiteSuperior.y);
            posicionGeneracion = new Vector3(posX, posY, 0f);

            // Verificar si la posición está fuera del radio de seguridad del jugador
            if (Vector3.Distance(posicionGeneracion, jugador.position) > radioSeguridad)
            {
                posicionValida = true;
            }

        } while (!posicionValida);

        // Instanciar enemigo
        GameObject orco = Instantiate(orcoPrefab, posicionGeneracion, Quaternion.identity);
        
        // Ajustar estadísticas según el nivel
        if (controladorNiveles != null)
        {
            int nivelActual = controladorNiveles.nivelActual;
            
            // Ajustar la vida del enemigo
            VidaEnemigo vidaEnemigo = orco.GetComponent<VidaEnemigo>();
            if (vidaEnemigo != null)
            {
                // Aumenta la vida en x puntos por cada nivel
                vidaEnemigo.saludMaxima = vidaEnemigo.saludMaxima + (nivelActual - 1) * 2;
                // Importante: reiniciar la salud actual al máximo
                vidaEnemigo.ReiniciarSalud();
            }
            
            // Ajustar el daño del enemigo
            SistemaDanioColisionEnemigo sistemaDanio = orco.GetComponent<SistemaDanioColisionEnemigo>();
            if (sistemaDanio != null)
            {
                // Aumenta el daño cada 2 niveles
                if (nivelActual > 2)
                {
                    sistemaDanio.daño = sistemaDanio.daño + Mathf.FloorToInt((nivelActual - 1) / 2);
                }
            }
        }
    }
}
