using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

namespace ReportRequest
{
    public class ReportRequestManager : MonoBehaviour
    {
        #region Variables

        #region TrelloVariable

        private ScreenShotManager screenShotManager;
        public string key;
        public string token;
        private int currentTypeRequestIndex;
        private const string CardBaseUrl = "https://api.trello.com/1/cards/";
        [SerializeField] private string defaultNameScreenshot;
        [SerializeField] private bool withScreenShot;
        public TypeReportRequestClass[] typeReportRequestList;

        #endregion

        #region UI

        [SerializeField] private Button cancelButton;
        [SerializeField] private TextMeshProUGUI titleRequestPanel;
        public Sprite currentScreenShot;
        public bool isOpenReportRequestPanel;
        private const string defaultTitle = "Request Report";
        public GameObject reportRequestPanel;

        #region MainPanel

        [SerializeField] private GameObject reportRequestMainPanel;
        public GridLayoutGroup gridLayoutMainPanel;
        private bool isOpenMainReportRequestPanel;
        public GameObject templateButtonTypeRequest;
        public GameObject buttonTypeRequestPanel;

        #endregion

        #region SecondaryPanel

        [SerializeField] private TMP_InputField[] requireInputFieldList;
        [SerializeField] private GameObject reportRequestSecondaryPanel;

        #endregion

        private Color screenShotEmpty = new Color(0.4509804f, 0.4823529f, 0.8313726f, 0.3f);
        [SerializeField] Button sendRequestButton;
        [SerializeField] private TMP_Dropdown subTypeDropDown;

        [SerializeField] private Image previewScreenShot;
        [SerializeField] private GameObject deleteScreenShot;
        [SerializeField] private GameObject noneScreenShotSavedText;

        #region EndPanel

        [SerializeField] private GameObject reportRequestLoadingPanel;
        private const string loadingDefault = "Request send in progress";
        private const string loadingError = "Error, it can come from your internet connection";
        private const string loadingSuc = "successful loading !";
        [SerializeField] private TextMeshProUGUI loadingText;

        #endregion

        #endregion

        #endregion


        private void Start()
        {
            reportRequestPanel.SetActive(false);

            screenShotManager = GetComponent<ScreenShotManager>();
        }
        
        public void OpenRequestReportPanel()
        {
            if (!screenShotManager.isOpenScreenShotPanel)
            {
                reportRequestPanel.SetActive(true);
                isOpenReportRequestPanel = true;
                Time.timeScale = 0;
                isOpenMainReportRequestPanel = true;
            }
        }


        public void CloseRequestReportPanel()
        {
            if (isOpenMainReportRequestPanel)
            {
                reportRequestPanel.SetActive(false);
                isOpenReportRequestPanel = false;
                Time.timeScale = 1;
            }
            else
            {
                titleRequestPanel.text = defaultTitle;
                reportRequestLoadingPanel.SetActive(false);
                reportRequestSecondaryPanel.SetActive(false);
                reportRequestMainPanel.SetActive(true);
                isOpenMainReportRequestPanel = true;
                currentTypeRequestIndex = -1;
            }
        }


        public void OpenRequestReportSecondaryPanel(int index)
        {
            isOpenMainReportRequestPanel = false;
            reportRequestSecondaryPanel.SetActive(true);
            reportRequestMainPanel.SetActive(false);
            for (int i = 0; i < requireInputFieldList.Length; i++)
            {
                requireInputFieldList[i].text = String.Empty;
            }

            titleRequestPanel.text = typeReportRequestList[index].typeRequestName;
            sendRequestButton.interactable = false;
            subTypeDropDown.options.Clear();

            for (int i = 0; i < typeReportRequestList[index].subTypesList.Length; i++)
            {
                subTypeDropDown.options.Add(new TMP_Dropdown.OptionData());

                subTypeDropDown.options[i].text = typeReportRequestList[index].subTypesList[i].subTypeName;
            }

            CheckScreenShot();
            currentTypeRequestIndex = index;
        }

        void CheckScreenShot()
        {
            if (currentScreenShot != null)
            {
                withScreenShot = true;
                deleteScreenShot.SetActive(true);
                noneScreenShotSavedText.SetActive(false);
                previewScreenShot.sprite = currentScreenShot;
                previewScreenShot.color = Color.white;
            }

            else
            {
                withScreenShot = false;
                deleteScreenShot.SetActive(false);
                noneScreenShotSavedText.SetActive(true);
                previewScreenShot.color = screenShotEmpty;
            }
        }

        public void RemoveScreenShot()
        {
            withScreenShot = false;
            deleteScreenShot.SetActive(false);
            noneScreenShotSavedText.SetActive(true);
            previewScreenShot.sprite = null;
            previewScreenShot.color = screenShotEmpty;
            currentScreenShot = null;
        }

        public void CheckRequireField()
        {
            for (int i = 0; i < requireInputFieldList.Length; i++)
            {
                if (requireInputFieldList[i].text.Length == 0)
                {
                    sendRequestButton.interactable = false;
                    return;
                }
            }

            sendRequestButton.interactable = true;
        }


        public void SendRequest()
        {
            reportRequestLoadingPanel.SetActive(true);
            reportRequestSecondaryPanel.SetActive(false);
            List<IMultipartFormSection> form = new List<IMultipartFormSection>();
            string finalNameRequest =
                $"{subTypeDropDown.options[subTypeDropDown.value].text}: {requireInputFieldList[1].text} ({requireInputFieldList[0].text})";
            string finalDescription = $"Description :{requireInputFieldList[2].text}";
            string finalLabel = typeReportRequestList[currentTypeRequestIndex].subTypesList[subTypeDropDown.value]
                .indexLabelInTrello;
            string finalTrelloList = typeReportRequestList[currentTypeRequestIndex].subTypesList[subTypeDropDown.value]
                .indexListInTrello;

            if (withScreenShot)
            {
               
                byte[] finalScreenshot = currentScreenShot.texture.EncodeToJPG();
                form.Add(new MultipartFormFileSection("fileSource", finalScreenshot, defaultNameScreenshot, ""));
            }

            form.Add(new MultipartFormDataSection("name", finalNameRequest));
            form.Add(new MultipartFormDataSection("desc", finalDescription));
            form.Add(new MultipartFormDataSection("idLabels", finalLabel));
            form.Add(new MultipartFormDataSection("idList", finalTrelloList));
            UnityWebRequest uwr = UnityWebRequest.Post($"{CardBaseUrl}?key={key}&token={token}", form);

            loadingText.text = loadingSuc;
            cancelButton.interactable = false;

            var operation = uwr.SendWebRequest();
            while (!operation.isDone)
            {
                if (uwr.result == UnityWebRequest.Result.ConnectionError ||
                    uwr.result == UnityWebRequest.Result.DataProcessingError || uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    loadingText.text = loadingError;
                    break;
                }
            }


            cancelButton.interactable = true;
        }
    }
}