using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    public void EnableForSeconds(int secs)
    {
       StartCoroutine(EnableForSecondsCoroutine(secs));
    }
    public void DisableForSeconds(int secs)
    {
        StartCoroutine(DisableForSecondsCoroutine(secs));
    }

    private IEnumerator EnableForSecondsCoroutine(int secs)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(secs);
        gameObject.SetActive(false);
    }
    private IEnumerator DisableForSecondsCoroutine(int secs)
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(secs);
        gameObject.SetActive(true);
    }

    public void DisableAndMove(Transform pos)
    {
        StartCoroutine(DisableAndMoveCoroutine(pos));
    }

    IEnumerator DisableAndMoveCoroutine(Transform pos)
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        transform.position = pos.position;
        gameObject.SetActive(true);
    }

    public void MoveToPos(Transform pos)
    {
        transform.position = pos.position;
    }
}
