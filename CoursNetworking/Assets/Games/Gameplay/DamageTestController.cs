using Unity.Netcode;
using UnityEngine;

public class DamageTestController : MonoBehaviour
{
    // Renseignez l'ID du joueur à cibler (0 pour l'Hôte/J1, 1 pour le Client/J2)
    [field: SerializeField] public ulong TargetClientId { get; set; } = 0;

    // Les dégâts à appliquer à chaque clic
    [SerializeField] private float testDamage = 10f;

    public void OnDamageButtonClicked()
    {
        // Vérifie si le NetworkManager est actif et que l'on n'est pas en mode déconnecté
        if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsListening)
        {
            Debug.LogError("[TEST] Le réseau n'est pas actif !");
            return;
        }

        // 1. Trouver le Character cible via le PlayerManager
        Character targetCharacter = PlayerManager.Instance.GetCharacterByClientId(TargetClientId);

        if (targetCharacter != null && targetCharacter.healthSystem != null)
        {
            // 2. Appeler le ServerRpc sur le HealthSystem du joueur cible
            targetCharacter.healthSystem.TakeDamageServerRPC(testDamage);
            Debug.Log($"[TEST] Demande de dégâts ({testDamage}) envoyée pour le joueur ID: {TargetClientId}");
        }
        else
        {
            Debug.LogWarning($"[TEST WARNING] Personnage ou HealthSystem introuvable pour l'ID: {TargetClientId}.");
        }
    }
}
