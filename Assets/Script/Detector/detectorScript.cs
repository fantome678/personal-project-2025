using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class detectorScript : MonoBehaviour
{
    [System.Serializable]
    public class ListIA
    {
        public string name;
        public GameObject list;
    }

    SphereCollider sphereCollider;

    public float rayLength = 30f;  
    public LayerMask collisionLayer;  
    public GameObject radar;  
    public GameObject radarPointPrefab;  
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        ClearRadarPoints();

        

        float rayLength = 30f;

        
        Collider[] colliders = Physics.OverlapSphere(player.position, rayLength);
        // change angle of Radar Detection
        float maxAngle = 360f; 

        foreach (var collider in colliders)
        {
            
            if (collider.gameObject.tag == "IA")
            {
                
                Vector3 directionToObject = collider.transform.position - player.position;


                float angleBetween = Vector3.Angle(player.forward, directionToObject);

              
                if (angleBetween <= maxAngle)
                {

                    CreateRadarPoint(collider.transform.position);
                }
            }
        }
    }




    void CreateRadarPoint(Vector3 objectPosition)
    {
      
        Vector3 radarPosition = WorldToRadarPosition(objectPosition);

      
        GameObject radarPoint = Instantiate(radarPointPrefab, radar.transform);
        radarPoint.transform.localPosition = radarPosition;
    }


    Vector3 WorldToRadarPosition(Vector3 worldPosition)
    {
    
        Vector3 relativePosition = worldPosition - player.position;

        // Appliquer une rotation inverse de l'orientation du joueur
        Quaternion inversePlayerRotation = Quaternion.Euler(0, -player.rotation.eulerAngles.y, 0);  // Rotation inversée

        // Appliquer la rotation inverse
        Vector3 rotatedPosition = inversePlayerRotation * relativePosition;


        float radarScale = radar.transform.localScale.x / 0.5f;

        // Calculer la position radar en utilisant les axes X et Z
        float x = rotatedPosition.x / rayLength * radarScale;
        float y = rotatedPosition.z / rayLength * radarScale;

        
        return new Vector3(x, y, 0); // Position finale sur le radar
    }

    // Effacer les anciens points du radar à chaque mise à jour
    void ClearRadarPoints()
    {
        // Effacer tous les points enfants du radar
        foreach (Transform child in radar.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
