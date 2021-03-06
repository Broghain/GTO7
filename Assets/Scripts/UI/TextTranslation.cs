﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class TextTranslation : MonoBehaviour {

    [SerializeField]
    private string key;

    private Text textField;

    private LanguageController languageController;

	// Use this for initialization
	void Start () 
    {
        Init();
        if (languageController != null)
        {
            languageController.AddTranslateField(this);
            UpdateLanguage();
        }
	}

    private void Init()
    {
        textField = GetComponent<Text>();
        languageController = FindObjectOfType<LanguageController>();
    }

    public void UpdateLanguage()
    {
        if (languageController == null || textField == null)
        {
            Init();
        }
        textField.text = languageController.GetTextWithKey(key);
    }

    public void SetKey(string key)
    {
        this.key = key;
        UpdateLanguage();
    }
}
