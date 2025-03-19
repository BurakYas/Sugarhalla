using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerControlScripts : MonoBehaviour
{
    // Küp kontrol ve hareketleri için deneme script dosyasıdır.
 
    [Header("References")]
    private Rigidbody _rigidBody;
    
    [Header("Movement Settings")]
    [SerializeField] private float _rollingSpeed = 5;
    private bool _isMoving;

    [Header("Jumping Settings")]
    
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private float _jumpForce;
    [SerializeField] private bool _canJump;

    [SerializeField] private float _jumpCoolDown;

    [Header("Ground Check Settings")]

    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;
   

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        
    }
    void Update()
    {
        if(_isMoving) return;
        if(Input.GetKey(KeyCode.A)) Assemble(Vector3.left);
        if(Input.GetKey(KeyCode.D)) Assemble(Vector3.right);
        if(Input.GetKey(KeyCode.W)) Assemble(Vector3.forward);
        if(Input.GetKey(KeyCode.S)) Assemble(Vector3.back);
        

        void Assemble(Vector3 dir)
        {
         var anchor = transform.position +(Vector3.down + dir)* 0.5f;
            var axis = Vector3.Cross(Vector3.up, dir);
            StartCoroutine(Roll(anchor,axis));
        }
    }
    void FixedUpdate()
    {
        SetInputs();
    }

    private void SetInputs()
    {
        if(Input.GetKey(_jumpKey) && _canJump && onGround())
        {
            // Zıplama İşlemi Gerçekleşecek
            _canJump = false;
            PlayerJump();
            Invoke(nameof(ResetJumping), _jumpCoolDown);
        }
    }

    private void PlayerJump()
    {
        _rigidBody.linearVelocity = new Vector3(_rigidBody.linearVelocity.x, 0f, _rigidBody.linearVelocity.z);
        _rigidBody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJumping()
    {
        _canJump = true;
    }
    private bool onGround()
    {
      return Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundLayer);
    }

    IEnumerator Roll(Vector3 anchor,Vector3 axis)
    {
        _isMoving = true;
         for (int i = 0; i < (90 / _rollingSpeed); i++)
         {
            transform.RotateAround(anchor, axis, _rollingSpeed);
            yield return new WaitForSeconds(0.01f);
         }
        _isMoving = false;
    }
}
