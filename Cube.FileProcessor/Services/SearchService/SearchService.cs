using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace Cube.FileProcessor.Services.SearchService
{
	public class SearchService: ISearchService
	{
		private string _pattern;
		private Regex _reg;

		public SearchService()
		{ }

		
		#region Methods & Properties
		/// <summary>
		/// Regular expression pattern to search for 
		/// </summary>
		public string RegexPattern
		{
			get { return _pattern; }
			set
			{
				_pattern = value;
				_reg = new Regex("(" + string.Join("|", value) + ")", RegexOptions.None);
			}
		}

		/// <summary>
		/// Searches passed text and returns true if text contains any pattern
		/// </summary>
		/// <param name="text">Text to search</param>
		/// <returns>True when text contains any pattern</returns>
		public bool ContainsAny(string text)
		{
			return _reg.Match(text).Success;
		}

        #endregion
    }
}


