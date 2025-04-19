using UnityEngine;

public class PosicionarArmasJugador : MonoBehaviour
{
    public GameObject[] prefabricadosDeArmas; // Array de prefabricados de armas
    public float distanciaHorizontal = 1.5f; // Distancia horizontal entre las armas
    public float distanciaVertical = 1.5f; // Distancia vertical entre las armas
    public int numeroDeArmas = 1; // Número total de armas que el jugador tiene

    private GameObject[] armasInstanciadas;  // Array de instancias de las armas

    private void Start()
    {
        // Inicializar el array de armas instanciadas
        armasInstanciadas = new GameObject[numeroDeArmas];
        Debug.Log(armasInstanciadas.Length);

        // Crear las armas alrededor del jugador
        PosicionarArmas();
    }

    public void PosicionarArmas()
    {
        // Si no hay prefabricados de armas, no hacer nada
        if (prefabricadosDeArmas.Length == 0) return;

        // Buscar el GestorMejorasArmas para asegurarnos de que existe
        GestorMejorasArmas gestorMejoras = FindAnyObjectByType<GestorMejorasArmas>();
        if (gestorMejoras == null)
        {
            // Si no existe, crear un objeto con el componente GestorMejorasArmas
            GameObject gestorMejorasObj = new GameObject("GestorMejorasArmas");
            gestorMejoras = gestorMejorasObj.AddComponent<GestorMejorasArmas>();
            DontDestroyOnLoad(gestorMejorasObj);
            Debug.Log("Creado nuevo GestorMejorasArmas");
        }

        if (numeroDeArmas == 1)
        {
            // Si hay solo una arma, se coloca a la izquierda un poco más arriba de la altura del jugador
            Vector3 posicionIzquierda = new Vector3(-distanciaHorizontal, distanciaVertical, 0);
            armasInstanciadas[0] = Instantiate(prefabricadosDeArmas[0], transform.position + posicionIzquierda, Quaternion.identity);
            Debug.Log("Arma instanciada con danioBase: " + armasInstanciadas[0].GetComponent<ArmasMelee>()?.danioBase);
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
        if (armasInstanciadas == null || armasInstanciadas.Length == 0) return;

        for (int i = 0; i < armasInstanciadas.Length; i++)
        {
            if (armasInstanciadas[i] != null)
            {
                // Ajuste dinámico de las posiciones dependiendo de la cantidad de armas
                switch (numeroDeArmas)
                {
                    case 1:
                        armasInstanciadas[0].transform.position = transform.position + new Vector3(-distanciaHorizontal, 0, 0);
                        break;
                    case 2:
                        armasInstanciadas[0].transform.position = transform.position + new Vector3(-distanciaHorizontal, 0, 0);
                        armasInstanciadas[1].transform.position = transform.position + new Vector3(distanciaHorizontal, 0, 0);
                        break;
                    case 3:
                        armasInstanciadas[0].transform.position = transform.position + new Vector3(-distanciaHorizontal, 0, 0);
                        armasInstanciadas[1].transform.position = transform.position + new Vector3(distanciaHorizontal, 0, 0);
                        armasInstanciadas[2].transform.position = transform.position + new Vector3(0, distanciaVertical * 2f, 0);
                        break;
                    case 4:
                        armasInstanciadas[0].transform.position = transform.position + new Vector3(-distanciaHorizontal, -distanciaVertical, 0);
                        armasInstanciadas[1].transform.position = transform.position + new Vector3(distanciaHorizontal, -distanciaVertical, 0);
                        armasInstanciadas[2].transform.position = transform.position + new Vector3(-distanciaHorizontal, distanciaVertical, 0);
                        armasInstanciadas[3].transform.position = transform.position + new Vector3(distanciaHorizontal, distanciaVertical, 0);
                        break;
                    case 5:
                        armasInstanciadas[0].transform.position = transform.position + new Vector3(-distanciaHorizontal, -distanciaVertical, 0);
                        armasInstanciadas[1].transform.position = transform.position + new Vector3(distanciaHorizontal, -distanciaVertical, 0);
                        armasInstanciadas[2].transform.position = transform.position + new Vector3(-distanciaHorizontal, distanciaVertical, 0);
                        armasInstanciadas[3].transform.position = transform.position + new Vector3(distanciaHorizontal, distanciaVertical, 0);
                        armasInstanciadas[4].transform.position = transform.position + new Vector3(0, distanciaVertical * 2f, 0);
                        break;
                }
            }
        }
    }


    public Vector3 ObtenerPosicionActualDelArma(GameObject arma)
    {
        for (int i = 0; i < armasInstanciadas.Length; i++)
        {
            if (armasInstanciadas[i] == arma)
            {
                return transform.position + (armasInstanciadas[i].transform.position - transform.position);
            }
        }
        return arma.transform.position; // En caso de error, usa su posición actual
    }

    public void AgregarArma(GameObject nuevaArma)
    {
        // Eliminar todas las armas previas en la capa "Armas"
        GameObject[] armasEnCapa = GameObject.FindGameObjectsWithTag("Armas");
        foreach (var arma in armasEnCapa)
        {
            Destroy(arma);
        }

        // Incrementar el número de armas
        numeroDeArmas++;

        // Crear un nuevo array de prefabricados con el tamaño actualizado
        GameObject[] nuevoArrayPrefabricados = new GameObject[numeroDeArmas];
        
        // Copiar las armas existentes al nuevo array de prefabricados
        for (int i = 0; i < prefabricadosDeArmas.Length; i++)
        {
            nuevoArrayPrefabricados[i] = prefabricadosDeArmas[i];
        }

        // Añadir la nueva arma al final del array de prefabricados
        nuevoArrayPrefabricados[numeroDeArmas - 1] = nuevaArma;

        // Actualizar el array de prefabricados
        prefabricadosDeArmas = nuevoArrayPrefabricados;

        // Crear un nuevo array para las armas instanciadas con el tamaño actualizado
        armasInstanciadas = new GameObject[numeroDeArmas];

        // Reposicionar las armas
        PosicionarArmas();
    }

    public GameObject[] ObtenerArmasActuales()
    {
        return armasInstanciadas;
    }

}