using UnityEngine;

public class PosicionarArmasJugador : MonoBehaviour
{
    public GameObject[] prefabricadosDeArmas; // Array de prefabricados de armas
    public float distanciaHorizontal = 1.5f; // Distancia horizontal entre las armas
    public float distanciaVertical = 1.5f; // Distancia vertical entre las armas
    public int numeroDeArmas = 5; // Número total de armas que el jugador tiene

    private GameObject[] armasInstanciadas; // Array de instancias de las armas

    private void Start()
    {
        // Inicializar el array de armas instanciadas
        armasInstanciadas = new GameObject[numeroDeArmas];

        // Crear las armas alrededor del jugador
        PosicionarArmas();
    }

    void PosicionarArmas()
    {
        // Si no hay prefabricados de armas, no hacer nada
        if (prefabricadosDeArmas.Length == 0) return;

        if (numeroDeArmas == 1)
        {
            // Si hay solo una arma, se coloca a la izquierda un poco más arriba de la altura del jugador
            Vector3 posicionIzquierda = new Vector3(-distanciaHorizontal, distanciaVertical, 0);
            armasInstanciadas[0] = Instantiate(prefabricadosDeArmas[0], transform.position + posicionIzquierda, Quaternion.identity);
        }
        else if (numeroDeArmas == 2)
        {
            // Si hay dos armas, una a cada lado, a la altura del jugador
            Vector3 posicionDerecha = new Vector3(distanciaHorizontal, 0, 0);
            Vector3 posicionIzquierda = new Vector3(-distanciaHorizontal, 0, 0);
            
            armasInstanciadas[0] = Instantiate(prefabricadosDeArmas[0], transform.position + posicionIzquierda, Quaternion.identity);
            armasInstanciadas[1] = Instantiate(prefabricadosDeArmas[1], transform.position + posicionDerecha, Quaternion.identity);
        }
        else if (numeroDeArmas == 3)
        {
            // Si hay tres armas, dos a los lados y una encima del jugador
            Vector3 posicionDerecha = new Vector3(distanciaHorizontal, 0, 0);
            Vector3 posicionIzquierda = new Vector3(-distanciaHorizontal, 0, 0);
            Vector3 posicionArriba = new Vector3(0, distanciaVertical, 0);

            armasInstanciadas[0] = Instantiate(prefabricadosDeArmas[0], transform.position + posicionIzquierda, Quaternion.identity);
            armasInstanciadas[1] = Instantiate(prefabricadosDeArmas[1], transform.position + posicionDerecha, Quaternion.identity);
            armasInstanciadas[2] = Instantiate(prefabricadosDeArmas[2], transform.position + posicionArriba, Quaternion.identity);
        }
        else if (numeroDeArmas == 4)
        {
            // Si hay cuatro armas, dos a cada lado
            Vector3 posicionDerecha = new Vector3(distanciaHorizontal, 0, 0);
            Vector3 posicionIzquierda = new Vector3(-distanciaHorizontal, 0, 0);
            Vector3 posicionArriba = new Vector3(0, distanciaVertical, 0);
            Vector3 posicionAbajo = new Vector3(0, -distanciaVertical, 0);

            armasInstanciadas[0] = Instantiate(prefabricadosDeArmas[0], transform.position + posicionIzquierda + posicionAbajo, Quaternion.identity);
            armasInstanciadas[1] = Instantiate(prefabricadosDeArmas[1], transform.position + posicionDerecha + posicionAbajo, Quaternion.identity);
            armasInstanciadas[2] = Instantiate(prefabricadosDeArmas[2], transform.position + posicionIzquierda + posicionArriba, Quaternion.identity);
            armasInstanciadas[3] = Instantiate(prefabricadosDeArmas[3], transform.position + posicionDerecha + posicionArriba, Quaternion.identity);
        }
        else if (numeroDeArmas == 5)
        {
            // Si hay cinco armas, dos a cada lado y una encima
            Vector3 posicionDerecha = new Vector3(distanciaHorizontal, 0, 0);
            Vector3 posicionIzquierda = new Vector3(-distanciaHorizontal, 0, 0);
            Vector3 posicionArriba = new Vector3(0, distanciaVertical, 0);
            Vector3 posicionAbajo = new Vector3(0, -distanciaVertical, 0);

            armasInstanciadas[0] = Instantiate(prefabricadosDeArmas[0], transform.position + posicionIzquierda + posicionAbajo, Quaternion.identity);
            armasInstanciadas[1] = Instantiate(prefabricadosDeArmas[1], transform.position + posicionDerecha + posicionAbajo, Quaternion.identity);
            armasInstanciadas[2] = Instantiate(prefabricadosDeArmas[2], transform.position + posicionIzquierda + posicionArriba, Quaternion.identity);
            armasInstanciadas[3] = Instantiate(prefabricadosDeArmas[3], transform.position + posicionDerecha + posicionArriba, Quaternion.identity);
            armasInstanciadas[4] = Instantiate(prefabricadosDeArmas[4], transform.position + posicionArriba, Quaternion.identity); // Quinta arma en la zona superior
        }
    }

    private void Update()
    {
        // Actualizar la posición de las armas para que sigan al jugador
        for (int i = 0; i < numeroDeArmas; i++)
        {
            if (armasInstanciadas[i] != null)
            {
                // Ajuste dinámico de las posiciones, dependiendo de las armas
                if (numeroDeArmas == 1)
                {
                    // Una sola arma, a la izquierda y un poco más arriba de la altura del jugador
                    if (i == 0) armasInstanciadas[i].transform.position = transform.position + new Vector3(-distanciaHorizontal, 0, 0);
                }
                else if (numeroDeArmas == 2)
                {
                    // Dos armas a los lados a la altura del jugador
                    if (i == 0) armasInstanciadas[i].transform.position = transform.position + new Vector3(-distanciaHorizontal, 0, 0);
                    if (i == 1) armasInstanciadas[i].transform.position = transform.position + new Vector3(distanciaHorizontal, 0, 0);
                }
                else if (numeroDeArmas == 3)
                {
                    // Tres armas: dos a los lados y una encima
                    if (i == 0) armasInstanciadas[i].transform.position = transform.position + new Vector3(-distanciaHorizontal, 0, 0);
                    if (i == 1) armasInstanciadas[i].transform.position = transform.position + new Vector3(distanciaHorizontal, 0, 0);
                    if (i == 2) armasInstanciadas[i].transform.position = transform.position + new Vector3(0, distanciaVertical, 0);
                }
                else if (numeroDeArmas == 4)
                {
                    // Cuatro armas: dos a cada lado
                    if (i == 0) armasInstanciadas[i].transform.position = transform.position + new Vector3(-distanciaHorizontal, 0, 0) + new Vector3(0, -distanciaVertical, 0);
                    if (i == 1) armasInstanciadas[i].transform.position = transform.position + new Vector3(distanciaHorizontal, 0, 0) + new Vector3(0, -distanciaVertical, 0);
                    if (i == 2) armasInstanciadas[i].transform.position = transform.position + new Vector3(-distanciaHorizontal, 0, 0) + new Vector3(0, distanciaVertical, 0);
                    if (i == 3) armasInstanciadas[i].transform.position = transform.position + new Vector3(distanciaHorizontal, 0, 0) + new Vector3(0, distanciaVertical, 0);
                }
                else if (numeroDeArmas == 5)
                {
                    // Cinco armas: dos a cada lado y una encima
                    if (i == 0) armasInstanciadas[i].transform.position = transform.position + new Vector3(-distanciaHorizontal, 0, 0) + new Vector3(0, -distanciaVertical, 0);
                    if (i == 1) armasInstanciadas[i].transform.position = transform.position + new Vector3(distanciaHorizontal, 0, 0) + new Vector3(0, -distanciaVertical, 0);
                    if (i == 2) armasInstanciadas[i].transform.position = transform.position + new Vector3(-distanciaHorizontal, 0, 0) + new Vector3(0, distanciaVertical, 0);
                    if (i == 3) armasInstanciadas[i].transform.position = transform.position + new Vector3(distanciaHorizontal, 0, 0) + new Vector3(0, distanciaVertical, 0);
                    if (i == 4) armasInstanciadas[i].transform.position = transform.position + new Vector3(0, 1.5f, 0);
                }
            }
        }
    }
}