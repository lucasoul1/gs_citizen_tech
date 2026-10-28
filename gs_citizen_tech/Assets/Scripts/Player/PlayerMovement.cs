
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDuration = 0.15f;

    [Header("Collision Settings")]
    public LayerMask obstacleLayer;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private bool isMoving;

    // Prepara os componentes que serão usados pelo movimento.
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Lê os comandos do jogador enquanto ele não está se movendo.
    void Update()
    {
        if (isMoving)
            return;

        Vector2 direction = GetMovementDirection();

        // Inicia o movimento quando uma direção é pressionada.
        if (direction != Vector2.zero)
            StartCoroutine(MoveOneCell(direction));
    }

    // Retorna a direção pressionada, sem permitir diagonais.
    Vector2 GetMovementDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Dá prioridade ao movimento horizontal.
        if (horizontal != 0)
            return new Vector2(Mathf.Sign(horizontal), 0);

        if (vertical != 0)
            return new Vector2(0, Mathf.Sign(vertical));

        // Retorna zero quando nenhuma direção é pressionada.
        return Vector2.zero;
    }

    // Verifica se existe uma parede no caminho do robô.
    bool IsObstacleAhead(Vector2 direction)
    {
        Vector2 checkSize = boxCollider.bounds.size * 0.7f;

        RaycastHit2D hit = Physics2D.BoxCast(
            rb.position,
            checkSize,
            0f,
            direction,
            1f,
            obstacleLayer
        );

        return hit.collider != null;
    }

    // Move o robô uma célula na direção escolhida.
    IEnumerator MoveOneCell(Vector2 direction)
    {
        isMoving = true;

        // Se houver um obstáculo, cancela o movimento.
        if (IsObstacleAhead(direction))
        {
            isMoving = false;
            yield break;
        }

        Vector2 startPosition = rb.position;
        Vector2 targetPosition = startPosition + direction;

        // Guarda a duração deste passo antes de começar.
        float stepDuration = moveDuration;
        float elapsedTime = 0f;

        while (elapsedTime < stepDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(
                elapsedTime / stepDuration
            );

            MovePlayer(startPosition, targetPosition, progress);

            yield return null;
        }

        // Conclui o movimento na posição da próxima célula.
        rb.MovePosition(targetPosition);

        yield return new WaitForFixedUpdate();

        isMoving = false;
    }

    // Calcula e aplica a posição intermediária do movimento.
    void MovePlayer(
        Vector2 startPosition,
        Vector2 targetPosition,
        float progress)
    {
        Vector2 newPosition = Vector2.Lerp(
            startPosition,
            targetPosition,
            progress
        );

        rb.MovePosition(newPosition);
    }
}
