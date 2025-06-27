using UnityEngine;
using StarterAssets;

//Collision sur les objets -> Table par exemple

public class CollisionSpeedReducer : MonoBehaviour
{
    [Tooltip("Pourcentage de ralentissement (entre 0 et 1, ex: 0.3 = 30% de ralentissement)")]
    public float slowFactor = 0.3f;
    
    [Tooltip("Durée du ralentissement en secondes")]
    public float slowDuration = 3.0f;
    
    private BoxCollider boxCollider;
    
    private void Awake()
    {
        // S'assurer qu'il y a un BoxCollider
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
            
            // Configurer automatiquement le collider en fonction de la taille de l'objet
            Renderer renderer = GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                boxCollider.center = renderer.bounds.center - transform.position;
                boxCollider.size = renderer.bounds.size;
            }
            else
            {
                // Configuration par défaut si pas de Renderer
                boxCollider.center = Vector3.zero;
                boxCollider.size = new Vector3(2.0f, 1.0f, 2.0f);
            }
        }
    }
    
    // Cette méthode est déclenchée quand la Table est touchée par un autre objet
    private void OnCollisionEnter(Collision collision)
    {
        ApplySlowdown(collision.gameObject);
    }
    
    // Cette méthode est déclenchée quand un objet entre dans la trigger zone
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[CollisionSpeedReducer] OnTriggerEnter avec {other.gameObject.name} (tag: {other.tag})");
        if (other.CompareTag("Player"))
        {
            ApplySlowdown(other.gameObject);
        }
        else
        {
            Debug.Log($"[CollisionSpeedReducer] Objet ignoré car pas Player: {other.gameObject.name}");
        }
    }
    
    // Il nous faut aussi un script à mettre sur le joueur pour détecter les collisions avec le CharacterController
    private void OnEnable()
    {
        // Créer et attacher automatiquement le script détecteur sur le joueur s'il existe
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && player.GetComponent<CharacterController>() != null)
        {
            if (player.GetComponent<CollisionDetector>() == null)
            {
                CollisionDetector detector = player.AddComponent<CollisionDetector>();
                detector.collisionHandlers.Add(this);
            }
            else
            {
                CollisionDetector detector = player.GetComponent<CollisionDetector>();
                if (!detector.collisionHandlers.Contains(this))
                {
                    detector.collisionHandlers.Add(this);
                }
            }
        }
    }
    
    // Ralentir le joueur s'il a un ThirdPersonController
    public void ApplySlowdown(GameObject playerObject)
    {
        Debug.Log($"[CollisionSpeedReducer] ApplySlowdown appelé sur {playerObject.name}");
        
        // Chercher le ThirdPersonController
        ThirdPersonController controller = playerObject.GetComponent<ThirdPersonController>();
        
        if (controller == null)
        {
            controller = playerObject.GetComponentInParent<ThirdPersonController>();
            Debug.Log($"[CollisionSpeedReducer] Controller trouvé dans parent: {controller != null}");
        }
        
        if (controller != null)
        {
            // Réduire la vitesse
            float originalSpeed = controller.MoveSpeed;
            float newSpeed = originalSpeed * (1 - slowFactor);
            
            Debug.Log($"[CollisionSpeedReducer] Vitesse originale: {originalSpeed}, nouvelle vitesse: {newSpeed}, slowFactor: {slowFactor}");
            
            controller.MoveSpeed = newSpeed;
            
            // Restaurer la vitesse originale après un délai
            StartCoroutine(RestoreSpeed(controller, originalSpeed));
        }
        else
        {
            Debug.Log($"[CollisionSpeedReducer] Aucun ThirdPersonController trouvé sur {playerObject.name}");
        }
    }
    
    // Coroutine pour restaurer la vitesse après un délai
    private System.Collections.IEnumerator RestoreSpeed(ThirdPersonController controller, float originalSpeed)
    {
        Debug.Log($"[CollisionSpeedReducer] Attente de {slowDuration} secondes avant restauration");
        yield return new WaitForSeconds(slowDuration);
        if (controller != null)
        {
            Debug.Log($"[CollisionSpeedReducer] Restauration vitesse à {originalSpeed}");
            controller.MoveSpeed = originalSpeed;
        }
        else
        {
            Debug.Log($"[CollisionSpeedReducer] Controller null lors de la restauration");
        }
    }
}

// Classe pour détecter les collisions du CharacterController avec les objets
public class CollisionDetector : MonoBehaviour
{
    public System.Collections.Generic.List<CollisionSpeedReducer> collisionHandlers = new System.Collections.Generic.List<CollisionSpeedReducer>();
    
    // Cet événement est appelé quand le CharacterController heurte un collider
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Vérifier si l'objet touché a un CollisionSpeedReducer
        CollisionSpeedReducer reducer = hit.collider.GetComponent<CollisionSpeedReducer>();
        
        if (reducer != null)
        {
            reducer.ApplySlowdown(gameObject);
        }
        
        // Alternativement, vérifier dans notre liste de handlers connus
        foreach (var handler in collisionHandlers)
        {
            if (hit.gameObject == handler.gameObject)
            {
                handler.ApplySlowdown(gameObject);
                break;
            }
        }
    }
}