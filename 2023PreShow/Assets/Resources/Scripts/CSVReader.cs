using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
 
public class CSVReader
{
	static string SPLIT_RE = @";(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
	static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
	static char[] TRIM_CHARS = { '\"' };
 
	public static Dictionary<string, Dictionary<string, string>> Read(string file)
	{
		var table = new Dictionary<string, Dictionary<string, string>>();
		TextAsset data = Resources.Load (file) as TextAsset;
 
		var lines = Regex.Split (data.text, LINE_SPLIT_RE);
 
		if(lines.Length <= 1) return table;
 
		var header = Regex.Split(lines[0], SPLIT_RE);
		for(var i=1; i < lines.Length; i++) {
 
			var values = Regex.Split(lines[i], SPLIT_RE);
			if (values.Length == 0 ||values[0] == "") continue;
 
			var entry = new Dictionary<string, string>();
			
			for(var j=1; j < header.Length && j < values.Length; j++ ) {
				string value = values[j];
				value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
				object finalvalue = value;
				int n;
				float f;
				if(int.TryParse(value, out n)) {
					finalvalue = n;
				} else if (float.TryParse(value, out f)) {
					finalvalue = f;
				}
				entry[header[j]] = 
					Encoding.Default.GetString(
						Encoding.Default.GetBytes(finalvalue.ToString()));
			}
			table.Add (values[0], entry);
		}
		return table;
	}
}