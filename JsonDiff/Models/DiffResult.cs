using System.Collections.Generic;

namespace JsonDiff.Models
{
    public class DiffResult
    {
        public DiffResult()
        {
            Differences = new List<string>();
        }

        public bool AreEqual { get; set; }
        public IList<string> Differences { get; set; }
    }
}
