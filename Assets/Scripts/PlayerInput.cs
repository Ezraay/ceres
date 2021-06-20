using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public static Vector2 input;

    [SerializeField] float moveSpeed;
    [SerializeField] float smoothTime;

    Vector3 targetRotation;
    new Rigidbody rigidbody;

    void Update () {
        float horizontalInput = Input.GetAxisRaw ("Horizontal");
        float verticalInput = Input.GetAxisRaw ("Vertical");
        input = new Vector2 (horizontalInput, verticalInput).normalized;

        if (input.x != 0 || input.y != 0)
            targetRotation = new Vector3 (0, Mathf.Rad2Deg * Mathf.Atan2 (input.x, input.y), 0);

        if (targetRotation.y >= 180)
            targetRotation.y -= 360;
        else if (targetRotation.y <= -180)
            targetRotation.y += 360;

        if (rigidbody != null)
            rigidbody.transform.rotation = Quaternion.Lerp (rigidbody.transform.rotation, Quaternion.Euler (targetRotation), smoothTime * Time.deltaTime);
    }

    void FixedUpdate () {
        if (GameManager.showingUI || StateManager.server) return;
        if (rigidbody == null) 
            rigidbody = GameManager.mainPlayer.GetComponent<Rigidbody> ();
        
        if (rigidbody != null)
            rigidbody.velocity = new Vector3 (input.x, 0, input.y) * moveSpeed;
    }

}