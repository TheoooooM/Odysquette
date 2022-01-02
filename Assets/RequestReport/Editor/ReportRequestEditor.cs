using System;
using ReportRequest;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace ReportRequestEditor
{
    [CustomEditor(typeof(ReportRequestManager))]
    public class ReportRequestEditor : Editor
    {
        private ReportRequestManager SO;
        private Editor gridMainPanelCurrentEditor;

        private GUIStyle title = new GUIStyle();
        
       private bool is2Dview;
        private bool InUpdateGrid;
        private Vector3 previousPositionCamera;

        private Color backgroundColor = new Color(0, 0, 0,0.5f);
        private Color buttonColor = new Color(1.2f, 1.4f, 2, 1);
        public override void OnInspectorGUI()
        {
            title.fontSize = 15;
            title.fontStyle = FontStyle.Bold;
            title.normal.textColor = Color.white;
            
            EditorGUIUtility.labelWidth = 125;
            serializedObject.Update();
            GUI.backgroundColor = backgroundColor;
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                  SO = (ReportRequestManager) serializedObject.targetObject;
                          
                            GUILayout.Label("Buttons", title);
                            EditorGUILayout.Space(8f);
                            GUI.backgroundColor = buttonColor;
                            if (Application.isPlaying)
                                GUI.enabled = false;
                            if (Application.isPlaying)
                                GUI.enabled = false;
                            if (SO.token.Length == 0 && SO.key.Length == 0)
                                GUI.enabled = false;
                            if (GUILayout.Button("Get ID"))
                            {
                                JsonWindow.OpenJsonWindow();
                            }
                            if(SO.token.Length == 0 && SO.key.Length == 0)
                                GUI.enabled = true;
                            if (GUILayout.Button("Update Type Request"))
                            {
                                AddButtonType(SO);
                            }
                
                            if (InUpdateGrid)
                                GUI.enabled = false;
                            if (GUILayout.Button("Update GridLayout"))
                            {
                                
                                SO.reportRequestPanel.SetActive(true);
                                InUpdateGrid = true;
                                SceneView.lastActiveSceneView.pivot = previousPositionCamera;
                    
                                SceneView.lastActiveSceneView.pivot  = new Vector3(SO.reportRequestPanel.transform.position.x,
                                    SO.reportRequestPanel.transform.position.y, SO.reportRequestPanel.transform.position.z + 10f);
                                SceneView.lastActiveSceneView.Repaint();
                            }
                            if (InUpdateGrid)
                            GUI.enabled = true;
                            if (!InUpdateGrid)
                            {
                            
                                    GUI.enabled = false;
                            }
                            if (GUILayout.Button("Cancel UpdateGridLayout"))
                            {
                                InUpdateGrid = false;
                                  SO.reportRequestPanel.SetActive(false);
                                
                                  SceneView.lastActiveSceneView.pivot = previousPositionCamera;
                                  previousPositionCamera = Vector3.zero;
                                  SceneView.lastActiveSceneView.Repaint();
                            }
                            if (!InUpdateGrid)
                            GUI.enabled = true;
                               if (Application.isEditor)
                                            GUI.enabled = true;
                               GUI.enabled = true;
                               EditorGUILayout.Space(8f);
                               using( new GUILayout.VerticalScope(EditorStyles.helpBox))
                               {
                                   GUILayout.Label("Trello", title);
                                   EditorGUILayout.Space(8f);

                                   EditorGUILayout.PropertyField(serializedObject.FindProperty("key"));
                                   EditorGUILayout.Space(2f);
                                   EditorGUILayout.PropertyField(serializedObject.FindProperty("token"));
                                   EditorGUILayout.Space(2f);
                                   EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultNameScreenshot"),
                                       new GUIContent("Screenshot Name"));
                                   EditorGUILayout.Space(2f);
                                   EditorGUILayout.PropertyField(serializedObject.FindProperty("typeReportRequestList"));
                                   EditorGUILayout.Space(2f);
                               }
                               EditorGUILayout.Space(8f);
                               using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                               {
                                   GUILayout.Label("UI", title);
                                   EditorGUILayout.Space(8f);
                               if(gridMainPanelCurrentEditor == null)
                            gridMainPanelCurrentEditor = CreateEditor(SO.gridLayoutMainPanel);
                            DrawFoldoutInspector(SO.gridLayoutMainPanel, ref gridMainPanelCurrentEditor);
                               }
                               serializedObject.ApplyModifiedProperties();
            }

        }

        public void AddButtonType(ReportRequestManager SO)
        {
            if (SO.typeReportRequestList.Length != 0)
            {
                int count = SO.buttonTypeRequestPanel.transform.childCount;

                for (int i = 0; i < count; i++)
                {
                    DestroyImmediate(SO.buttonTypeRequestPanel.transform.GetChild(0).gameObject);
                }

                for (int i = 0; i < SO.typeReportRequestList.Length; i++)
                {
                    GameObject currentObj = Instantiate(SO.templateButtonTypeRequest, SO.buttonTypeRequestPanel.transform);
                    currentObj.GetComponent<Image>().sprite = SO.typeReportRequestList[i].typeRequestButtonRenderer;
                    
                     
                    UnityAction<int> action = SO.OpenRequestReportSecondaryPanel;
                    UnityEventTools.AddIntPersistentListener(currentObj.GetComponent<Button>().onClick, action, i);
                    


                    currentObj.GetComponentInChildren<TextMeshProUGUI>().text =
                        SO.typeReportRequestList[i].typeRequestButtonText;
                    currentObj.name = SO.typeReportRequestList[i].typeRequestName + " Button";
                }

                Debug.Log(
                    "For change  the position and the size of each Button, modify the component Grid Layout Group");
            }
            else
            {
                Debug.Log("Error, you must to assign in inspector the typeReportRequestList");
            }

        }

        private void OnDisable()
        {
          
               try
               {
 serializedObject.Update();
                  SO = (ReportRequestManager) serializedObject.targetObject;
                           if (SO.reportRequestPanel.activeSelf)
                           {
                               SO.reportRequestPanel.SetActive(false);
                           }

                           serializedObject.ApplyModifiedProperties();
               }
               catch (Exception e)
               {
                   
               }
              
           
         
        }
    }
    
    }

