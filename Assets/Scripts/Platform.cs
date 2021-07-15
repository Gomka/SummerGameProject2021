using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private int durability = 1;
    [SerializeField] private bool isWalkable = true, canChangeHeight = true;
    [SerializeField] private Sprite defaultSprite, brokenSprite;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public int getDurability()
    {
        return this.durability;
    }

    public void setDurability(int newDurability)
    {
        if(newDurability > -1) this.durability = newDurability;
    }

    public void reduceDurability()
    {
        if (durability > 0) durability -= 1;

        spriteRenderer.sprite = getSpriteFromDurability();
    }

    public bool isBroken()
    {
        if (!isWalkable) return true;

        return durability <= 0;
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public bool getCanChangeHeight()
    {
        return canChangeHeight;
    }

    private Sprite getSpriteFromDurability()
    {
        switch (this.durability)
        {
            case 0:
                return brokenSprite;
            default:
                return defaultSprite;
        }
    }
}
