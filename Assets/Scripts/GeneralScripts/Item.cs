using UnityEngine;

[RequireComponent(typeof(Hover))]
public class Item : MonoBehaviour
{
    [SerializeField] ProgressEnum Progress;
    [SerializeField] GameObject ItemGetParticle;

    Hover m_hover;

    void Start() {
        m_hover = GetComponent<Hover>();
    }

    void Get() {
        SaveSystem.SetProgress(Progress, true);
        Instantiate(ItemGetParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(m_hover.IsHovering && other.CompareTag("Player")){
            Get();
        }
    }
}
