using System.Collections;

public class SCCGameStateValue
{
	public enum TYPES
	{
		INT,
		BOOL,
		FLOAT,
		STRING
	};

	public TYPES type;
	private object value;

	public SCCGameStateValue(TYPES t, object v) {
		this.type = t;
		this.value = v;
	}
}
