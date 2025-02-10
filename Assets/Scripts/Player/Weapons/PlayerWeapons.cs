using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
 public GameObject[] prefabricadosDeArmas; // Array de prefabricados de armas
    public float distanciaHorizontal = 1.5f; // Distancia horizontal entre las armas
    public float distanciaVertical = 1.5f; // Distancia vertical entre las armas
    public int numeroDeArmas = 6; // Número total de armas que el jugador puede tener

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

        // Posicionar armas a la derecha
        for (int i = 0; i < numeroDeArmas / 2; i++)
        {
            if (i < prefabricadosDeArmas.Length)
            {
                // Calcula la posición para la arma en el lado derecho
                Vector3 posicionDerecha = new Vector3(distanciaHorizontal, (i - 1) * distanciaVertical, 0);
                armasInstanciadas[i] = Instantiate(prefabricadosDeArmas[i % prefabricadosDeArmas.Length], transform.position + posicionDerecha, Quaternion.identity);
                armasInstanciadas[i].transform.rotation = Quaternion.Euler(0, 0, 0); // Opcional: Ajusta la rotación si es necesario
            }
        }

        // Posicionar armas a la izquierda
        for (int i = numeroDeArmas / 2; i < numeroDeArmas; i++)
        {
            if (i < prefabricadosDeArmas.Length)
            {
                // Calcula la posición para la arma en el lado izquierdo
                Vector3 posicionIzquierda = new Vector3(-distanciaHorizontal, ((i - numeroDeArmas / 2) - 1) * distanciaVertical, 0);
                armasInstanciadas[i] = Instantiate(prefabricadosDeArmas[i % prefabricadosDeArmas.Length], transform.position + posicionIzquierda, Quaternion.identity);
                armasInstanciadas[i].transform.rotation = Quaternion.Euler(0, 0, 0); // Opcional: Ajusta la rotación si es necesario
            }
        }
    }

    private void Update()
    {
        // Actualizar la posición de las armas para que sigan al jugador
        for (int i = 0; i < numeroDeArmas / 2; i++)
        {
            if (armasInstanciadas[i] != null)
            {
                Vector3 posicionDerecha = new Vector3(distanciaHorizontal, (i - 1) * distanciaVertical, 0);
                armasInstanciadas[i].transform.position = transform.position + posicionDerecha;
            }
        }

        for (int i = numeroDeArmas / 2; i < numeroDeArmas; i++)
        {
            if (armasInstanciadas[i] != null)
            {
                Vector3 posicionIzquierda = new Vector3(-distanciaHorizontal, ((i - numeroDeArmas / 2) - 1) * distanciaVertical, 0);
                armasInstanciadas[i].transform.position = transform.position + posicionIzquierda;
            }
        }
    }
}