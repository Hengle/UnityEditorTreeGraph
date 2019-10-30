﻿using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateNodeMenu("Set Parameter/Set Bool")]
public class SetBoolNode : BaseNode
{
	public string boolName;
	public bool value;
	public override void Test(List<Node> nodes)
	{
		base.Test(nodes);
		if (string.IsNullOrEmpty(boolName))
		{
			Debug.LogError(nodeName + " : Parameter name is empty.");
		}
		else
		{
			bool isParameterExist = false;
			foreach (Node node in nodes)
			{
				if (node is BoolNode b)
				{
					if (b.GetNodeName() == boolName)
					{
						isParameterExist = true;
					}
				}
			}

			if (!isParameterExist)
			{
				Debug.LogError(nodeName + " : Bool parameter \"" + boolName + "\" doesn't exist.");
			}
		}
	}

	public override string GetCode()
	{
		string code = string.Format(CodeTemplateReader.Instance.GetTemplate("SetParameter.txt"), nodeName, boolName, value.ToString().ToLower());
		return base.GetCode();
	}
}
