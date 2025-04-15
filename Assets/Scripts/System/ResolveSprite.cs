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
    private bool isPlayer;
    private Transform player;

    void Start()
    {
        isPlayer = transform.root.name == "Player";
        if (!isPlayer) player = Tools.FindGameObjectByName("Player").transform; //If gameObject is not player, will need reference to track position.
        else
        {
            animator = transform.parent.transform.parent.GetComponentInChildren<Animator>();
            resolver = GetComponent<SpriteResolver>();
            category = resolver.GetCategory();
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultLayer = spriteRenderer.sortingOrder;
    }

    void FixedUpdate()
    {
        if (isPlayer)
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
        else
        {
            if (transform.position.y < player.position.y)
            {
                // if (layerChange) spriteRenderer.sortingOrder = defaultLayer + baseLayer;
                if (layerChange) spriteRenderer.sortingLayerName = "SouthPlayer";
            }
            else
            {
                // if (layerChange) spriteRenderer.sortingOrder = defaultLayer;
                if (layerChange) spriteRenderer.sortingLayerName = "NorthPlayer";
            }
        }
    }
}
