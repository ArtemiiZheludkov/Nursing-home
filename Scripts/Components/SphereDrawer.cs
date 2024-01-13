using UnityEngine;

public class SphereDrawer : MonoBehaviour
{
    public float radius = 1.0f;
    public Color color = Color.white;

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    
	[ContextMenu("Clear Player Prefs")]
    void DoSomething()
    {
        PlayerPrefs.DeleteAll();
    }
}