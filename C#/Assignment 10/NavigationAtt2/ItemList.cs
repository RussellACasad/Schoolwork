using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationAtt2
{

    public class ItemList
    {
        public List<Item> Items { get; } = new List<Item>();

        public void MakeList()
        {
            Items.Add(new Item
            {
                Name = "Health Potion",
                HealthChange = 35,
                EatText = "Tastes like grapes!",
                GetText = "Why is it purple...?",
                EatOrDrink = "drink"
            });

            Items.Add(new Item
            {
                Name = "Green Apple",
                HealthChange = 25,
                EatText = "Still Crunchy!",
                GetText = "How is this still fresh..?",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "Lime",
                HealthChange = 25,
                EatText = "Squishy, but refreshing!",
                GetText = "Not even a lemon...",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "Lemon",
                HealthChange = 25,
                EatText = "Ewww! Sour!",
                GetText = "Not even a lime...",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "Unknown Gameboy Game",
                HealthChange = 25,
                EatText = "The junk on the cartridge fills you up",
                GetText = "If only it had a label...",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "Milkshake",
                HealthChange = 25,
                EatText = "Still cold! Not sure if that's concerning...",
                GetText = "Somehow not milk soup!",
                EatOrDrink = "drink"
            });

            Items.Add(new Item
            {
                Name = "Carton of McDonald's Fries",
                HealthChange = 25,
                EatText = "Rubbery!",
                GetText = "They're cold.",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "Bruised Banana",
                HealthChange = 25,
                EatText = "It tastes as bad as it looks...",
                GetText = "Might not taste very good...",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "Purple GTC Promotional Bracelet",
                HealthChange = 50,
                EatText = "The promotional aspect gets to you!",
                GetText = "Very promotional.",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "Pringles Can",
                HealthChange = 25,
                EatText = "The crumbs taste good too!",
                GetText = "There are crumbs in it",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "Rock",
                HealthChange = 15,
                EatText = "Wait, did you just...",
                GetText = "It's a rock",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "Half a brick",
                HealthChange = 15,
                EatText = "Crunky",
                GetText = "You're so hungry you consider eating it...",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "Dr. Pepper",
                HealthChange = 30,
                EatText = "It's flat.",
                GetText = "Fizzy!",
                EatOrDrink = "drink"
            });

            Items.Add(new Item
            {
                Name = "Chug Jug",
                HealthChange = 100,
                EatText = "We can be pro Fortnite gamers!",
                GetText = "I really want to Chug Jug with you...",
                EatOrDrink = "drink"
            });

            Items.Add(new Item
            {
                Name = "Handful of Spiders",
                HealthChange = 25,
                EatText = "Don't worry, they like it.",
                GetText = "That's a lot of spiders for one chest...",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "3.39oz of bottled spoiled milk",
                HealthChange = 20,
                EatText = "At least it's airline compliant?",
                GetText = "There's little good to be said",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "A book of sheet music",
                HealthChange = 25,
                EatText = "Tasted like paper",
                GetText = "What is this even good for..?",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "Box of Kraft Mac and Cheese",
                HealthChange = 25,
                EatText = "Crunchy!",
                GetText = "Nothing to cook it with though...",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "1 vegan marshmallow",
                HealthChange = 15,
                EatText = "Firm but still tastes good",
                GetText = "At least it's vegan!",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "A bundle of carrot sticks",
                HealthChange = 25,
                EatText = "Crunchy!",
                GetText = "They're a bit old...",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "Chick-Fil-A Dipping sauce bottle",
                HealthChange = 25,
                EatText = "Tastes like dressing.",
                GetText = "Not even any nuggets!",
                EatOrDrink = "drink"
            });

            Items.Add(new Item
            {
                Name = "Cooked Chicken Chunks",
                HealthChange = 25,
                EatText = "They're cold...",
                GetText = "The breading is only slightly falling off",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "can of Liquid Death Sparkling Water",
                HealthChange = 25,
                EatText = "Refreshing!",
                GetText = "Sustainable, recyclable water packaging! Cool! (Not Sponsored)",
                EatOrDrink = "drink"
            });

            Items.Add(new Item
            {
                Name = "Container of Blue Diamond Almonds",
                HealthChange = 25,
                EatText = "Almonds are still gross...",
                GetText = "Almonds are gross...",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "Single-Serve bag of M&Ms",
                HealthChange = 25,
                EatText = "The green ones taste the best!",
                GetText = "I hope there's green ones...",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "Sticker",
                HealthChange = 10,
                EatText = "Scratches your mouth up and makes you choke a little while swallowing...",
                GetText = "It's shaped like a frog!",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "Small bottle of Ranch Dressing",
                HealthChange = 25,
                EatText = "You slammed that ranch!",
                GetText = "Lukewarm! how I like it. ",
                EatOrDrink = "drink"
            });

            Items.Add(new Item
            {
                Name = "Mystic Elixir",
                HealthChange = 50,
                EatText = "You feel a surge of energy!",
                GetText = "You found a Mystic Elixir.",
                EatOrDrink = "drink"
            });

            Items.Add(new Item
            {
                Name = "Healing Apple",
                HealthChange = 30,
                EatText = "Your wounds start to mend.",
                GetText = "You picked up a Healing Apple.",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "Ethereal Potion",
                HealthChange = 40,
                EatText = "You feel weightless for a moment.",
                GetText = "You obtained an Ethereal Potion.",
                EatOrDrink = "drink"
            });

            Items.Add(new Item
            {
                Name = "Savory Bread",
                HealthChange = 20,
                EatText = "Your hunger is satisfied.",
                GetText = "You acquired Savory Bread.",
                EatOrDrink = "eat"
            });

            Items.Add(new Item
            {
                Name = "Firewater",
                HealthChange = 60,
                EatText = "You breathe out a burst of flames!",
                GetText = "You stumbled upon Firewater.",
                EatOrDrink = "drink"
            });

        }

    }
}
