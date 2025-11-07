using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : NetworkBehaviour
{
    #region Variables
    public CharacterMovementController movementController;
    public CharacterAnimationsController animationsController;
    public CharacterSkillsPlayer skillsPlayer;
    #endregion

    protected override void OnNetworkPostSpawn()
    {
        base.OnNetworkPostSpawn();
        Debug.Log(IsOwner);

        if (IsOwner)
        {
            InputController.Instance.SetCharacter(this);
        }
    }

    private void Awake()
    {
        movementController = GetComponent<CharacterMovementController>();
        animationsController = GetComponent<CharacterAnimationsController>();
        skillsPlayer = GetComponent<CharacterSkillsPlayer>();

        movementController.SetManager(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateMovementAnimation(float speed)
    {
        animationsController.SetSpeed(speed);
    }

    public void PlayDashAnim()
    {
        animationsController.SetDash(true);
    }

    public void StopDashAnim()
    {
        animationsController.SetDash(false);
    }
}