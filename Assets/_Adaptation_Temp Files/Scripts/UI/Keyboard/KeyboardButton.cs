using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TevaVR.UI
{
    public delegate void OnClick(string str);
    public class KeyboardButton : MonoBehaviour
    {
        public Button button;
        public TextMeshProUGUI buttonSymbol;
        public RectTransform Rect { get {return GetComponent<RectTransform>(); } }

        public void Init(string _symbol,OnClick _onClick)
        {
            buttonSymbol.text = _symbol;
            button.onClick.AddListener(()=> { _onClick(_symbol); });
        }
    }
}