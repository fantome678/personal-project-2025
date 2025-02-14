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

    //[SerializeField] List<ListIA> IAList;

    SphereCollider sphereCollider;

    public float rayLength = 30f;  // Longueur du rayon
    public LayerMask collisionLayer;  // Définir un layer pour filtrer les objets
    public GameObject radar;  // Le radar (objet 3D)
    public GameObject radarPointPrefab;  // Le point représentant un objet sur le radar
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        //sphereCollider = GetComponent<SphereCollider>();
        // IAList = new List<ListIA>();
        //player = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Dessiner un rayon pour vérifier visuellement dans la scène (basé sur la rotation)
        // Debug.DrawRay(player.position, player.forward * rayLength, Color.red);

        // Réinitialiser la position des points de radar
        ClearRadarPoints();

        // Vector3 rayDirection = Quaternion.Euler(0, 80f, 0) * player.forward; // ou

        float rayLength = 30f;

        // Vérifier les objets autour du joueur dans un cône de 80 degrés
        Collider[] colliders = Physics.OverlapSphere(player.position, rayLength);

        float maxAngle = 80f; // Angle maximal du cône de détection (en degrés)

        foreach (var collider in colliders)
        {
            // Vérifier si l'objet est une IA
            if (collider.gameObject.tag == "IA")
            {
                // Calculer la direction de l'objet par rapport au joueur
                Vector3 directionToObject = collider.transform.position - player.position;

                // Calculer l'angle entre la direction du joueur et la direction vers l'objet
                // Utiliser la rotation du joueur pour obtenir l'orientation correcte
                float angleBetween = Vector3.Angle(player.forward, directionToObject);

                // Vérifier si l'objet est dans la zone de 80 degrés par rapport à la direction du joueur
                if (angleBetween <= maxAngle)
                {
                    // L'objet est dans la zone de détection

                    // Créer un point sur le radar
                    CreateRadarPoint(collider.transform.position);
                }
            }
        }
    }



    // Crée un point sur le radar pour chaque objet détecté
    void CreateRadarPoint(Vector3 objectPosition)
    {
        // Convertir la position de l'objet dans l'espace 3D en coordonnées locales sur le radar (2D)
        Vector3 radarPosition = WorldToRadarPosition(objectPosition);

        // Créer une sphère (ou tout autre objet) sur le radar
        GameObject radarPoint = Instantiate(radarPointPrefab, radar.transform);
        radarPoint.transform.localPosition = radarPosition;
    }

    // Convertir les coordonnées du monde en coordonnées locales sur le radar (plan 2D)
    Vector3 WorldToRadarPosition(Vector3 worldPosition)
    {
        // Calculer la direction relative au joueur
        Vector3 relativePosition = worldPosition - player.position;

        // Appliquer une rotation inverse de l'orientation du joueur
        Quaternion inversePlayerRotation = Quaternion.Euler(0, -player.rotation.eulerAngles.y, 0);  // Rotation inversée

        // Appliquer la rotation inverse
        Vector3 rotatedPosition = inversePlayerRotation * relativePosition;

        // Convertir en coordonnées radar
        float radarScale = radar.transform.localScale.x / 0.5f;

        // Calculer la position radar en utilisant les axes X et Z
        float x = rotatedPosition.x / rayLength * radarScale;
        float y = rotatedPosition.z / rayLength * radarScale;

        // Retourner la position sur le radar
        return new Vector3(x, y, 0); // Position finale sur le radar
    }
    // Convertir les coordonnées du monde en coordonnées locales sur le radar (plan 2D)
    /*Vector3 WorldToRadarPosition(Vector3 worldPosition)
    {
        // Calculer la direction par rapport au joueur
        Vector3 relativePosition = worldPosition - player.position;

        // Convertir la position en coordonnées locales sur le radar
        float radarScale = radar.transform.localScale.x / 2f;

        // Calculer les coordonnées locales sur le radar
        float x = relativePosition.x / rayLength * radarScale;
        float y = relativePosition.z / rayLength * radarScale;

        //return new Vector3(x, 0, y); // Position sur le plan XY du radar
        return new Vector3(x, y, 0); // Position sur le plan XY du radar
    }*/

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
