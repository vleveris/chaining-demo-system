using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareerTestBot.Models;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.GettingUpdates;
using Telegram.BotAPI.UpdatingMessages;

namespace CareerTestBot
{
    public sealed class CareerBot : TelegramBotBase
    {
        private static string botToken = "<bot_token>";
        public static readonly BotClient Bot = new (botToken);
public static readonly User Me =Bot.GetMe();

        //
        private Message _message;
        private bool _hasText;
        private User _appUser;
        private List<Question> _questions = new List<Question>() {
                new Question(1, "I would rather be a wildlife expert.", "I would rather be a public relations professional."),
                new Question(2, "I would rather be a company controller.", "I would rather be a TV news anchor."),
                new Question(3, "I would rather be a tax lawyer.", "I would rather be a newspaper editor."),
                new Question(4, "I would rather be an auditor.", "I would rather be a musician."),
                new Question(5, "I would rather be a production manager.", "I would rather be an advertising manager."),
new Question(6, "I would rather be an accounting manager.", "I would rather be a history professor."),
new Question(7, "I would rather be a bookkeeper.", "I would rather be an electrician."),
new Question(8, "I would rather be a writer.", "I would rather be an elected official."),
new Question(9, "I would rather be a clerical worker.", "I would rather be a carpenter."),
new Question(10, "I would rather be a payroll manager.", "I would rather be a manager of engineering."),
new Question(11, "I would rather be an audit manager.", "I would rather be a safety manager."),
new Question(12, "I would rather be an artist.", "I would rather be a salesperson."),
new Question(13, "I am usually patient when I have to wait on an appointment.", "I get restless when I have to wait on an appointment."),
new Question(14, "It is easy to laugh at one's little social errors or \"aux pas\"", "It is hard to laugh at one's little social errors or \"faux pas\"."),
new Question(15, "It is wise to make it known if someone is doing something that bothers you.", "It is wise to remain silent if someone is doing something that bothers you."),
new Question(16, "It's not really OK to argue with others even when you know you are right.", "It's OK to argue with others when you know you are right."),
new Question(17, "I like to bargain to get a good price.", "I don't like to have to bargain to get a good price."),
new Question(18, "It is easy to be outgoing and sociable at a party with strangers.", "It is hard to be outgoing and sociable at a party with strangers."),
new Question(19, "I would read the instructions first when putting a new toy together for a child.", "I would just \"jump in\" and start putting a new toy together for a child."),
new Question(20, "It is usually best to be pleasant and let others decide if your ideas are worth accepting.", "It is usually best to be forceful and \"sell\" your ideas to others."),
new Question(21, "I usually like to work cautiously.", "I usually like to work fast."),
new Question(22, "Generally I prefer to work quietly with a minimum of wasted movement.", "Generally I prefer to move around and burn some energy while I work."),
new Question(23, "I don't like to have to persuade others to accept my ideas when there is a strong forceful opposition or argument from others.", "I like to sell and promote my ideas with others even when it takes some argument."),
new Question(24, "It is better to listen carefully and be sure you understand when topics are being discussed.", "It is better to speak up quickly and be heard when topics are being discussed.")
};
        private Dictionary<long, List<string>> _userFacts = new Dictionary<long, List<string>>();

        public override void OnUpdate(Update update)
        {
            Console.WriteLine("New update with id: {0}. Type: {1}", update?.UpdateId, update?.Type.ToString("F"));
            base.OnUpdate(update);
        }

        protected override void OnMessage(Message message)
        {
            // Ignore user 777000 (Telegram)
            if (message.From.Id == 777000)
            {
                return;
            }
            Console.WriteLine("New message from chat id: {0}", message.Chat.Id);

            _appUser = message.From; // Save current user;
            _message = message; // Save current message;
            _hasText = !string.IsNullOrEmpty(message.Text); // True if message has text;
            Console.WriteLine("Message Text: {0}", _hasText ? message.Text : "|:O");

            if (message.Chat.Type == ChatType.Private) // Private Chats
            {
                if (_hasText)
                {
                    if (message.Text.StartsWith('/')) // New commands
                    {
                        var splitText = message.Text.Split(' ');
                        var command = splitText.First();
                        var parameters = splitText.Skip(1).ToArray();
                        // If the command includes a mention, you should verify that it is for your bot, otherwise you will need to ignore the command.
                        var pattern = string.Format(@"^\/(?<cmd>\w*)(?:$|@{0}$)", Me.Username);
                        var match = Regex.Match(command, pattern, RegexOptions.IgnoreCase);
                        if (match.Success)
                        {
                            command = match.Groups.Values.Last().Value; // Get command name
                            Console.WriteLine("New command: {0}", command);
                            OnCommand(command, parameters);
                        }
                    }
                }

            }
            else // Group chats
            {

            }
        }

        private void OnCommand(string cmd, string[] args)
        {
            Console.WriteLine("Params: {0}", args.Length);
            var userId = _appUser.Id;
            switch (cmd)
            {
                case "quiz":
                    if (_userFacts.ContainsKey(userId))
                    {
                        Bot.SendMessage(_message.Chat.Id, "You have already started career quiz. Please cancel it using /cancel command before starting a new one.");
                    }

                    else
                    {
                        _userFacts.Add(userId, new List<string>());
                        Bot.SendMessage(_message.Chat.Id, "Welcome! In order for us to estimate your personal Interests and Usual Style, you will first need to answer a series of questions. Read each pair of phrases below and decide which one of the two most describes you, then click the appropriate button.\nAs you make your choices, assume that all jobs are of equal pay and prestige.\nThere are 24 total questions.");
                        SendQuestion(1);

                    }
                    break;
                case "cancel":
                    if (_userFacts.ContainsKey(userId))
                    {
                        _userFacts.Remove(userId);
                        Bot.SendMessage(_message.Chat.Id, "Career quiz canceled successfully.");
                    }
                    else
                    {
                        Bot.SendMessage(_message.Chat.Id, "Career quiz is not started.");
                    }
                    break;
                case "help":
                    Bot.SendMessage(_message.Chat.Id, "This bot allows you to evaluate your possible career type according to Birkman method.\nIt was created as a part of forward chaining demonstration system.\nUse /quiz command to start quiz and folllow on-screen instructions to complete the career test.\nIf you would like to stop quiz currently in progress, use /cancel command.");
                    break;
            }
        }

        protected override void OnBotException(BotRequestException exp)
        {
            Console.WriteLine("New BotException: {0}", exp.Message);
            Console.WriteLine("Error Code: {0}", exp.ErrorCode);
            Console.WriteLine();
        }

        protected override void OnException(Exception exp)
        {
            Console.WriteLine("New Exception: {0}", exp.Message);
            Console.WriteLine();
        }

        private void SendQuestion(int number)
{
    var question = _questions.FirstOrDefault(q => q.Number == number);
    if (question is null)
    {
        return;
    }
    var replyMarkup = new InlineKeyboardMarkup
    {
        InlineKeyboard = new InlineKeyboardButton[][]{
                                        new InlineKeyboardButton[]{
                                            InlineKeyboardButton.SetCallbackData(question.Answer1, $"{number.ToString()}-A"),
                                            InlineKeyboardButton.SetCallbackData(question.Answer2, $"{number.ToString()}-B")

        }

        }
    };
    Bot.SendMessage(_message.Chat.Id, $"{number}.", replyMarkup: replyMarkup);
}

        private void UpdateMessage(Message message, int number)
        {
            var question = _questions.FirstOrDefault(q => q.Number == number);
            if (question is null)
            {
                return;
            }
            var replyMarkup = new InlineKeyboardMarkup
            {
                InlineKeyboard = new InlineKeyboardButton[][]{
                                        new InlineKeyboardButton[]{
                                            InlineKeyboardButton.SetCallbackData(question.Answer1, $"{number.ToString()}-A"),
                                            InlineKeyboardButton.SetCallbackData(question.Answer2, $"{number.ToString()}-B")

        }

        }
            };
            Bot.EditMessageText(new EditMessageTextArgs($"{number}.")
            {
                ChatId = message.Chat.Id,
                MessageId = message.MessageId,
ReplyMarkup = replyMarkup
            });

        }

        private void EditQueryMessage(CallbackQuery query, string text)
        {
            Bot.EditMessageText(new EditMessageTextArgs(text)
            {
                ChatId = query.Message.Chat.Id,
                  MessageId = query.Message.MessageId
            });
                    }

    protected override async void OnCallbackQuery(CallbackQuery callbackQuery)
        {
            _appUser = callbackQuery.From; // Save current user;
            var userId = _appUser.Id;
            if (!_userFacts.ContainsKey(userId))
            {
                Bot.AnswerCallbackQuery(callbackQuery.Id, "Error, user data not found.", true);
                EditQueryMessage(callbackQuery, "Invalid quiz. Restart.");
                return;
            }
            var data = callbackQuery.Data;
            if (data is null || data.Length <3)
            {
                Bot.AnswerCallbackQuery(callbackQuery.Id, "Error, invalid answer provided.", true);
                _userFacts.Remove(userId);
                EditQueryMessage(callbackQuery, "Invalid quiz. Restart.");
                return;
            }
            int number;
            var success = int.TryParse(data.Substring(0, data.IndexOf('-')), out number);
            if (!success || number<1 || _userFacts[userId] is null)
//                || _userFacts[userId].Count != number-1)
            {
                Bot.AnswerCallbackQuery(callbackQuery.Id, "Internal error.", true);
                _userFacts.Remove(userId);
                EditQueryMessage(callbackQuery, "Invalid quiz 3. Restart.");
                return;
                            }
            Bot.AnswerCallbackQuery(callbackQuery.Id);
            _userFacts[userId].Add(data);
            if (number == _questions.Count)
            {
                EditQueryMessage(callbackQuery, "All questions are answered. Checking results...");
                var resultsUtility = new ResultsUtility();
                var factSuccess = await resultsUtility.SetFacts(_userFacts[userId]);
                if (!factSuccess)
                {
                    Bot.SendMessage(_message.Chat.Id, "An error occured while uploading data to the server. Sorry for the inconvenience, try quiz later.");
                }
                else
                {
                    var interestColor = await GetColor(resultsUtility, true);
                    var styleColor = await GetColor(resultsUtility, false);
                    if (interestColor is null || styleColor is null)
                    {
                        Bot.SendMessage(_message.Chat.Id, "Cannot get information from server.");
                    }
                    else {
                        StringBuilder results = new StringBuilder();
                        results.AppendLine("After you complete The Princeton Review Career Quiz we will show you careers that match the \"style\" and \"interest\" colors you created. The colors have particular meanings:");
                        results.AppendLine("RED: Expediting");
                        results.AppendLine("GREEN: Communicating");
                        results.AppendLine("BLUE: Planning");
                        results.AppendLine("YELLOW: Administrating");
                        results.AppendLine("");
                        results.AppendLine("Your Interest");
                        results.AppendLine("\"Interests\" describe the types of activities that you are drawn to; these will need to be present in a job or career that you are considering if you are to stay motivated. It is important to note that interest in an activity does not necessarily indicate skill.");
                        results.AppendLine(interestColor);
                        string styleDescription, interestDescription;
                        switch (interestColor)
                        {
                            case "Red":
                                interestDescription = "People with red interests like hands-on / problem solving job responsibilities and professions that involve practical, technical, and objective activities. Red Interests include: building, implementing, organizing, producing, and delegating, which often lead to work in manufacturing, managing, directing, small business owning, and surgery.";
                                break;
                            case "Yellow":
                                interestDescription = "People with yellow interests choose careers that need organization, systematization, precision, dependence, and objectivity. They enjoy ordering, numbering, scheduling, systematizing, preserving, maintaining, measuring, specifying details, and archiving.These activities frequently lead to careers in research, banking, accounting, systems analysis, tax law, finance, government work, and engineering.";
                                break;
                            case "Blue":
                                interestDescription = "People with blue interests choose work duties and careers that call for creative, humanistic, reflective, and contemplative activities. Abstracting, theorizing, designing, writing, thinking, and originating new ideas are of interest for people who get blue interest color. It frequently leads to employment in editing, teaching, composing, inventing, mediating, clergy, and writing.";
                                break;
                            default:
                                interestDescription = "You are probably interested in careers and tasks that require convincing, selling, promoting, and interpersonal or group interactions. You may like motivating, mediating, marketing, persuading, delegating authority, entertaining, and lobbying. These interests frequently lead to careers in public relations, marketing, training, counseling, consulting, law, and other fields.";
                                break;
                        }
                        switch (styleColor)
                        {
                            case "Red":
                                styleDescription = "You probably perform your job responsibilities in an action-oriented manner. These people prefer to take action and seek results immediately. They excel in a self-organized, high - pressure, hierarchical, production - focused, competitive atmosphere and tend to be straightforward, forceful, rational, likable, authoritative, pleasant, direct, and resourceful.";
                                break;
                            case "Yellow":
                                styleDescription = "People with yellow styles perform their job responsibilities in a manner that is orderly and planned to meet a known schedule. They prefer to work where things get done with a minimum of interpretation and unexpected change. People with a yellow style tend to be orderly, cautious, structured, loyal, systematic, solitary, methodical, and organized, and usually thrive in a research-oriented, predictable, established, controlled, measurable, orderly environment.";
                                break;
                            case "Blue":
                                styleDescription = "People with blue styles try to carry out their duties in a way that is supportive and helpful to others and avoids conflict as much as possible. People with a blue style are frequently perceptive, intelligent, selectively social, creative, contemplative, emotional, imaginative, and sensitive. They often flourish in a cutting-edge, casual - paced, and future-focused workplace.Blue - style people like jobs that give them time to consider a situation before acting.";
                                break;
                            default:
                                styleDescription = "This style color suggests that the person is impulsive, chatty, personable, persuasive, risk-takers, and competitive. People with green styles prefer to operate in decisive environments. They often flourish in environments focused on teamwork, adventure, informality, innovation, and the big picture.";
                                break;
                        }
                        results.AppendLine(interestDescription);
                        results.AppendLine("Your Style");
                        results.AppendLine("\"Style\" describes the strengths that you could bring to a work environment when you are at your best. This is the way you like to get results. A work environment in which your strengths are appreciated is a big part of career satisfaction.");
                        results.AppendLine(styleColor);
                        results.AppendLine(styleDescription);
                        results.AppendLine("");
                        Bot.SendMessage(_message.Chat.Id, results.ToString());
                    }
                }
                    _userFacts.Remove(_appUser.Id);
            }
            else
            {
                UpdateMessage(callbackQuery.Message, _userFacts[userId].Count + 1);
            }
        }

        private async Task<string> GetColor(ResultsUtility resultsUtility, bool interest)
        {
            bool goalAchieved;
                var prefix = "Style";
                if (interest)
                {
                    prefix = "Interest";
                }
            goalAchieved = await resultsUtility.InferGoal($"Red{prefix}");
            if (goalAchieved)
            {
                return "Red";
            }
            goalAchieved = await resultsUtility.InferGoal($"Green{prefix}");
            if (goalAchieved)
            {
                return "Green";
            }
            goalAchieved = await resultsUtility.InferGoal($"Yellow{prefix}");
            if (goalAchieved)
            {
                return "Yellow";
            }
            goalAchieved = await resultsUtility.InferGoal($"Blue{prefix}");
            if (goalAchieved)
            {
                return "Blue";
            }
            return null;
        }
        }
        }