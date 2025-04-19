using UnityEngine;

public class GeneradorMagos : MonoBehaviour
{
    public GameObject magoPrefab; // Prefabricado de Orco
    public Vector2 limiteInferior; // Limite inferior de la zona de generación
    public Vector2 limiteSuperior; // Limite superior de la zona de generación
    public int enemigosPorOleada = 2; // Número base de enemigos por oleada
    public float tiempoEntreOleadas = 7f; // Tiempo base entre oleadas
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
            temporizador = tiempoEntreOleadasActual; // Resetear temporizador para la siguiente oleada
        }
    }

    void BuscarJugador()
    {
        GameObject jugadorObjeto = GameObject.FindGameObjectWithTag("Jugador");
        if (jugadorObjeto != null)
        {
            jugador = jugadorObjeto.transform;
            Debug.Log("Jugador encontrado correctamente. {GeneradorMagos}");
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
        
        // Los magos aparecen más lentamente que los orcos, pero son más peligrosos
        // Por eso aumentamos de forma más conservadora
        
        // Ajustar cantidad de enemigos por oleada (aumenta con el nivel)
        // Comenzamos con 2 y añadimos uno cada 3 niveles
        enemigosPorOleadaActual = enemigosPorOleada + Mathf.FloorToInt((nivelActual - 1) / 3);
        
        // Ajustar tiempo entre oleadas (disminuye con el nivel, pero tiene un mínimo)
        // Los magos aparecen menos frecuentemente que los orcos
        tiempoEntreOleadasActual = Mathf.Max(3.5f, tiempoEntreOleadas - (nivelActual * 0.15f));
        
        Debug.Log($"Nivel {nivelActual}: {enemigosPorOleadaActual} magos por oleada, cada {tiempoEntreOleadasActual} segundos");
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
        GameObject mago = Instantiate(magoPrefab, posicionGeneracion, Quaternion.identity);
        
        // Ajustar estadísticas según el nivel
        if (controladorNiveles != null)
        {
            int nivelActual = controladorNiveles.nivelActual;
            
            // Ajustar la vida del enemigo
            VidaEnemigo vidaEnemigo = mago.GetComponent<VidaEnemigo>();
            if (vidaEnemigo != null)
            {
                // Aumenta la vida en 4 puntos por cada nivel
                vidaEnemigo.saludMaxima = vidaEnemigo.saludMaxima + (nivelActual - 1) * 4;
                // Importante: reiniciar la salud actual al máximo
                vidaEnemigo.ReiniciarSalud();
            }
            
            // Ajustar el daño de los proyectiles
            SistemaDanioDistanciaEnemigo sistemaDanio = mago.GetComponent<SistemaDanioDistanciaEnemigo>();
            if (sistemaDanio != null)
            {
                // Aumenta el daño cada 2 niveles a partir del nivel 2
                if (nivelActual > 2)
                {
                    sistemaDanio.daño = sistemaDanio.daño + Mathf.FloorToInt((nivelActual - 1) / 2);
                }
                
                // Reducir ligeramente el tiempo entre disparos con el nivel
                if (nivelActual > 3)
                {
                    sistemaDanio.tiempoEntreDisparos = Mathf.Max(1.5f, sistemaDanio.tiempoEntreDisparos - (nivelActual * 0.05f));
                }
            }
        }
    }
}
