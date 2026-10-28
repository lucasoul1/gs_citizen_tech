
using UnityEngine;

public class PlayerBoost : MonoBehaviour
{
    [Header("Boost Settings")]
    public KeyCode boostKey = KeyCode.LeftShift;
    public float boostedMoveDuration = 0.07f;
    public float boostDuration = 1.5f;
    public float cooldownDuration = 3f;

    private PlayerMovement playerMovement;
    private float normalMoveDuration;

    private float boostTimer;
    private float cooldownTimer;

    private bool isBoosting;

    // Informações que a interface pode consultar.
    public bool IsBoosting => isBoosting;
    public float CooldownRemaining => Mathf.Max(0f, cooldownTimer);

    void Awake()
    {
        // Encontra o script de movimentação.
        playerMovement = GetComponent<PlayerMovement>();

        // Guarda a velocidade normal do robô.
        normalMoveDuration = playerMovement.moveDuration;
    }

    void Update()
    {
        // Atualiza o tempo do boost, se estiver ativo.
        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;

            if (boostTimer <= 0f)
                StopBoost();

            return;
        }

        // Atualiza o tempo de recarga.
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        // Ativa o boost ao pressionar a tecla.
        if (Input.GetKeyDown(boostKey))
            StartBoost();
    }

    // Ativa o boost e inicia sua contagem de tempo.
    void StartBoost()
    {
        isBoosting = true;
        boostTimer = boostDuration;

        playerMovement.moveDuration = boostedMoveDuration;
    }

    // Encerra o boost e inicia o cooldown.
    void StopBoost()
    {
        isBoosting = false;

        // Restaura a velocidade normal.
        playerMovement.moveDuration = normalMoveDuration;

        // Inicia o tempo de recarga.
        cooldownTimer = cooldownDuration;
    }

    // Garante que a velocidade seja restaurada ao desativar o objeto.
    void OnDisable()
    {
        if (playerMovement != null)
            playerMovement.moveDuration = normalMoveDuration;
    }
}
