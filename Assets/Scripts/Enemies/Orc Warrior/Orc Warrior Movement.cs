using UnityEngine;

public class OrcWarriorMovement : MonoBehaviour
{
    public Transform jugador;
    public float velocidad = 2f;

    private bool estaMirandoDerecha = true;

    void FixedUpdate()
    {
        IASeguimiento();
    }

    void Update()
    {

        bool estaJugadorDerecha = transform.position.x < jugador.transform.position.x; //Obtiene si esta el jugador en el lado derecho
        Girar(estaJugadorDerecha);
    }

    private void IASeguimiento()
    {
        transform.position = Vector2.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime); //Seguimiento al jugador
    }

    private void Girar(bool estaJugadorDerecha)
    {
        //Gira el enemigo  segun la posicion del jugador
        if ((estaMirandoDerecha && !estaJugadorDerecha) || (!estaMirandoDerecha && estaJugadorDerecha))
        {
            estaMirandoDerecha = !estaMirandoDerecha;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}
