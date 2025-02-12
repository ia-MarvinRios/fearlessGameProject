using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [HideInInspector] public int _misionId = 0;
    Animator _animator;
    public AnimationClip _idleAnimation;
    public AnimationClip _talkAnimation;
    private GameManager gameManager;


    private void Start()
    {
        gameManager = GameManager.Instance;
        gameObject.tag = "NPC";

        
        _animator = gameObject.AddComponent<Animator>();

        if(_animator != null && _idleAnimation != null)
        {


            //_idleAnimation.wrapMode = WrapMode.Loop;
            _animator.Play(_idleAnimation.name);
        }


        //Debug.Log("NPC: " + gameManager.misiones[_misionId].nombre);
    }


    public void MostrarMision()
    {

        if (gameManager.misiones[_misionId].cerrada)
        {
            Debug.Log("No tengo misiones para ti");
            return;

        }

        if (gameManager.misiones[_misionId].EstaCompleta)
        {
            Vector3 offset = transform.forward * 1f + Vector3.up * 1f;  // 1m adelante y 1m arriba
            Debug.Log("Lo hiciste! he aca tu recompensa");
            gameManager.misiones[_misionId].cerrada = true;
            GameObject recompensa = Instantiate(
            gameManager.misiones[_misionId].recompensa,
            transform.position + offset,  // Clona a 1 metro en la dirección frontal
            Quaternion.identity
        );

            return;
        }


        if (!gameManager.misiones[_misionId].asignada)
        {
            Debug.Log("Tu mision, si quieres aceptarla es ...");
            gameManager.misiones[_misionId].asignada = true;

            if (_talkAnimation != null) {
             //   PlayTalk();
            }


            return;
        }

        if (gameManager.misiones[_misionId].asignada)
        {
            Debug.Log("Lo lamento pero te hace falta para terminar la mision ...");
            return;
        }



    }


    public void PlayTalk()
    {
        //_animator.CrossFade(_talkAnimation.name.ToString(), 0.2f); // Transición suave de 0.2s
        _animator.Play(_talkAnimation.name);
        Invoke(nameof(PlayIdle), _animator.GetCurrentAnimatorStateInfo(0).length); // Espera el tiempo de duración
    }

    public void PlayIdle()
    {
        _animator.CrossFade(_idleAnimation.name, 0.2f);
    }

}
