using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    PlayerMovement playerMovement; // Referencia al script PlayerMovement para obtener la direccion
    Animator animator;

    //Se llama al empezar
    void Start()
    {
        animator = GetComponent<Animator>(); //Se obtiene el componente 
        playerMovement = GetComponent<PlayerMovement>(); // Obtienes el componente PlayerMovement
    }
    
    //Se llama tras cada frame
    void Update()
    {
        Animaciones();

    }

    //Funcion de animaciones
    private void Animaciones() 
    {
        Vector2 direction = playerMovement.direction; // Accedes a la dirección de movimiento del jugador

        //Si esta quieto (IDLE)
        if (direction == Vector2.zero) animator.SetBool("corriendo", false);
        //Si se mueve (RUN)
        else animator.SetBool("corriendo", true);

        // Si se mueve a la serecha (ANIMACION)
        if (direction.x > 0) transform.localScale = new Vector3(1f, 1f, 1f); 
         // Si se mueve a la izquierda (ANIMACION INVERSA)
        else if (direction.x < 0)  transform.localScale = new Vector3(-1f, 1f, 1f); 
    }
}
