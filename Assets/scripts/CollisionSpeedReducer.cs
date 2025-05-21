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
                Debug.Log("BoxCollider ajouté et configuré automatiquement");
            }
            else
            {
                // Configuration par défaut si pas de Renderer
                boxCollider.center = Vector3.zero;
                boxCollider.size = new Vector3(2.0f, 1.0f, 2.0f);
                Debug.Log("BoxCollider ajouté avec configuration par défaut");
            }
        }
    }
    
    // Cette méthode est déclenchée quand la Table est touchée par un autre objet
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision détectée avec " + collision.gameObject.name);
        ApplySlowdown(collision.gameObject);
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
                Debug.Log("CollisionDetector ajouté au joueur");
            }
            else
            {
                CollisionDetector detector = player.GetComponent<CollisionDetector>();
                if (!detector.collisionHandlers.Contains(this))
                {
                    detector.collisionHandlers.Add(this);
                    Debug.Log("Cette table ajoutée aux handlers du joueur");
                }
            }
        }
    }
    
    // Ralentir le joueur s'il a un ThirdPersonController
    public void ApplySlowdown(GameObject playerObject)
    {
        Debug.Log("Tentative de ralentissement pour " + playerObject.name);
        
        // Chercher le ThirdPersonController
        ThirdPersonController controller = playerObject.GetComponent<ThirdPersonController>();
        
        if (controller == null)
        {
            controller = playerObject.GetComponentInParent<ThirdPersonController>();
        }
        
        if (controller != null)
        {
            Debug.Log("ThirdPersonController trouvé, application du ralentissement");
            // Réduire la vitesse
            float originalSpeed = controller.MoveSpeed;
            float newSpeed = originalSpeed * (1 - slowFactor);
            controller.MoveSpeed = newSpeed;
            
            // Restaurer la vitesse originale après un délai
            StartCoroutine(RestoreSpeed(controller, originalSpeed));
        }
        else
        {
            Debug.LogWarning("Aucun ThirdPersonController trouvé sur " + playerObject.name);
        }
    }
    
    // Coroutine pour restaurer la vitesse après un délai
    private System.Collections.IEnumerator RestoreSpeed(ThirdPersonController controller, float originalSpeed)
    {
        yield return new WaitForSeconds(slowDuration);
        if (controller != null)
        {
            controller.MoveSpeed = originalSpeed;
            Debug.Log("Vitesse originale restaurée");
        }
    }
}

// Classe pour détecter les collisions du CharacterController avec les objets
public class CollisionDetector : MonoBehaviour
{
    public System.Collections.Generic.List<CollisionSpeedReducer> collisionHandlers = new System.Collections.Generic.List<CollisionSpeedReducer>();
    
    private void Start()
    {
        Debug.Log("CollisionDetector démarré");
    }
    
    // Cet événement est appelé quand le CharacterController heurte un collider
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Vérifier si l'objet touché a un CollisionSpeedReducer
        CollisionSpeedReducer reducer = hit.collider.GetComponent<CollisionSpeedReducer>();
        
        if (reducer != null)
        {
            Debug.Log("CollisionSpeedReducer trouvé directement sur l'objet touché");
            reducer.ApplySlowdown(gameObject);
        }
        
        // Alternativement, vérifier dans notre liste de handlers connus
        foreach (var handler in collisionHandlers)
        {
            if (hit.gameObject == handler.gameObject)
            {
                Debug.Log("Objet trouvé dans la liste des handlers connus");
                handler.ApplySlowdown(gameObject);
                break;
            }
        }
    }
}