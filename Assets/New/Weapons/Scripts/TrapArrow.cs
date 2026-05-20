using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapArrow : MonoBehaviour
{
    private BoxArrow[] _boxArrows;
    // Start is called before the first frame update
    void Start()
    {
        _boxArrows = GetComponentsInChildren<BoxArrow>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ActivateTrap(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (BoxArrow ba in _boxArrows)
        {
            ba.Attack();
        }
    }
}
