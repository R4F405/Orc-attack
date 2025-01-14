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
        //rigidBody.velocity = direction * speed; 
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
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized; //Obtiene el movimiento atraves de WASD
    }

    //Funcion de animaciones
    private void Animations() 
    {

    }

}
