using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoxScript : MonoBehaviour
{
    private StringBuilder UserName;
    private static readonly List<char> tabooSigns = new List<char>() {'.',',',' ','\'','\"' };
    private Text errorText;
    private Text text;
    // Start is called before the first frame update
    void Awake()
    {
        UserName = new StringBuilder(string.Empty);
        var output = GameObject.Find("Error Text");
        if(output)
            errorText = output.GetComponent<Text>();
        var input = GameObject.Find("input");
        if (input)
            text = input.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
		
    }
    public void CheckOnError(string name)
	{
        foreach (var item in tabooSigns)
        {
            if (name.Contains(item))
            {
                PrintError();
                return;
            }
        }
        errorText.enabled = false;
    }
    public void UpdateName(string name)
	{
        UserName = new StringBuilder(name);
	}
    private void PrintError()
	{
        if(errorText)
		{
            errorText.text = $"dont use signs these signs ({ListToString<char>(tabooSigns)})";
            errorText.enabled = true;
        }
	}
    private void ClearText()
	{
        if (text)
            text.text = string.Empty;
	}
    private static string ListToString<T>(IEnumerable<T> list)
	{
        StringBuilder text = new StringBuilder(string.Empty);
        foreach (var item in list)
            text.Append(item.ToString() + " ");
		
        return text.ToString();
	}
}
