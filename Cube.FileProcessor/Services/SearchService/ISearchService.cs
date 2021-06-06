using System;
using System.Collections.Generic;
using System.Text;

namespace Cube.FileProcessor.Services.SearchService
{
    public interface ISearchService
    {
		/// <summary>
		/// List of keywords to search for
		/// </summary>
		string RegexPattern { get; set; }

		/// <summary>
		/// Searches passed text and returns true if text contains any keyword
		/// </summary>
		/// <param name="text">Text to search</param>
		/// <returns>True when text contains any keyword</returns>
		bool ContainsAny(string text);
	}
}
