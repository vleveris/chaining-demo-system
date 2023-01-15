using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainingAPI.Models;
using ChainingAPI.Helpers;
namespace ChainingAPI.Services
{
    public class ForwardChainingService : IChainingService
    {
        private string _goal;
        private List<int> _path = new List<int>();
        private StringBuilder _output = new StringBuilder();
        private KnowledgeBase _inferenceEntity;

        //Executes algorithm

        public ExecutionResult Execute(KnowledgeBase inferenceEntity, string goal, bool trace)
        {

            inferenceEntity.Rules.Sort((a, b) => a.Number.CompareTo(b.Number));
            _inferenceEntity = inferenceEntity;
            _goal = goal;
            _output.Clear();
            _output.AppendLine(ChainingHelper.PrintInitialData(inferenceEntity, goal));
            _path.Clear();
            bool goalAchieved = FindGoal(1);
            _output.AppendLine(ChainingHelper.PrintResults(goalAchieved, _path));
            string traceOutput = null;
            if (trace)
            {
                traceOutput = _output.ToString();
            }
            return new ExecutionResult { GoalAchieved = goalAchieved, Path = _path, Trace = traceOutput };
        }

        private bool CheckInFacts()
        {
            _output.AppendLine("Checking facts...");
            if (_inferenceEntity.Facts.Contains(_goal))
            {
                _output.AppendLine("Stopping trace... Facts contain goal.");
                return true;
            }
                return false;
        }

        private bool FindGoal(int iteration)
        {
            _output.AppendLine($"Iteration {iteration}");
            if (CheckInFacts())
            {
                return true;
            }
            foreach (var r in _inferenceEntity.Rules)
            {
                _output.Append($"Rule {r.Number}: ");
                if (r.Flag2)
                {
                    _output.AppendLine("skipping, flag 2 is set.");
                }
                else if (r.Flag1)
                {
                    _output.AppendLine("skipping, flag 1 is set.");
                }
                else if (_inferenceEntity.Facts.Contains(r.Consequent))
                {
                    _output.AppendLine("not applied, consequent  is in facts. Setting flag2.");
                    r.Flag2 = true;
                }
                else if (IsInRuleAntecedent(r.Antecedent))
                {
                    _output.AppendLine($"applying, adding fact {r.Consequent}, setting flag1.");
                    _inferenceEntity.Facts.Add(r.Consequent);
                    r.Flag1 = true;
                    _path.Add(r.Number);
                    return FindGoal(iteration + 1);
                }
                else
                {
                    List<string> missing = new List<string>();
                    foreach (var a in r.Antecedent)
                    {
                        if (!_inferenceEntity.Facts.Contains(a))
                        {
                            missing.Add(a);
                        }
                    }
                        _output.AppendLine($"not applied, missing facts {string.Join(", ", missing.ToList())}.");
                }
            }
            return false;
        }

        private bool IsInRuleAntecedent(IEnumerable<string> antecedent)
        {
            int count = 0;

            foreach (var f in antecedent)
            {
                if (_inferenceEntity.Facts.Contains(f))
                {
                    count++;
                }
            }
            return count == antecedent.Count();
        }

    }
}
