using UnityEngine;

public class MovementLimmiter : MonoBehaviour
{
    public static MovementLimmiter instance;

    [SerializeField] bool initialCharacterCanMove = true;
    public bool CharacterCanMove;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CharacterCanMove = initialCharacterCanMove;
    }

}
