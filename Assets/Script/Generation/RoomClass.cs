using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomClass : MonoBehaviour
{
    [Header("----------G�n�rration Proc�durale---------")]
    [SerializeField] bool _OpenTop; //Est ce que la salle est ouverte en haut
    public bool OpenTop => _OpenTop;

    [SerializeField] bool _OpenBottom; //Est ce que la salle est ouverte en bas
    public bool OpenBottom => _OpenBottom;

    [SerializeField] bool _OpenLeft; //Est ce que la salle est ouverte � gauche
    public bool OpenLeft => _OpenLeft;

    [SerializeField] bool _OpenRight; //Est ce que la salle est ouverte � droite
    public bool OpenRight => _OpenRight;

    [SerializeField] bool _Closing; //Est e que la salle a 1 issue et ferme le niveau
    public bool Closing => _Closing;

    [SerializeField] int _NbrExit; //Nombre de sortie de la pi�ce
    public int NbrExit => _NbrExit;

    [Header("----------------IN GAME------------------")]
    [SerializeField] bool IsClosed; //Est ce que la salle est ferm�

}
