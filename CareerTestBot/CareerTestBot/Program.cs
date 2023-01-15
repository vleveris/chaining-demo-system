using System;
using System.Linq;
using CareerTestBot.Models;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.GettingUpdates;
namespace CareerTestBot
{
    class Program
    {
        static void Main()
        {
            CareerBot.Bot.SetMyCommands(new BotCommand("quiz", "New quiz"));
			CareerBot.Bot.DeleteWebhook();
			// Long Polling: Start
			var updates = CareerBot.Bot.GetUpdates();
			var botInstance = new CareerBot();
			while (true)
			{
				if (updates.Any())
				{
					foreach (var update in updates)
					{
						botInstance.OnUpdate(update);
					}
					var offset = updates.Last().UpdateId + 1;
					updates = CareerBot.Bot.GetUpdates(offset);
				}
				else
				{
					updates = CareerBot.Bot.GetUpdates();
				}
			}
			// Long Polling: End
		}
	}
}
