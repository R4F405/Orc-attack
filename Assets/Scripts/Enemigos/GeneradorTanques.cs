using UnityEngine;

public class GeneradorTanques : MonoBehaviour
{
    public GameObject tanquePrefab; // Prefabricado del Tanque
    public Vector2 limiteInferior; // Límite inferior de la zona de generación
    public Vector2 limiteSuperior; // Límite superior de la zona de generación
    public float tiempoEntreOleadas = 5f; // Tiempo base entre oleadas
    public float radioSeguridad = 2.5f; // Radio alrededor del jugador donde no pueden aparecer enemigos

    private Transform jugador; // Referencia al jugador
    private float temporizador; // Temporizador para gestionar las oleadas
    private ControladorNiveles controladorNiveles; // Referencia al controlador de niveles
    private int tanquesPorOleadaActual;
    private float tiempoEntreOleadasActual;
    private bool puedeGenerarTanques = false;

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
            return;
        }

        // Verificar si ya podemos generar tanques (a partir del nivel 3)
        if (!puedeGenerarTanques && controladorNiveles.nivelActual >= 3)
        {
            puedeGenerarTanques = true;
            AjustarDificultadSegunNivel();
        }

        if (!puedeGenerarTanques)
            return;

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
            Debug.Log("Jugador encontrado correctamente.{GeneradorTanques}");
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
        
        // Tanques solo aparecen a partir del nivel 3
        if (nivelActual < 3)
        {
            tanquesPorOleadaActual = 0;
            puedeGenerarTanques = false;
            return;
        }
        
        // Calcular tanques por oleada: +1 cada 2 niveles a partir del nivel 3
        tanquesPorOleadaActual = 1 + Mathf.FloorToInt((nivelActual - 4) / 2);
        
        // Ajustar tiempo entre oleadas (disminuye con el nivel, pero tiene un mínimo)
        tiempoEntreOleadasActual = Mathf.Max(3.0f, tiempoEntreOleadas - (nivelActual * 0.2f));
        
        Debug.Log($"Nivel {nivelActual}: {tanquesPorOleadaActual} tanques por oleada, cada {tiempoEntreOleadasActual} segundos");
    }

    void GenerarOleada()
    {
        // Actualizar dificultad antes de generar la oleada
        if (controladorNiveles != null)
        {
            AjustarDificultadSegunNivel();
        }

        for (int i = 0; i < tanquesPorOleadaActual; i++)
        {
            GenerarEnemigo();
        }
    }

    void GenerarEnemigo()
    {
        if (jugador == null || tanquePrefab == null) return; // Evitar errores

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
        GameObject tanque = Instantiate(tanquePrefab, posicionGeneracion, Quaternion.identity);
        
        // Ajustar estadísticas según el nivel
        if (controladorNiveles != null)
        {
            int nivelActual = controladorNiveles.nivelActual;
            
            // Ajustar la vida del enemigo
            VidaEnemigo vidaEnemigo = tanque.GetComponent<VidaEnemigo>();
            if (vidaEnemigo != null)
            {
                // Aumenta la vida en x puntos por cada nivel
                vidaEnemigo.saludMaxima = vidaEnemigo.saludMaxima + (nivelActual - 1) * 2;
                // Importante: reiniciar la salud actual al máximo
                vidaEnemigo.ReiniciarSalud();
            }
            
            // Ajustar el daño del enemigo
            SistemaDanioColisionEnemigo sistemaDanio = tanque.GetComponent<SistemaDanioColisionEnemigo>();
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