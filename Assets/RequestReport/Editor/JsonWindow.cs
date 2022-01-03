using System.Collections.Generic;
using MiniJSON;
using ReportRequest;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace ReportRequestEditor
{
    public class JsonWindow : EditorWindow
    {
        private Color backgroundColor = new Color(0, 0, 0,0.5f);
        private Color buttonColor = new Color(1.2f, 1.4f, 2, 1);
        private ReportRequestManager reportRequestManager;
        private const string baseURLBoard = "https://api.trello.com/1/boards/";
        private const string memberBaseUrl = "https://api.trello.com/1/members/me";
        private string key;
        private string token;
        [SerializeField]
        private List<NameWithID> allBoards = new List<NameWithID>();
        [SerializeField]
        private List<NameWithID> allLists = new List<NameWithID>();
        [SerializeField]
        private List<NameWithID> allLabels = new List<NameWithID>();
        [SerializeField] private string boardId;
        private bool fieldRequireIsCompleted;
        private bool operationInProgress;
        Vector2  scrollPosition = Vector2.zero;
        private GUIStyle title = new GUIStyle();
        public static void OpenJsonWindow()
        {

            GetWindow<JsonWindow>("Get ID");
        }

        private void OnEnable()
        {
            reportRequestManager = (ReportRequestManager) FindObjectOfType(typeof(ReportRequestManager));
            key = reportRequestManager.key;
            token = reportRequestManager.token;
        }

        private void OnGUI()
        {
            title.fontSize = 15;
            title.fontStyle = FontStyle.Bold;
            title.normal.textColor = Color.white;
            GUI.backgroundColor = backgroundColor;
            using( new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUI.backgroundColor = buttonColor;
                  EditorGUIUtility.fieldWidth = 130f;
                  GUILayout.Label("Buttons", title);
                            if (operationInProgress)
                            {
                                GUI.enabled = false;
                            }
                            if (GUILayout.Button("Get All Board"))
                            {
                                SearchAllBoard();
                            }
                
                            if (allBoards.Count != 0)
                            {                                
                                EditorGUI.BeginChangeCheck();
                                boardId = EditorGUILayout.TextField("Board ID", boardId);
                                if (EditorGUI.EndChangeCheck())
                                {
                                                                            
                                    if (boardId.Length != 0)
                                        fieldRequireIsCompleted = true;
                                    else
                                    { 
                                        fieldRequireIsCompleted = false;
                                    }
                                }             
                                if (!fieldRequireIsCompleted )
                                {
                                    GUI.enabled = false;
                                }
                                                                        
                                if (GUILayout.Button("Get All List"))
                                {
                                    SearchAllList();
                                }
                                                                         
                                if (GUILayout.Button("Get All Label"))
                                {
                                    SearchAllLabel();
                                }
                                                                         
                                if (!fieldRequireIsCompleted )
                                {
                                    GUI.enabled = true;
                                }
                                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, true, true);
                                GUILayout.Label("All Boards", title);
                                EditorGUILayout.Space(4f);
                                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                                {
                                    for (int i = 0; i < allBoards.Count; i++)
                                    {
                                        using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                                        {
                                            GUILayout.TextField(allBoards[i].name + ": " + allBoards[i].id);
                                        }

                                        EditorGUILayout.Space(2f);
                                    }
                                }

                                EditorGUILayout.Space(6f);
                                GUILayout.Label("All Lists", title);
                                EditorGUILayout.Space(4f);
                              
                                    if (allLists.Count != 0)
                                    { using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                                        {
                                        for (int i = 0; i < allLists.Count; i++)
                                        {
                                            using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                                            {
                                                GUILayout.TextField(allLists[i].name + ": " + allLists[i].id);
                                            }

                                            EditorGUILayout.Space(2f);
                                        }
                                    }
                                }

                                EditorGUILayout.Space(6f);
                                GUILayout.Label("All Labels", title);
                                EditorGUILayout.Space(4f);
                                
                                    if (allLabels.Count != 0)
                                    {
                                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                                        {
                                        for (int i = 0; i < allLabels.Count; i++)
                                        {
                                            using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                                            {
                                                GUILayout.TextField(allLabels[i].name + ": " + allLabels[i].id);
                                            }

                                            EditorGUILayout.Space(2f);
                                        }
                                    }
                                }

                                EditorGUILayout.EndScrollView();
                                
                            }
                            if (operationInProgress)
                            {
                                GUI.enabled = true;
                            }
            }
          
            
        }

        private List<object> searchFinal;
        void SearchAllBoard()
        {
           SearchAll($"{memberBaseUrl}?key={key}&token={token}&boards=all", "boards", allBoards, "name");
               
        }

        private List<object> _list;

        void SearchAllList()
        {
            SearchAll($"{baseURLBoard}{boardId}?key={key}&token={token}&lists=all", "lists", allLists, "name");

        }

        private List<object> _label;

        void SearchAllLabel()
        {
            SearchAll( $"{baseURLBoard}{boardId}?key={key}&token={token}&labels=all", "labels", allLabels, "color");
        }

        public List<object> Populate(string url, string type)
        {
           
            var uwr = UnityWebRequest.Get(url);
            var operation = uwr.SendWebRequest();
            while (!operation.isDone)
            {
                operationInProgress = true;
            }
            if (!(Json.Deserialize(uwr.downloadHandler.text) is Dictionary<string, object> currentDict))
            {
                Debug.LogError("Error, check your token or id are available or retry");
                return null;
            }
            
            return (List<object>) currentDict[type];
        }
 void SearchAll(string url, string type, List<NameWithID> allList, string keyForId)
        {
            searchFinal = Populate(url, type);
            List<string> idList = new List<string>();
            List<string> nameList = new List<string>();
            allList.Clear();
            for (var i = 0; i < searchFinal.Count; i++)
            {
                var search = (Dictionary<string, object>) searchFinal[i];
                foreach (var element in search)
                {
                    if (element.Key == keyForId || element.Key == "id")
                    { 
                        if (element.Key == keyForId)
                        {
                            nameList.Add((string)element.Value);
                                
                        }
                        else
                        {
                            idList.Add((string) element.Value);
                        }
                           
                    }
                  
                }
            }   
            for (int i = 0; i < nameList.Count; i++)
            {
                NameWithID currentNameID = new NameWithID();
                currentNameID.id = idList[i];
                currentNameID.name = nameList[i];
                allList.Add(currentNameID);
            }
            nameList.Clear();
            idList.Clear();
            operationInProgress = false;
        }


    }
}
