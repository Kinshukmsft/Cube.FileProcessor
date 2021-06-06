using System.Collections;

namespace Cube.FileProcessor.Services.SearchService
{
	/// <summary>
	/// Tree node representing character and its 
	/// transition and failure function
	/// </summary>
	class TreeNode
	{
		#region Constructor & Methods

		/// <summary>
		/// Initialize tree node with specified character
		/// </summary>
		/// <param name="parent">Parent node</param>
		/// <param name="c">Character</param>
		public TreeNode(TreeNode parent, char c)
		{
			_char = c; _parent = parent;
			_results = new ArrayList();
			_resultsAr = new string[] { };

			_transitionsAr = new TreeNode[] { };
			_transHash = new Hashtable();
		}


		/// <summary>
		/// Adds pattern ending in this node
		/// </summary>
		/// <param name="result">Pattern</param>
		public void AddResult(string result)
		{
			if (_results.Contains(result)) return;
			_results.Add(result);
			_resultsAr = (string[])_results.ToArray(typeof(string));
		}

		/// <summary>
		/// Adds trabsition node
		/// </summary>
		/// <param name="node">Node</param>
		public void AddTransition(TreeNode node)
		{
			_transHash.Add(node.Char, node);
			TreeNode[] ar = new TreeNode[_transHash.Values.Count];
			_transHash.Values.CopyTo(ar, 0);
			_transitionsAr = ar;
		}


		/// <summary>
		/// Returns transition to specified character (if exists)
		/// </summary>
		/// <param name="c">Character</param>
		/// <returns>Returns TreeNode or null</returns>
		public TreeNode GetTransition(char c)
		{
			return (TreeNode)_transHash[c];
		}


		/// <summary>
		/// Returns true if node contains transition to specified character
		/// </summary>
		/// <param name="c">Character</param>
		/// <returns>True if transition exists</returns>
		public bool ContainsTransition(char c)
		{
			return GetTransition(c) != null;
		}

		#endregion
		#region Properties

		private char _char;
		private TreeNode _parent;
		private TreeNode _failure;
		private ArrayList _results;
		private TreeNode[] _transitionsAr;
		private string[] _resultsAr;
		private Hashtable _transHash;

		/// <summary>
		/// Character
		/// </summary>
		public char Char
		{
			get { return _char; }
		}


		/// <summary>
		/// Parent tree node
		/// </summary>
		public TreeNode Parent
		{
			get { return _parent; }
		}


		/// <summary>
		/// Failure function - descendant node
		/// </summary>
		public TreeNode Failure
		{
			get { return _failure; }
			set { _failure = value; }
		}


		/// <summary>
		/// Transition function - list of descendant nodes
		/// </summary>
		public TreeNode[] Transitions
		{
			get { return _transitionsAr; }
		}


		/// <summary>
		/// Returns list of patterns ending by this letter
		/// </summary>
		public string[] Results
		{
			get { return _resultsAr; }
		}

		#endregion
	}
}
