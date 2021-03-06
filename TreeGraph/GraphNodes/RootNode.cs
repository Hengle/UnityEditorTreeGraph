﻿using XNode;
using System.Collections.Generic;
using UnityEngine;
[CreateNodeMenu("Head/Root")]
[NodeTint("#ffaaaa")]
public class RootNode : Node, ITreeGraphNode
{
    public string nodeName;
    [Output(ShowBackingValue.Never, ConnectionType.Override, false)] public string output;

    public string GetNodeName(){
        return nodeName;
    }

	public void SetNodeName(string name)
	{
		nodeName = name;
	}

	public bool Test(List<Node> nodes)
	{
		bool result = true;
		if (string.IsNullOrEmpty(nodeName))
		{
			Debug.LogError("Root node doesn't have a name. All node should have an unique name.");
			result = false;
		}

		foreach (Node node in nodes)
		{
			if (node is ITreeGraphNode i)
			{
				if (this.nodeName == i.GetNodeName() && node != this)
				{
					Debug.LogError(nodeName + "This nodename is not unique.");
					result = false;
				}
			}
		}

		return result;
	}

	public CodeTemplateParameterHolder GetParameterHolder()
	{
		CodeTemplateParameterHolder holder = new CodeTemplateParameterHolder();
		holder.SetParameter("name", nodeName);

		return holder;
	}

	public string GetKey()
	{
		return "Root";
	}

	public void InheritFrom(Node original)
	{

	}
}
