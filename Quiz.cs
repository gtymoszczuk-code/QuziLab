using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuziLab
{
    class Quiz
    {
        public string Title { get; set; }
        public int QuestionsCount { get; set; }

        // Możesz tu dodać więcej pól, które przydadzą się na innych stronach
        public string Author { get; set; }
        public string BestScore { get; set; }
    }
}
