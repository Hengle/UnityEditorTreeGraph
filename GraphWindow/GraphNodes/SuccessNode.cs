﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateNodeMenu("Miscellaneous/Success")]
public class SuccessNode : BaseNode
{
	public override string GetCode()
	{
		string code = "BT_Success "+nodeName+"= new BT_Success();\n";
		return code;
	}
}