using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SCCLine
{
	public string questState;
	public string character;
	public string location;
	public string condition1Left;
	public string condition1Comp;
	public object condition1Right;
	public string condition2Left;
	public string condition2Comp;
	public object condition2Right;
	public string effectLeft;
	public string effectOp;
	public object effectRight;
	public string effectLeft2;
	public string effectOp2;
	public object effectRight2;
	public string speaker;
	public string line1;
	public string line2;
	public string line3;
	public string line4;
	public string line5;
	public string eventTag;

	public int numTimesSelected = 0;

	public SCCLine()
	{
		this.line1 = "DEFAULT EMPTY LINE";
	}

	public string renderLine()
	{
		List<string> lines = new List<string>();
		if (line1 != "") lines.Add(line1);
		if (line2 != "") lines.Add(line2);
		if (line3 != "") lines.Add(line3);
		if (line4 != "") lines.Add(line4);
		if (line5 != "") lines.Add(line5);

		//Debug.Log(lines[4]);

		int index = this.numTimesSelected % lines.Count;
		this.numTimesSelected++;



		return lines[index];
	}

	public string speakerCheck()
	{
		return speaker;
	}
}
