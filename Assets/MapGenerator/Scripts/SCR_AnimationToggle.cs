using UnityEngine;

public class SCR_AnimationToggle: MonoBehaviour
{
    // Turn off animation of this tile if player won't see it

    SpriteRenderer[] renderers;
    Animator animator;

    private float timer;
    private float interval = 2f;

    public float maxOrthographicSize = 17f;

    void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();

        timer = Random.value * 2; // Ensure all objects won't update at once
    }

    void LateUpdate()
    {
        // if camera is too far turn off animations
        if (Camera.main.orthographicSize > maxOrthographicSize)
        {
            animator.enabled = false;
            return;
        }

        // Check only once per interval seconds
        timer += Time.deltaTime;
        if (timer < interval)
        {
            return;
        }
        timer = 0f;


        bool isVisible = false;
        foreach (SpriteRenderer renderer in renderers)
        {
            if (renderer.isVisible)
            {
                isVisible = true;
                break;
            }
        }
        animator.enabled = isVisible;
    }
}
