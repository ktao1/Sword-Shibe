using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirimeBullet : MonoBehaviour
{
    public int damage;
    public float speed;
    public float lifelength;
    Vector3 dir;
    public void setDir(Vector3 dir)
    {
        this.dir = dir;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) 
            n += 360;
        return n;
    }
    private void Update()
    {
        transform.position += dir *speed * Time.deltaTime;
        Destroy(gameObject, lifelength);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.layer != 13)
        {
            collision.gameObject.SendMessage("takeDamage", damage);
        }

        Destroy(gameObject);
    }

}