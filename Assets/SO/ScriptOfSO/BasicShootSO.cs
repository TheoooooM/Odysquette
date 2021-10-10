using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "BasicShootSO", menuName = "ShootMode/BasicShootSO", order = 1)]
public class BasicShootSO : StrawSO
{
    [NamedArray("float" )]
   
    public float[] directions;
    public float[] directionParameter;

    public override void OnValidate()
    {
        base.OnValidate();
        
    }

    public override void Shoot(GameManager.Effect effect1, GameManager.Effect effect2, Transform parentBulletTF,MonoBehaviour script, float currentTimeValue = 1 ) 
    {

        if (directionParameter != null)
        {
              for (int i = 0; i < directions.Length; i++)
                    {
                       directions[i] += directionParameter[i] * currentTimeValue;
                    }
        }


        if (isDelay)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 currentBasePosition = new Vector3();
                GameObject bullet = Instantiate(prefabBullet, parentBulletTF.position, parentBulletTF.rotation);
                currentBasePosition = bullet.transform.position;

                Vector3 rotation = Quaternion.Euler(0, directions[i], 0) * parentBulletTF.transform.forward;
                if (basePosition.Length != null)
                {
                    bullet.transform.position += basePosition[i] + basePositionParameter[i] * currentTimeValue;
                    currentBasePosition = bullet.transform.position;
                }
                //save pool

                bullet.GetComponent<Rigidbody>().AddForce(rotation * (speedBullet + speedParameter * currentTimeValue),
                    ForceMode.Force);
                SetParameter(bullet, currentTimeValue, effect1, effect2, currentBasePosition);
            }
        }
        else
        {
           script.StartCoroutine(ShootDelay(effect1, effect2, parentBulletTF, currentTimeValue));
        }
      
    }

 

    public override IEnumerator ShootDelay(GameManager.Effect effect1, GameManager.Effect effect2, Transform parentBulletTF, float currentTimeValue)
    {
        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 currentBasePosition = new Vector3();
            GameObject bullet = Instantiate(prefabBullet, parentBulletTF.position, parentBulletTF.rotation);
            currentBasePosition = bullet.transform.position ;
                            
            Vector3 rotation = Quaternion.Euler(0, directions[i], 0) * parentBulletTF.transform.forward; 
            if (basePosition.Length != null)
            {
                bullet.transform.position += basePosition[i]+basePositionParameter[i]*currentTimeValue;
                currentBasePosition = bullet.transform.position;
            }
            //save pool
                             
            bullet.GetComponent<Rigidbody>().AddForce(rotation*(speedBullet+speedParameter*currentTimeValue), ForceMode.Force);
            SetParameter(bullet, currentTimeValue, effect1, effect2, currentBasePosition);
            yield return new WaitForSeconds(delay + delayParameter * currentTimeValue);
        }
    }

    public class ListoToPopupAttribute : PropertyAttribute
    {
    
        public float[] bonsoir = new float[10] ;
        public string propertyName; 
        public ListoToPopupAttribute(List<float> _bonsoir )
        {
        
            for (int i = 0; i < _bonsoir.Count; i++)
            {
                bonsoir[i] = _bonsoir[i];
            }
        }
    }
    [CustomPropertyDrawer(typeof(ListoToPopupAttribute))]
    public class ListToPopupDrawer : PropertyDrawer
    {
        private int selectedIndex = 0;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ListoToPopupAttribute atb = attribute as ListoToPopupAttribute;
            for (int i = 0; i < atb.bonsoir.Length; i++)
            {
                GUI.color = Random.ColorHSV();
                EditorGUI.FloatField(position, label, atb.bonsoir[i]);
            }
        
            
            
        
     
        }
    
    }
}
