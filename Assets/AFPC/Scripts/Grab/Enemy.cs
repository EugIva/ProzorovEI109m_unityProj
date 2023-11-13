using UnityEngine;
using System.Collections;
// скрипт для врага, который бежит на игрока
public class Enemy : MonoBehaviour
{

    public Transform player;
    public float move_speed;
    public float rotation_speed;
    public Transform enemy;
    void Update()
    {
        var look_dir = player.position - enemy.position;
        look_dir.y = 0;
        enemy.rotation = Quaternion.Slerp(enemy.rotation, Quaternion.LookRotation(look_dir), rotation_speed * Time.deltaTime);
        enemy.position += enemy.forward * move_speed * Time.deltaTime;
    }
}
