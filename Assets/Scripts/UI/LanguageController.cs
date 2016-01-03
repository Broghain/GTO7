using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

//thx Tom
public class LanguageController : MonoBehaviour
{
    private List<TextTranslation> textFields = new List<TextTranslation>();
    private string currentLanguage;
    private string defaultLanguage = "en";

	private Hashtable textStrings = new Hashtable();

	// Use this for initialization
	void Start ()
	{
		string savedLanguage = PlayerPrefs.GetString ("Language", "en");
        SetLanguage(savedLanguage);
	}

	public void UpdateTextfields()
	{
        foreach (TextTranslation text in textFields)
        {
            text.UpdateLanguage();
        }
	}
	
	public void SetLanguage(string language)
	{
		textStrings.Clear ();

		currentLanguage = language;

		string BasePath = "Assets/Resources/Language/";

		string PathText = BasePath + currentLanguage + "_Text.xml";

		if (!System.IO.File.Exists(PathText))
		{
			PathText = BasePath + defaultLanguage + "_Text.xml"; 
		}

		ReadXml (PathText, textStrings);

		UpdateTextfields ();
		PlayerPrefs.SetString("Language", language);
	}
	
    void ReadXml(string path, Hashtable table)
	{
		XmlDocument xml = new XmlDocument();
		xml.Load(path);

		XmlElement baseNode = xml.DocumentElement;

		for (int index = 0; index < baseNode.ChildNodes.Count; index++)
		{
			if (baseNode.ChildNodes[index].Name != "#comment")
			{
				table.Add(baseNode.ChildNodes[index].Name, baseNode.ChildNodes[index].InnerText);
			}
		}
	}
	public string GetLanguage()
	{
		return currentLanguage;
	}
	public string GetTextWithKey(string key)
	{
		string text = "";

		if (textStrings.ContainsKey(key) && key != "")
		{
			text = textStrings[key].ToString();
		}

		return text;
	}

	public void AddTranslateField(TextTranslation textField)
	{
		textFields.Add (textField);
	}
}
