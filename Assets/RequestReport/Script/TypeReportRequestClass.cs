using System;
using UnityEngine;
namespace ReportRequest
{
    [Serializable]
    public class TypeReportRequestClass
    {
        public string typeRequestName;
      

        [Space]
        
        public SubType[] subTypesList;
  
   
        public Sprite typeRequestButtonRenderer;
        public string typeRequestButtonText;
        [Serializable]
        public class SubType
        {
            public string subTypeName;
            public string indexListInTrello;
            public string indexLabelInTrello;

        }
    }
    
}
