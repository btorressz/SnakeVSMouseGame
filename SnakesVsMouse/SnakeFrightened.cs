using UnityEngine;

public class SnakeFrightened : SnakeBehavior
{
    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer blue;
    public SpriteRenderer white;

    public bool eaten { get; private set; }

    public override void Enable(float duration)
    {
        base.Enable(duration);

        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        Invoke(nameof(Flash), duration / 2f);
    }

    public override void Disable()
    {
        base.Disable();

        body.enabled = true;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    private void Eaten()
    {
        eaten = true;
        snake.SetPosition(snake.home.inside.position);
        snake.home.Enable(duration);

        body.enabled = false;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    private void Flash()
    {
        if (!eaten)
        {
            blue.enabled = false;
            white.enabled = true;
            white.GetComponent<AnimatedSprite>().Restart();
        }
    }

    private void OnEnable()
    {
        blue.GetComponent<AnimatedSprite>().Restart();
        snake.movement.speedMultiplier = 0.5f;
        eaten = false;
    }

    private void OnDisable()
    {
        snake.movement.speedMultiplier = 1f;
        eaten = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            // Find the available direction that moves farthest from mouse
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // If the distance in this direction is greater than the current
                // max distance then this direction becomes the new farthest
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (snake.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            snake.movement.SetDirection(direction);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (enabled) {
                Eaten();
            }
        }
    }

}