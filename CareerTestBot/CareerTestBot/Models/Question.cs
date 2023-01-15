using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTestBot.Models
{
    public class Question
    {
public int Number { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }

        public Question(int number, string answer1, string answer2)
        {
            Number = number;
            Answer1 = answer1;
            Answer2 = answer2;
        }
    }
}
