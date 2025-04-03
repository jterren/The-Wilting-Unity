using System;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class ResolveSprite : MonoBehaviour
{
    public Animator animator;
    public SpriteResolver resolver; // Reference to Sprite Resolver
    public SpriteRenderer spriteRenderer;
    private string category;
    public int baseLayer;
    private int defaultLayer;
    public bool layerChange;
    public string prefix = "";

    void Start()
    {
        animator = transform.parent.transform.parent.GetComponentInChildren<Animator>();
        resolver = GetComponent<SpriteResolver>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        category = resolver.GetCategory();
        defaultLayer = spriteRenderer.sortingOrder;
    }

    void FixedUpdate()
    {
        if (animator.GetBool("Vertical"))
        {
            resolver.SetCategoryAndLabel(category, $"{prefix}Up");
            if (layerChange) spriteRenderer.sortingOrder = defaultLayer - baseLayer;
        }
        else
        {
            resolver.SetCategoryAndLabel(category, $"{prefix}Down");
            if (layerChange) spriteRenderer.sortingOrder = defaultLayer;
        }
    }
}
