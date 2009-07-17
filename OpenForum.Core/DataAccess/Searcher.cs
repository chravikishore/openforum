using System.Collections.Generic;
using System.Web;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using OpenForum.Core.Models;

namespace OpenForum.Core.DataAccess
{
    public class Searcher : ISearcher
    {
        private const string ID_FIELD = "id";
        private const string DATA_FIELD = "data";

        private Analyzer _analyser = new StandardAnalyzer();
        private string _indexPath;

        public Searcher()
            : this("~/App_Data/OpenForumSearchIndex")
        {
        }

        public Searcher(string indexPath)
        {
            if (HttpContext.Current == null)
            {
                _indexPath = indexPath;
            }
            else
            {
                _indexPath = HttpContext.Current.Server.MapPath(indexPath);
            }
        }

        public int[] GetSearchResults(string queryText)
        {
            List<int> result = new List<int>();
            
            IndexReader indexReader = IndexReader.Open(_indexPath);
            Lucene.Net.Search.Searcher searcher = new IndexSearcher(indexReader);

            QueryParser parser = new QueryParser(DATA_FIELD, _analyser);
            Query query = parser.Parse(queryText);

            Hits hits = searcher.Search(query);

            for (int i = 0; i < hits.Length(); i++)
            {
                result.Add(int.Parse(hits.Doc(i).GetValues(ID_FIELD)[0]));
            }

            indexReader.Close();

            return result.ToArray();
        }

        public void IndexPosts(IEnumerable<Post> posts, bool recreateIndex)
        {
            IndexWriter writer = new IndexWriter(_indexPath, _analyser, recreateIndex);

            foreach (var item in posts)
            {
                if (!recreateIndex)
                {
                    DeletePostIndex(item);
                }

                Document document = new Document();
                document.Add(new Field(ID_FIELD, item.Id.ToString(), Field.Store.YES, Field.Index.UN_TOKENIZED));
                document.Add(new Field(DATA_FIELD, item.GetFullText(), Field.Store.NO, Field.Index.TOKENIZED));
                writer.AddDocument(document);
            }

            writer.Optimize();
            writer.Close();
        }

        private void DeletePostIndex(Post post)
        {
            IndexReader indexReader = IndexReader.Open(_indexPath);
            indexReader.DeleteDocuments(new Term(ID_FIELD, post.Id.ToString()));
            indexReader.Close();
        }
    }
}
