﻿using System.Collections.Generic;
using XNode;
using UnityEngine;
using System.IO;
using System.Linq;

[CreateAssetMenu(fileName = "BehaviourTreeCompiler",
								menuName = "Compilers/BehaviourTreeCompiler")]
public class BehaviourTreeCompiler : EditorTreeCompiler
{
	List<string> createdNodes;
	protected string inheritedClass = "BehaviourTreeComponent";
	override public string Compile(string fileName, List<Node> nodes, string inheritTarget = "")
	{
		Debug.Log("Start Compile");
		if (string.IsNullOrEmpty(inheritTarget))
		{
			inheritTarget = inheritedClass;
		}
		List<SubNode> subNodes = new List<SubNode>();
		createdNodes = new List<string>();
		RootNode root = new RootNode();
		foreach (Node node in nodes)
		{
			if (node is SubNode s)
			{
				subNodes.Add(s);
			}
			else if (node is RootNode r)
			{
				root = r;
			}
		}

		//CodeTemplateReader.dirName = Path.Combine(Application.dataPath, codeTemplatePath);
		CodeTemplateReader.Init(Path.Combine(Application.dataPath, codeTemplatePath));
		string template = CodeTemplateReader.GetTemplate("Base", "Class");

		string className = FileNameToClassName(fileName);
		string inheritName = FileNameToClassName(inheritTarget);

		string declareParameters = "";
		var sortedSubNodes = subNodes
											.OrderBy(x => x.GetType().ToString())
											.ToArray();
		foreach (SubNode node in sortedSubNodes)
		{
			if (!node.isInherited)
			{
				CodeTemplateParameterHolder holder = node.GetParameterHolder();
				string key = node.GetKey();
				string source = CodeTemplateReader.GetTemplate("Declare", key);
				//declareParameters += node.GetDeclare();
				declareParameters += CodeTemplateInterpolator.Interpolate(source, holder);
			}
		}

		string constructTree = "";
		CodeTemplateParameterHolder rootParameter = root.GetParameterHolder();
		string rootKey = root.GetKey();
		string rootDeclare = CodeTemplateInterpolator.Interpolate(CodeTemplateReader.GetTemplate("Declare", rootKey), rootParameter);
		string rootInit = CodeTemplateInterpolator.Interpolate(CodeTemplateReader.GetTemplate("Init", rootKey), rootParameter);
		constructTree += rootDeclare + rootInit;
		var rootChild = root.GetOutputPort("output").GetConnection(0).node as IBTGraphNode;

		foreach (Node node in nodes)
		{
			if (!(node is SubNode) && !(node is RootNode))
			{
				if (node is IBTGraphNode i)
				{
					CodeTemplateParameterHolder holder = i.GetParameterHolder();
					string key = i.GetKey();
					string source = CodeTemplateReader.GetTemplate("Declare", key);
					constructTree += CodeTemplateInterpolator.Interpolate(source, holder) + "\n";
					//constructTree += i.GetDeclare() + "\n";
				}
			}
		}

		foreach (Node node in nodes)
		{
			if (!(node is SubNode))
			{
				if (node is IBTGraphNode i)
				{
					if (!(node is RootNode))
					{
						CodeTemplateParameterHolder holder = i.GetParameterHolder();
						string key = i.GetKey();
						string source = CodeTemplateReader.GetTemplate("Init", key);
						constructTree += CodeTemplateInterpolator.Interpolate(source, holder) + "\n";
						//constructTree += i.GetInit() + "\n";
					}
					var children = node.GetOutputPort("output").GetConnections()
											.OrderBy(x => x.node.position.y)
											.ToArray();
					foreach (NodePort port in children)
					{
						Node child = port.node;
						if (child is IBTGraphNode i_child)
						{
							constructTree += i.GetNodeName() + ".AddChild(" + i_child.GetNodeName() + ");\n";
						}
					}
				}
			}
		}

		//string code = string.Format(template, className, inheritName, declareParameters, constructTree);
		CodeTemplateParameterHolder templateParameter = new CodeTemplateParameterHolder();
		templateParameter.SetParameter("className", className);
		templateParameter.SetParameter("inheritName", inheritName);
		templateParameter.SetParameter("declareParameters", declareParameters);
		templateParameter.SetParameter("constructTree", constructTree);
		string code = CodeTemplateInterpolator.Interpolate(template, templateParameter);
		return code;
	}
}
