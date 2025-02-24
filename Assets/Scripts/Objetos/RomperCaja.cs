using UnityEngine;

public class RomperCaja : MonoBehaviour
{
    public GameObject pocionPrefab; // Asigna la poción en el Inspector
    public int vidaCaja = 1; // La cantidad de golpes que aguanta la caja

    public void RecibirGolpe()
    {
        if (pocionPrefab != null)
        {
            Instantiate(pocionPrefab, transform.position, Quaternion.identity); //Instancia la pocion en el lugar de la caja
        }
        Destroy(gameObject); // Destruye la caja
    }

}
