using UnityEngine;
using StarterAssets;

public class SpeedObstacle : MonoBehaviour
{
    [Tooltip("Pourcentage de ralentissement (entre 0 et 1, ex: 0.2 = 20% de ralentissement)")]
    public float slowFactor = 0.2f;
    
    public void OnTriggerEnter(Collider other)
    {
        // Chercher le ThirdPersonController
        ThirdPersonController controller = other.GetComponent<ThirdPersonController>();

        Debug.Log("controller");
        
        if (controller == null)
        {
            controller = other.GetComponentInParent<ThirdPersonController>();
        }
        
        if (controller != null)
        {
            // Enregistrer la vitesse actuelle avant de la modifier
            controller.gameObject.AddComponent<SpeedMemory>().SetOriginalSpeed(controller.MoveSpeed);
            
            // Réduire la vitesse
            float newSpeed = controller.MoveSpeed * (1 - slowFactor);
            controller.MoveSpeed = newSpeed;
            
            Debug.Log($"Ralentissement: Vitesse réduite à {newSpeed}");
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        // Chercher le ThirdPersonController
        ThirdPersonController controller = other.GetComponent<ThirdPersonController>();
        if (controller == null)
        {
            controller = other.GetComponentInParent<ThirdPersonController>();
        }
        
        if (controller != null)
        {
            // Récupérer et restaurer la vitesse originale
            SpeedMemory memory = controller.GetComponent<SpeedMemory>();
            if (memory != null)
            {
                controller.MoveSpeed = memory.originalSpeed;
                Destroy(memory);
                
                Debug.Log($"Vitesse restaurée à {controller.MoveSpeed}");
            }
        }
    }
}

// Classe auxiliaire pour stocker la vitesse originale
public class SpeedMemory : MonoBehaviour
{
    public float originalSpeed;
    
    public void SetOriginalSpeed(float speed)
    {
        originalSpeed = speed;
    }
}