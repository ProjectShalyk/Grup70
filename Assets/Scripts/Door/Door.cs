using UnityEngine;

public class Door : MonoBehaviour
{
    public DoorSwitch[] doorSwitches;

    SpriteRenderer _spriteRenderer;
    bool _isOpen = false;


    public float gateSpeed = 5f;
    GameObject _gate;
    Vector3 _gateStartPos;
    Vector3 _gateEndPos;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.red;

        _gate = transform.GetChild(0).gameObject;
        _gateStartPos = _gate.transform.position;
        _gateEndPos = _gateStartPos + new Vector3(0, 4, 0);

        for (int i = 0; i < doorSwitches.Length; i++)
        {
            doorSwitches[i].attachedDoor = this;
            doorSwitches[i].index = i;
        }
    }

    void Update()
    {
        if (_isOpen && _gate.transform.position != _gateEndPos)
        {
            Vector3 newPos = Vector3.Lerp(_gate.transform.position, _gateEndPos, gateSpeed * Time.deltaTime);
            _gate.transform.position = newPos;
        }
        else if (!_isOpen && _gate.transform.position != _gateStartPos)
        {
            Vector3 newPos = Vector3.Lerp(_gate.transform.position, _gateStartPos, gateSpeed * Time.deltaTime);
            _gate.transform.position = newPos;
        }

    }


    public void CheckSwitches()
    {
        bool flag = true;
        int count = doorSwitches.Length;

        for (int i = 0; i < doorSwitches.Length; i++)
        {
            if (doorSwitches[i].state == false)
            {
                flag = false;
                count--;
            }
        }
        float colorLerp = (float)count / (float)doorSwitches.Length; 

        Color newColor = Color.Lerp(Color.red, Color.green, colorLerp);
        _spriteRenderer.color = newColor;

        _isOpen = flag;
    }

}
