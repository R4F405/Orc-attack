using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
     public GameObject[] prefabricadosDeArmas; // Array de prefabricados de armas
    public float distanciaArmas = 1.5f; // Distancia entre las armas
    public int numeroDeArmas = 6; // Número total de armas que el jugador puede tener

    private void Start()
    {
        // Crear las armas alrededor del jugador
        PosicionarArmas();
    }

    void PosicionarArmas()
    {
        // Si no hay prefabricados de armas, no hacer nada
        if (prefabricadosDeArmas.Length == 0) return;

        // Posicionar armas a la derecha
        for (int i = 0; i < numeroDeArmas / 2; i++) // Para la mitad de las armas
        {
            if (i < prefabricadosDeArmas.Length)
            {
                // Calcula la posición para la arma en el lado derecho
                Vector3 posicionDerecha = new Vector3(i * distanciaArmas, 0, 0);
                GameObject armaInstanciada = Instantiate(prefabricadosDeArmas[i % prefabricadosDeArmas.Length], transform.position + posicionDerecha, Quaternion.identity, transform);
                armaInstanciada.transform.rotation = Quaternion.Euler(0, 0, 0); // Opcional: Ajusta la rotación si es necesario
            }
        }

        // Posicionar armas a la izquierda
        for (int i = numeroDeArmas / 2; i < numeroDeArmas; i++) // Para la otra mitad de las armas
        {
            if (i < prefabricadosDeArmas.Length)
            {
                // Calcula la posición para la arma en el lado izquierdo
                Vector3 posicionIzquierda = new Vector3(-((i - numeroDeArmas / 2) * distanciaArmas), 0, 0);
                GameObject armaInstanciada = Instantiate(prefabricadosDeArmas[i % prefabricadosDeArmas.Length], transform.position + posicionIzquierda, Quaternion.identity, transform);
                armaInstanciada.transform.rotation = Quaternion.Euler(0, 0, 180); // Opcional: Ajusta la rotación si es necesario
            }
        }
    }
}