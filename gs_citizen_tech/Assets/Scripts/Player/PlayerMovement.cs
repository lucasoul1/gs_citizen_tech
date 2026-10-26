
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Grid grid;
    public float moveSpeed = 4f;

    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        // Alinha o personagem ao centro da célula inicial.
        Vector3Int cell = grid.WorldToCell(transform.position);
        transform.position = grid.GetCellCenterWorld(cell);

        targetPosition = transform.position;
    }

    void Update()
    {
        // Continua o movimento até a próxima célula.
        if (isMoving)
        {
            MoveToTarget();
        }
        else
        {
            // Só recebe outra direção quando termina de andar.
            CheckInput();
        }
    }

    void CheckInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = Vector3.zero;

        // Prioriza a horizontal para impedir diagonais.
        if (horizontal != 0)
        {
            direction = new Vector3(horizontal, 0, 0);
        }
        else if (vertical != 0)
        {
            direction = new Vector3(0, vertical, 0);
        }

        // Começa a andar se alguma direção foi pressionada.
        if (direction != Vector3.zero)
        {
            StartMove(direction);
        }
    }

    void StartMove(Vector3 direction)
    {
        // Descobre a célula atual e a próxima célula.
        Vector3Int currentCell = grid.WorldToCell(transform.position);

        Vector3Int nextCell = currentCell + new Vector3Int(
            Mathf.RoundToInt(direction.x),
            Mathf.RoundToInt(direction.y),
            0
        );

        targetPosition = grid.GetCellCenterWorld(nextCell);
        isMoving = true;
    }

    void MoveToTarget()
    {
        // Move o personagem até o centro da próxima célula.
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // Confirma que chegou antes de aceitar outra direção.
        if (transform.position == targetPosition)
        {
            isMoving = false;
        }
    }
}




/* 
     Start()	    Alinha o personagem à grade no início.
     Update()	    Decide se continua andando ou recebe uma nova direção.
     CheckInput()	Lê as teclas e impede movimentos diagonais.
     StartMove()	Calcula a próxima célula e define o destino. --> Move()
     MoveToTarget()	Move o personagem até o destino. --> OnMove()
*/
