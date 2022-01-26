using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Collision with Friendly");
                break;
            case "Finish":
                Debug.Log("Collision with Finish");
                break;
            case "Fuel":
                Debug.Log("Collision with Fuel");
                break;
            default:
                Debug.Log("Collision with other object");
                break;
        }
    }
}
