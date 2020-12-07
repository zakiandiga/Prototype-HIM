using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleConditionalConversation 
{
	public Dictionary<string, object> gameState;
	public string questState = "Q1T1";
	public string speakerName;

	Hashtable lines;
	
	public SimpleConditionalConversation(string dataPath)
	{
		this.gameState = new Dictionary<string, object>();
		List<Dictionary<string, object>> data = CSVReader.Read(dataPath);
		this.loadLines(data);
	}
	
	public SimpleConditionalConversation(List<Dictionary<string, object>> data)
	{
		this.gameState = new Dictionary<string, object>();
		this.loadLines(data);
	}
	
	
	// Loads data from the data structure that CSVReader creates when it loads
	// a CSV file.
	public void loadLines(List<Dictionary<string, object>> data) 
	{
		this.lines = new Hashtable();

		for (var i = 0; i < data.Count; i++) {
			if (!lines.ContainsKey((string)data[i]["questState"])) {
				lines.Add((string)data[i]["questState"], new Dictionary<string, List<SCCLine>>());
			}
			Dictionary<string, List<SCCLine>> questStateLines = (Dictionary<string, List<SCCLine>>)lines[(string)data[i]["questState"]];
			if (!questStateLines.ContainsKey((string)data[i]["character"])) {
				questStateLines[(string)data[i]["character"]] = new List<SCCLine>();
			}
			List<SCCLine> characterLines = questStateLines[(string)data[i]["character"]];
			SCCLine line = new SCCLine();
			line.questState = (string)data[i]["questState"];
			line.character = (string)data[i]["character"];
			line.location = (string)data[i]["location"];
			line.condition1Left = (string)data[i]["condition1Left"];
			line.condition1Comp = (string)data[i]["condition1Comp"];
			line.condition1Right = data[i]["condition1Right"];
			line.condition2Left = (string)data[i]["condition2Left"];
			line.condition2Comp = (string)data[i]["condition2Comp"];
			line.condition2Right = data[i]["condition2Right"];
			line.effectLeft = (string)data[i]["effectLeft"];
			line.effectOp = (string)data[i]["effectOp"];
			line.effectRight = data[i]["effectRight"];
			line.effectLeft2 = (string)data[i]["effectLeft2"];
			line.effectOp2 = (string)data[i]["effectOp2"];
			line.effectRight2 = data[i]["effectRight2"];
			line.speaker = (string)data[i]["speaker"];
			line.line1 = (string)data[i]["line1"];
			line.eventTag = (string)data[i]["event"];
			characterLines.Add(line);
		}
	}

    /*
     * General game programming entry point for getting dialogue from the Google
     * Sheet-based system. Dialogue retrieved is based on the conditions set in the
     * sheet, the current quest state (as stored by the GameManager), and number of
     * times the character was interacted wtih in the current quest state.
     * @param  {string} name            The name of the character character
     * @return {SCCLine}                Context-specific dialogue.
    */
    public string [] getSCCLine(string name) {

		string speaker;
		string dialogueLine;

		Dictionary<string, List<SCCLine>> questLines = (Dictionary<string, List<SCCLine>>)this.lines[questState];
		if (!questLines.ContainsKey(name)) {
			return new[] { "Ughhh", "ugh" };
		} 
		List <SCCLine> lines = questLines[name];

		foreach (SCCLine line in lines) {
			//Check both conditions until default is reached. Return the first one
			//Check condition1
			bool condition1 = checkCondition(line.condition1Left, line.condition1Comp, line.condition1Right);
			//Debug.Log(condition1);
			bool condition2 = checkCondition(line.condition2Left, line.condition2Comp, line.condition2Right);
			//Debug.Log(condition2);
			if (condition1 && condition2) {
				setGameStateValue(line.effectLeft, line.effectOp, line.effectRight);
				setGameStateValue(line.effectLeft2, line.effectOp2, line.effectRight2);
				speaker = line.speaker;
				dialogueLine = line.renderLine();
				return new string[] { speaker, dialogueLine };
			}
		}
		return new[] { "speaker not found ", "LINE NOT FOUND" };
    }

	public string checkSpeaker(string speaker)
    {
		speakerName = speaker;
		Debug.Log(speaker);
		return speaker;
		
		
    }

	/**
     * This checks a condition on dialogue for its truth value.
     * @param  {String} left  The left side of the conditional.
     * @param  {String} op    The comparison operator to use.
     * @param  {object} right The right side of the conditional.
     * @return {bool}       The truth value of the conditional.
     */
	public bool checkCondition(string left, string op, object right) 
	{
		//Debug.Log("CHECKING: " + left + " " + op + " " + (string)right);
		object leftValue = getGameStateValue(left);

		//If there's nothing there, and it is checking if it doesn't equal something, return true
		if (op == "not equals" && leftValue == null) {
			return true;
		} else if (leftValue == null && op != "") {
			return false;
		}

		if (leftValue is int) {
			int leftInt = (int)leftValue;
			//This means we can this as an int
			if (op == "greater") {
				return leftInt > (int)right;
			} else if (op == "less") {
				return leftInt  < (int)right;
			} else if (op == "equals") {
				return leftInt == (int)right;
			} else if (op == "not equals") {
				return leftInt != (int)right;
			}
		} else if (leftValue is float) {
			float leftFloat = (float)leftValue;
			//This means we can this as a float
			float rightFloat;
			float.TryParse(right.ToString(), out rightFloat);
			if (op == "greater") {
				//return leftFloat > (float)right;
				return leftFloat > rightFloat;
			} else if (op == "less") {
				return leftFloat < (float)right;
			} else if (op == "equals") {
				//round to int. Floats can't be equal.
				int leftInt = (int)leftFloat;
				int rightInt = (int)right;
				return leftInt == rightInt;
			} else if (op == "not equals") {
				//round to int. Floats can't be equal.
				int leftInt = (int)leftFloat;
				int rightInt = (int)right;
				return leftInt != rightInt;
			}
		} else if (leftValue is bool) {
			bool leftBool = (bool)leftValue;
			//This means we can this as a bool
			if (op == "equals") {
				return leftBool == (bool)right;
			} else if (op == "not equals") {
				return leftBool != (bool)right;
			}
		} else {
			//By default, treat it as a string
			if (op == "equals") {
				return (string)leftValue == (string)right;
			} else if (op == "not equals") {
				if (leftValue == null || (string)leftValue != (string)right) {
					return true;
				} else {
					return false;
				}
			}
		}
		//If we get down here, that means there was nothing there I think
		return true;
    }

    /*
     * Retrieves the value associated with the given in either the queststate/
     * dialogue structure or in the blackboard memory (A.K.A. this.gameState).
     * Values will be converted into numbers when possible.
     * @param  {String} id The id of the value to look up.
     * @return {Object}    The data associated with the id.
     */
	public object getGameStateValue(string id) 
	{
		if (this.gameState.ContainsKey(id)) {
			return this.gameState[id];
		}
		return null;
    }

    /*
     * Sets id/value pairs in game state for use by conditionals (and perhaps
     * more). It looks for existing entries to update. If no entry is found, an
     * entry in the GameManager's gameState is created. The value is modifed by
     * operation parameter (e.g. add, equals/set, etc.). The default for new ids
     * is 0. Values will be converted into numbers when possible.
     * @param {String} id    Id of the id/value pair.
     * @param {String} op    The operation to apply when setting the value.
     * @param {object} right The value side of the value to set.
     */
    public void setGameStateValue(string id, string op, object right) 
	{
		if (op == "add") {
			if (!this.gameState.ContainsKey(id)) {
				this.gameState.Add(id, 0);
			}
			this.gameState[id] = (int)this.gameState[id] + (int)right;
		} else if (op == "subtract") {
			if (!this.gameState.ContainsKey(id)) {
				this.gameState.Add(id, 0);
			}
			this.gameState[id] = (int)this.gameState[id] - (int)right;
		} else if (op == "equals" || op == "set") {
			bool rightBool;
			float rightFloat;
			if (float.TryParse(right.ToString(), out rightFloat)) {
			//if (right is float) {
				if (!this.gameState.ContainsKey(id)) {
					this.gameState.Add(id, right);
				} else {
					this.gameState[id] = right;
				}
			} else if (bool.TryParse((string)right, out rightBool)) {
				if (!this.gameState.ContainsKey(id)) {
					this.gameState.Add(id, rightBool);
				} else {
					this.gameState[id] = rightBool;
				}
			} else {
				if (!this.gameState.ContainsKey(id)) {
					this.gameState.Add(id, (string)right);
				} else {
					this.gameState[id] = (string)right;
				}
			}
		}
    }
}
