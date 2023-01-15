using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainingAPI.Models;
namespace ChainingAPI.Helpers
{
    public class ChainingHelper
    {
public static bool CheckKnowledgeBaseValidity(KnowledgeBase knowledgeBase)
        {
            if (knowledgeBase.Facts is null || knowledgeBase.Facts.Count < 1 || knowledgeBase.Rules is null || knowledgeBase.Rules.Count < 1)
            {
                return false;
            }
            return true;
                    }

        public static string PrintInitialData(KnowledgeBase knowledgeBase, string goal)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("Initial data");
            output.AppendLine($"Facts: {string.Join(", ", knowledgeBase.Facts)}.");
            output.AppendLine("");
            output.AppendLine("Rules:");
            foreach (var rule in knowledgeBase.Rules)
                output.AppendLine($"{rule.Number}. {rule.ToString()}");

            output.AppendLine($"Goal: {goal}.");

            output.AppendLine("Trace");
            output.AppendLine("");
            return output.ToString();
        }

        public static string PrintResults(bool goalAchieved, List<int> path)
        {
            var output = new StringBuilder();
            output.AppendLine("");

            output.AppendLine("Results");
            output.Append("Goal is ");

            if (!goalAchieved)
            {
                output.Append("not ");
            }
            output.AppendLine("achieved.");
            if (goalAchieved)
            {
                output.Append("Path: ");
                if (path.Count == 0)
                {
                    output.Append("Empty, goal is in facts");
                }
                else
                {
                    output.Append(string.Join(", ", path));
                }
                output.Append(".");
            }
            return output.ToString();

        }
    }
}
