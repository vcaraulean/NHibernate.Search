using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;

namespace NHibernate.Search.Fluent.Mapping.Parts
{
	public class AnalyzerPart<T> where T : IHasAnalyzer
	{
		private readonly T part;

		public AnalyzerPart(T part)
		{
			this.part = part;
		}

		public T Custom<TAnalyzer>() where TAnalyzer : Analyzer, new()
		{
			part.AnalyzerType = typeof(TAnalyzer);
			return part;
		}

		/// <summary>
		/// Sets the analyzer to a StandardAnalyzer
		/// </summary>
		/// <returns></returns>
		public T Standard()
		{
			return Custom<StandardAnalyzer>();
		}

		/// <summary>
		/// Sets the analyzer to a KeywordAnalyzer
		/// </summary>
		/// <returns></returns>
		public T Keyword()
		{
			return Custom<KeywordAnalyzer>();
		}

		/// <summary>
		/// Sets the analyzer to a SimpleAnalyzer
		/// </summary>
		/// <returns></returns>
		public T Simple()
		{
			return Custom<SimpleAnalyzer>();
		}

		/// <summary>
		/// Sets the analyzer to a StopAnalyzer
		/// </summary>
		/// <returns></returns>
		public T Stop()
		{
			return Custom<StopAnalyzer>();
		}

		/// <summary>
		/// Sets the analyzer to a WhitespaceAnalyzer
		/// </summary>
		/// <returns></returns>
		public T Whitespace()
		{
			return Custom<WhitespaceAnalyzer>();
		}
	}
}