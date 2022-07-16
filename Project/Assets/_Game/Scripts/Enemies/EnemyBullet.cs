using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float _bulletSpeed;
    // Start is called before the first frame update

    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
       // rb.velocity = Vector3.forward * _bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
