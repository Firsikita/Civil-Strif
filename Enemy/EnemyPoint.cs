using UnityEngine;

public class EnemyPoint : MonoBehaviour
{
    [SerializeField] private string _enemyTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == _enemyTag)
            collision.gameObject.GetComponent<Squishy>().ChangePoint();
    }
}
