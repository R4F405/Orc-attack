
using UnityEngine;

public class OrcWarriorMovement : MonoBehaviour
{
    public Transform jugador;
    public float velocidad;

    private bool estaMirandoDerecha = true;

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);

        bool estaJugadorDerecha = transform.position.x < jugador.transform.position.x;
        Flip(estaJugadorDerecha);
    }

    private void Flip(bool estaJugadorDerecha) 
    {
        if ((estaMirandoDerecha && !estaJugadorDerecha) || (!estaMirandoDerecha && estaJugadorDerecha)) 
        {
            estaMirandoDerecha = !estaMirandoDerecha;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

        }
    }
}
