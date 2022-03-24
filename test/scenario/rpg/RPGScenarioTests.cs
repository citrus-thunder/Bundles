using System.Collections.Generic;

using Xunit;

using Bundles.Test.Scenarios.RPG;

namespace Bundles.Test
{
	[Collection("Unique: Bundler Generation")]
	public class RPGScenarioTests
	{
		private const string XML_ROOT = "scenario/rpg/";

		[Theory]
		[InlineData("rpg.xml")]
		public void RunScenario(string file)
		{
			var playerName = "Rincewind";
			var currency = 12;
			var experience = 25;
			var hp = 100;

			var quests = new List<(string id, string name, string description)>
			{
				("0", "Tutorial", "A Tutorial Quest"),
				("1", "Delivery", "Take this wheat to the tavern"),
				("2", "Trouble!", "Clear the cave of kobolds")
			};

			var inventory = new List<(string id, string name, string description, int count)>
			{
				("0", "Potion", "A Potion", 10),
				("1", "Ration", "Some tough rations", 5),
				("2", "Map", "A tattered map of the area", 1)
			};

			var xmlFile = XML_ROOT + file;
			var bundler = TestConfig.GetBundler(xmlFile);

			var player = bundler.Data["Save/0/PlayerData"] as PlayerData;
			player.PlayerName.Value = playerName;
			player.Currency.Value = currency;
			player.Experience.Value = experience;
			player.HP.Value = hp;

			foreach (var quest in quests)
			{
				var q = player.Quests[quest.id];
				q.QuestName.Value = quest.name;
				q.Description.Value = quest.description;
			}

			foreach (var item in inventory)
			{
				var i = player.Items[item.id];
				i.ItemName.Value = item.name;
				i.Description.Value = item.description;
				i.Count.Value = item.count;
			}

			player.Save();

			var _bundler = TestConfig.GetBundler(xmlFile);
			var _player = _bundler.Data["Save/0/PlayerData"] as PlayerData;
			_player.Load();

			Assert.Equal(playerName, _player.PlayerName.Value);
			Assert.Equal(currency, _player.Currency.Value);
			Assert.Equal(experience, _player.Experience.Value);
			Assert.Equal(hp, _player.HP.Value);

			foreach (var quest in quests)
			{
				var q = _player.Quests[quest.id];
				Assert.Equal(quest.name, q.QuestName.Value);
				Assert.Equal(quest.description, q.Description.Value);
			}

			foreach (var item in inventory)
			{
				var i = _player.Items[item.id];
				Assert.Equal(item.name, i.ItemName.Value);
				Assert.Equal(item.description, i.Description.Value);
				Assert.Equal(item.count, i.Count.Value);
			}
		}
	}
}