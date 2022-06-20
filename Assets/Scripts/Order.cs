using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    [SerializeField] Renderer cardRenderer;
    [SerializeField] string sortingLayerName;

    int originOrder;


    public void SetOrigin(int originOrder)
    {
        this.originOrder = originOrder;
        SetOrder(originOrder);
    }

    public void SetOrder(int order)
    {
        int mulOrder = order * 10;
        cardRenderer.sortingLayerName = sortingLayerName;
        cardRenderer.sortingOrder = mulOrder;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
