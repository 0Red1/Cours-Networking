using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAsset;
    [SerializeField] private CharacterMovementController characterMovementController;
    [SerializeField] private CharacterSkillController skillController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputAsset.Enable();
        InputActionMap m = inputAsset.FindActionMap("Player");
        InputAction move = m.FindAction("Move");
        move.started += moveCallback;
        move.performed += moveCallback;
        move.canceled += moveCallback;

        m = inputAsset.FindActionMap("Skill");
        InputAction skill1 = m.FindAction("Skill1");
        skill1.started += ctx => skillCallback(ctx, "skill1");
        
        InputAction skill2 = m.FindAction("Skill2");
        skill2.started += ctx => skillCallback(ctx, "skill2");
    }

    private void moveCallback(InputAction.CallbackContext obj)
    {
        characterMovementController.Move(obj.ReadValue<Vector2>());
    }
    
    private void skillCallback(InputAction.CallbackContext obj, string skillName)
    {
        skillController.startSkill(skillName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
