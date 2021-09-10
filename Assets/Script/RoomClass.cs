using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomClass : MonoBehaviour
{
    [SerializeField] bool _OpenTop;
    public bool OpenTop => _OpenTop;

    [SerializeField] bool _OpenBottom;
    public bool OpenBottom => _OpenBottom;

    [SerializeField] bool _OpenLeft;
    public bool OpenLeft => _OpenLeft;

    [SerializeField] bool _OpenRight;
    public bool OpenRight => _OpenRight;
}
