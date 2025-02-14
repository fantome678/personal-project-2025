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
    public LayerMask collisionLayer;  // D�finir un layer pour filtrer les objets
    public GameObject radar;  // Le radar (objet 3D)
    public GameObject radarPointPrefab;  // Le point repr�sentant un objet sur le radar
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
        // Dessiner un rayon pour v�rifier visuellement dans la sc�ne (bas� sur la rotation)
        // Debug.DrawRay(player.position, player.forward * rayLength, Color.red);

        // R�initialiser la position des points de radar
        ClearRadarPoints();

        // Vector3 rayDirection = Quaternion.Euler(0, 80f, 0) * player.forward; // ou

        float rayLength = 30f;

        // V�rifier les objets autour du joueur dans un c�ne de 80 degr�s
        Collider[] colliders = Physics.OverlapSphere(player.position, rayLength);

        float maxAngle = 80f; // Angle maximal du c�ne de d�tection (en degr�s)

        foreach (var collider in colliders)
        {
            // V�rifier si l'objet est une IA
            if (collider.gameObject.tag == "IA")
            {
                // Calculer la direction de l'objet par rapport au joueur
                Vector3 directionToObject = collider.transform.position - player.position;

                // Calculer l'angle entre la direction du joueur et la direction vers l'objet
                // Utiliser la rotation du joueur pour obtenir l'orientation correcte
                float angleBetween = Vector3.Angle(player.forward, directionToObject);

                // V�rifier si l'objet est dans la zone de 80 degr�s par rapport � la direction du joueur
                if (angleBetween <= maxAngle)
                {
                    // L'objet est dans la zone de d�tection

                    // Cr�er un point sur le radar
                    CreateRadarPoint(collider.transform.position);
                }
            }
        }
    }



    // Cr�e un point sur le radar pour chaque objet d�tect�
    void CreateRadarPoint(Vector3 objectPosition)
    {
        // Convertir la position de l'objet dans l'espace 3D en coordonn�es locales sur le radar (2D)
        Vector3 radarPosition = WorldToRadarPosition(objectPosition);

        // Cr�er une sph�re (ou tout autre objet) sur le radar
        GameObject radarPoint = Instantiate(radarPointPrefab, radar.transform);
        radarPoint.transform.localPosition = radarPosition;
    }

    // Convertir les coordonn�es du monde en coordonn�es locales sur le radar (plan 2D)
    Vector3 WorldToRadarPosition(Vector3 worldPosition)
    {
        // Calculer la direction relative au joueur
        Vector3 relativePosition = worldPosition - player.position;

        // Appliquer une rotation inverse de l'orientation du joueur
        Quaternion inversePlayerRotation = Quaternion.Euler(0, -player.rotation.eulerAngles.y, 0);  // Rotation invers�e

        // Appliquer la rotation inverse
        Vector3 rotatedPosition = inversePlayerRotation * relativePosition;

        // Convertir en coordonn�es radar
        float radarScale = radar.transform.localScale.x / 0.5f;

        // Calculer la position radar en utilisant les axes X et Z
        float x = rotatedPosition.x / rayLength * radarScale;
        float y = rotatedPosition.z / rayLength * radarScale;

        // Retourner la position sur le radar
        return new Vector3(x, y, 0); // Position finale sur le radar
    }
    // Convertir les coordonn�es du monde en coordonn�es locales sur le radar (plan 2D)
    /*Vector3 WorldToRadarPosition(Vector3 worldPosition)
    {
        // Calculer la direction par rapport au joueur
        Vector3 relativePosition = worldPosition - player.position;

        // Convertir la position en coordonn�es locales sur le radar
        float radarScale = radar.transform.localScale.x / 2f;

        // Calculer les coordonn�es locales sur le radar
        float x = relativePosition.x / rayLength * radarScale;
        float y = relativePosition.z / rayLength * radarScale;

        //return new Vector3(x, 0, y); // Position sur le plan XY du radar
        return new Vector3(x, y, 0); // Position sur le plan XY du radar
    }*/

    // Effacer les anciens points du radar � chaque mise � jour
    void ClearRadarPoints()
    {
        // Effacer tous les points enfants du radar
        foreach (Transform child in radar.transform)
        {
            Destroy(child.gameObject);
        }
    }


}
