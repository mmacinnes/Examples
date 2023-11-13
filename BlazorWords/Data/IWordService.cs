using System.Collections.Generic;
using blazorWords.Models;

namespace blazorWords.Data
{
    public interface IWordService
    {
        public List<Words> GetWords();
    }
}