﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateNodeMenu("Control/While")]
public class WhileNode : BaseNode
{
	[TextArea]
	public string condition;

	override public bool Test(List<Node> nodes)
	{
		bool result = base.Test(nodes);
		if (!string.IsNullOrEmpty(nodeName))
		{
			if (string.IsNullOrEmpty(condition))
			{
				Debug.LogError(nodeName + ": condition is empty.");
				result = false;
			}
		}
		if (!this.GetOutputPort("output").IsConnected)
		{
			Debug.LogAssertion(nodeName + ": This node doesn't have any children.");
			result = false;
		}

		return result;
	}

	public override CodeTemplateParameterHolder GetParameterHolder()
	{
		CodeTemplateParameterHolder holder = new CodeTemplateParameterHolder();
		holder.SetParameter("name", nodeName);
		holder.SetParameter("condition", condition);

		return holder;
	}

	public override void InheritFrom(Node original)
	{
		base.InheritFrom(original);
		if (original is WhileNode wh_original)
		{
			this.condition = wh_original.condition;
		}
	}

	public override string GetKey()
	{
		return "While";
	}
}
