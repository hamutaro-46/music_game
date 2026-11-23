using UnityEngine;

public class NoteMover : MonoBehaviour
{
    private float speed = 5f; // 落下速度
    private float destroyY = -3f; // ノーツが消えるY座標
    
    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}
