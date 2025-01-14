using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{

    public float speed = 5f;
    Vector2 direction;
    

    Rigidbody2D rigidBody;
    Animator animator;

    //Se llama al empezar
    private void Start() 
    {   
        rigidBody = GetComponent<Rigidbody2D>(); //Se obtiene el componente 
        animator = GetComponent<Animator>(); //Se obtiene el componente 
    }

    private void FixedUpdate() 
    {
        //Crea el movimiento segun direccion y velocidad
        Vector2 targetPosition = rigidBody.position + direction * speed * Time.fixedDeltaTime; 
        rigidBody.MovePosition(targetPosition);
    }

    //Se llama tras cada frame
    private void Update() 
    {
        Movement();
        Animations();
    }

    //Funcion de movimientos
    private void Movement() 
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized; //Obtiene la direccion del movimiento atraves de WASD o flechas
    }

    //Funcion de animaciones
    private void Animations() 
    {
        //Si esta quito (IDLE)
        if (direction == Vector2.zero) animator.SetBool("running", false);
        //Si se mueve (RUN)
        else animator.SetBool("running", true);

        // Si se mueve a la serecha (ANIMACION)
        if (direction.x > 0) transform.localScale = new Vector3(1f, 1f, 1f); 
         // Si se mueve a la izquierda (ANIMACION INVERSA)
        else if (direction.x < 0)  transform.localScale = new Vector3(-1f, 1f, 1f); 
    }

}
