﻿using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateNodeMenu("Timing/Timer")]
public class TimerNode : BaseNode
{
	[SerializeField]
	string targetNode;
	[SerializeField]
	string waitTime;
	[SerializeField]
	bool isOverwrite, isMultiple;

	public override bool Test(List<Node> nodes)
	{
		bool result = base.Test(nodes);

		if (!string.IsNullOrEmpty(targetNode))
		{
			bool isTargetFound = false;
			bool isWaitTimeValid = false;
			foreach (Node node in nodes)
			{
				if (node is SubNode s)
				{
					if (s.nodeName == waitTime)
					{
						isWaitTimeValid = true;
					}
				}
				else if (node is ITreeGraphNode i)
				{
					if (i.GetNodeName() == targetNode)
					{
						isTargetFound = true;
						break;
					}
				}
			}

			if (!isTargetFound)
			{
				Debug.LogError(nodeName + ": \"" + nodeName + "\" doesn't exist.");
				result = false;
			}

			double tmp;
			if (!isWaitTimeValid && !double.TryParse(waitTime, out tmp))
			{
				Debug.LogError(nodeName + ": WaitTime is invalid.");
				result = false;
			}
		}
		else
		{
			Debug.LogError(nodeName + ": TargetNode is empty.");
			result = false;
		}

		if (string.IsNullOrEmpty(waitTime))
		{
			Debug.LogError(nodeName + ": WaitTime is empty.");
			result = false;
		}

		return result;
	}

	public override void InheritFrom(Node original)
	{
		base.InheritFrom(original);
		if (original is TimerNode t)
		{
			this.targetNode = t.targetNode;
			this.waitTime = t.waitTime;
			this.isOverwrite = t.isOverwrite;
			this.isMultiple = t.isMultiple;
		}
	}

	public override CodeTemplateParameterHolder GetParameterHolder()
	{
		CodeTemplateParameterHolder holder = new CodeTemplateParameterHolder();
		holder.SetParameter("name", nodeName);
		holder.SetParameter("isOverwrite", isOverwrite.ToString().ToLower());
		holder.SetParameter("isMultiple", isMultiple.ToString().ToLower());
		holder.SetParameter("targetNode", targetNode);
		holder.SetParameter("waitTime", waitTime);

		return holder;
	}

	public override string GetKey()
	{
		return "Timer";
	}
}
