
using UnityEngine;
using UnityEngine.UI;

public class PlayerBoostUI : MonoBehaviour
{
    [Header("UI Settings")]
    public Image boostBarFill;

    [Header("Boost Reference")]
    public PlayerBoost playerBoost;

    void Update()
    {
        UpdateBoostBar();
    }

    // Atualiza a barra de acordo com o estado do boost.
    void UpdateBoostBar()
    {
        // Durante o boost, a barra permanece cheia.
        if (playerBoost.IsBoosting)
        {
            boostBarFill.fillAmount = 1f;
            return;
        }

        // Se não há cooldown, o boost está disponível.
        if (playerBoost.CooldownRemaining <= 0f)
        {
            boostBarFill.fillAmount = 1f;
            return;
        }

        // Calcula quanto do cooldown já passou.
        float progress = 1f -
            (playerBoost.CooldownRemaining /
             playerBoost.cooldownDuration);

        // Atualiza o preenchimento da barra.
        boostBarFill.fillAmount = Mathf.Clamp01(progress);
    }
}
