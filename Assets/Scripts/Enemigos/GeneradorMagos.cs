using UnityEngine;

public class GeneradorMagos : MonoBehaviour
{
    public GameObject magoPrefab; // Prefabricado de Orco
    public Vector2 limiteInferior; // Limite inferior de la zona de generación
    public Vector2 limiteSuperior; // Limite superior de la zona de generación
    public int enemigosPorOleada = 2; // Número de enemigos por oleada
    public float tiempoEntreOleadas = 7f; // Tiempo entre oleadas
    public float radioSeguridad = 2f; // Radio alrededor del jugador donde no pueden aparecer enemigos

    private Transform jugador; // Referencia al jugador
    private float temporizador; // Temporizador para gestionar las oleadas

    private void Start()
    {
        BuscarJugador();
        temporizador = tiempoEntreOleadas; // Inicia el temporizador para la primera oleada
    }

    private void Update()
    {
         // Si el jugador no ha sido encontrado, seguir buscándolo
        if (jugador == null)
        {
            BuscarJugador();
            return; // No generar enemigos hasta que el jugador sea encontrado
        }

        temporizador -= Time.deltaTime;

        if (temporizador <= 0)
        {
            GenerarOleada();
            temporizador = tiempoEntreOleadas; // Resetear temporizador para la siguiente oleada
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

    void GenerarOleada()
    {
        for (int i = 0; i < enemigosPorOleada; i++)
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
        Instantiate(magoPrefab, posicionGeneracion, Quaternion.identity);
    }
}
