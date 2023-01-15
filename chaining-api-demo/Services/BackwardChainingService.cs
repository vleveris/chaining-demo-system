using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainingAPI.Models;
using ChainingAPI.Helpers;
namespace ChainingAPI.Services
{
    public class BackwardChainingService : IChainingService
    {
        private List<int> _path = new List<int>();
        private List<string> _derivedFacts = new List<string>();
        private List<string> _goals = new List<string>();
        private StringBuilder _output = new StringBuilder();
        private KnowledgeBase _inferenceEntity;
        private int _counter = 0;
        //Executes algorithm
        public ExecutionResult Execute(KnowledgeBase inferenceEntity, string goal, bool trace)
            {
            inferenceEntity.Rules.Sort((a, b) => a.Number.CompareTo(b.Number));
            _inferenceEntity = inferenceEntity;
            _output.Clear();
            _output.AppendLine(ChainingHelper.PrintInitialData(inferenceEntity, goal));
            _path.Clear();
            _counter = 0;
            _goals.Clear();
            _derivedFacts.Clear();
            bool state = BC(goal, null);
            _output.AppendLine(ChainingHelper.PrintResults(state, _path));
            string traceOutput = null;
            if (trace)
            {
                traceOutput = _output.ToString();
            }
            return new ExecutionResult { GoalAchieved = state, Path = _path, Trace = traceOutput };

        }

        private bool BC(string goal, Rule usedRule)
            {
                if (!_goals.Contains(goal))
                {
                    if (_inferenceEntity.Facts.Contains(goal))
                    {
                    _counter++;
                    _output.AppendLine($"{_counter}. Goal: {goal} (is in facts).");
                    return true;
                    }
                    else if (_derivedFacts.Contains(goal))
                    {
                    _counter++;
                    _output.AppendLine($"{_counter}. Goal: {goal} (is in derived facts).");
                    return true;
                    }
                    else
                    {
                        _goals.Add(goal);
                    foreach (Rule rule in _inferenceEntity.Rules)
                        {
                            if (rule.Consequent.Equals(goal))
                            {
                            _counter++;
                            _output.AppendLine($"{_counter}. Goal: {goal}. Found rule {rule.Number.ToString()}. New goals: {string.Join(", ", rule.Antecedent)}.");
                            bool usable = true;
                                List<string> tempDerivedFacts = new List<string>();
                                List<int> tempRuleNumbers = new List<int>();
                                tempDerivedFacts.AddRange(_derivedFacts);
                                tempRuleNumbers.AddRange(_path);
                                foreach (var fact in rule.Antecedent)
                                {
                                    if (!BC(fact, rule))
                                    {
                                        usable = false;
                                    _derivedFacts = tempDerivedFacts;
                                    _path = tempRuleNumbers;
                                    break;
                                    }
                                }

                                if (usable)
                                {
                                                                _derivedFacts.Add(rule.Consequent);
                                _path.Add(rule.Number);
                                _counter++;
                                _output.AppendLine($"{_counter}. Goal: {goal}. Deriving as new fact. Currently, a set of facts is {{{string.Join(", ", _inferenceEntity.Facts)}, {string.Join(", ", _derivedFacts)}}}.");
                                    _goals.Remove(goal);

                                    return true;
                                }
                            }
                        }
                    _counter++;
                    _output.AppendLine($"{_counter}. Goal {goal} is unriechable, there are no further rules to infer it.");
                        _goals.Remove(goal);
                        return false;
                    }
                }
                else
                {
                _counter++;
                _output.AppendLine($"{_counter}. Goal {goal} failed.");
                    return false;
                }
            }


        }
    }

