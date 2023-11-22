using System;
using System.Linq;
using UnityEngine;

namespace Narative
{
    public class ReadJsonNara : MonoBehaviour
    {
        [SerializeField] private TextAsset jsonFile;
        public AllDialogs allDialogs = new AllDialogs();
        
        [Serializable] public class AllDialogs
        {
            public Dialog[] dialog;

            public Dialog GetDialog(string id)
            {
                return dialog.First(d => d.id == id);
            }
        }
        
        [Serializable] public class Dialog
        {
            public string id;
            public string title;
            public string dialogTxt;
        }
        
        [ContextMenu("Read Json")]
        public void ReadJson()
        {
            allDialogs = new AllDialogs();
            allDialogs = JsonUtility.FromJson<AllDialogs>(jsonFile.text);
        }
    }
}