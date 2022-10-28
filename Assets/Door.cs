using UnityEngine;

public class Door : MonoBehaviour
{
    private int _doorStateId;
    private float _closedZ;
    
    
    [SerializeField]
    private float _openShiftZ = 2;
    
    [SerializeField]
    private float _openSpeed = 1;


    public int DoorState
    {
        get => _doorStateId;
        set
        {
            if (_doorStateId == value)
            {
                return;
            }

            _doorStateId = value;
            enabled = _doorStateId != 0;
        }
    }

    private void Awake()
    {
        _closedZ = transform.localPosition.z;
    }

    private void Update()
    {
        var position = transform.localPosition;
        var targetX = DoorState switch
        {
            -1 => _closedZ,
            1 => _closedZ + _openShiftZ,
            _ => position.z,
        };
            
        position.z = Mathf.MoveTowards(position.z, targetX, _openSpeed * Time.deltaTime * DoorState);
        transform.localPosition = position;
        if (Mathf.Approximately(position.z, targetX))
        {
            enabled = false;
        }
    }
}
