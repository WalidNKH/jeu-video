using UnityEngine;
using StarterAssets;
using System.Collections;

/// <summary>
/// Zone qui donne un boost de vitesse temporaire au joueur
/// Se d\u00e9sactive temporairement apr\u00e8s utilisation puis r\u00e9appara\u00eet
/// </summary>
public class SpeedBoostZone : MonoBehaviour
{
    [Tooltip("Multiplicateur de vitesse appliqu\u00e9 (ex: 1.5 = 50% plus rapide)")]
    public float speedMultiplier = 1.5f;
    
    [Tooltip("Dur\u00e9e du boost en secondes")]
    public float boostDuration = 5.0f;
    
    [Tooltip("Temps avant que la potion r\u00e9apparaisse")]
    public float respawnTime = 10.0f;
    
    private bool isRespawning = false;
    private MeshRenderer meshRenderer;
    private Collider potionCollider;
    
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        potionCollider = GetComponent<Collider>();
        
        // S'assurer qu'on a un collider trigger
        if (potionCollider == null)
        {
            potionCollider = gameObject.AddComponent<SphereCollider>();
        }
        potionCollider.isTrigger = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isRespawning) return;
        
        if (other.CompareTag("Player"))
        {
            ApplySpeedBoost(other.gameObject);
            StartCoroutine(RespawnPotion());
        }
    }
    
    /// <summary>
    /// Applique le boost de vitesse au joueur
    /// </summary>
    private void ApplySpeedBoost(GameObject player)
    {
        ThirdPersonController controller = player.GetComponent<ThirdPersonController>();
        if (controller != null)
        {
            StartCoroutine(BoostPlayerSpeed(controller));
        }
    }
    
    /// <summary>
    /// Coroutine qui applique le boost temporaire
    /// </summary>
    private IEnumerator BoostPlayerSpeed(ThirdPersonController controller)
    {
        float originalSpeed = controller.MoveSpeed;
        float boostedSpeed = originalSpeed * speedMultiplier;
        
        // Appliquer le boost
        controller.MoveSpeed = boostedSpeed;
        
        // Attendre la dur\u00e9e du boost
        yield return new WaitForSeconds(boostDuration);
        
        // Restaurer la vitesse originale
        if (controller != null)
        {
            controller.MoveSpeed = originalSpeed;
        }
    }
    
    /// <summary>
    /// Coroutine pour faire r\u00e9appara\u00eetre la potion
    /// </summary>
    private IEnumerator RespawnPotion()
    {
        isRespawning = true;
        
        // Masquer la potion
        if (meshRenderer != null) meshRenderer.enabled = false;
        if (potionCollider != null) potionCollider.enabled = false;
        
        // Attendre le temps de respawn
        yield return new WaitForSeconds(respawnTime);
        
        // R\u00e9afficher la potion
        if (meshRenderer != null) meshRenderer.enabled = true;
        if (potionCollider != null) potionCollider.enabled = true;
        
        isRespawning = false;
    }
}