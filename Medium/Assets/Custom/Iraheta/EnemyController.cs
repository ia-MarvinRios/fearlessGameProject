using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float _vida;
    public GameObject _recompensa; 




    public void take_dmg(float _dmg)
    {

        _vida-= _dmg;

        if( _vida < 0)
        {
            _vida = 0;
            Vector3 offset = transform.forward * 1f + Vector3.up * 1f;  // 1m adelante y 1m arriba
            GameObject recompensa = Instantiate(
                        _recompensa,
                        transform.position + offset,  // Clona a 1 metro en la dirección frontal
                        Quaternion.identity
                    );

            Destroy(gameObject);
        }

    }





}
