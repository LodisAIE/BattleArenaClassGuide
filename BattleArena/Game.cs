using System;
using System.Collections.Generic;
using System.Text;


// <summary>
/// Represents any entity that exists in game
/// </summary>
struct Character
{
    public string name;
    public float health;
    public float attackPower;
    public float defensePower;
}

class Game
{
    bool gameOver;
    int currentScene;
    Character player;
    Character[] enemies;
    private int currentEnemyIndex;
    private Character currentEnemy;

    /// <summary>
    /// Function that starts the main game loop
    /// </summary>
    public void Run()
    {
        Start();

        while (!gameOver)
        {
            Update();
        }

        End();
    }

    /// <summary>
    /// Function used to initialize any starting values by default
    /// </summary>
    public void Start()
    {
        gameOver = false;
        currentScene = 0;
        currentEnemyIndex = 0;

        Character fraawg = new Character { name = "Fraawg", health = 42590, attackPower = 15, defensePower = 573 };

        Character sassafrazzz = new Character { name = "Sassafrazzz", health = 745, defensePower = 1738, attackPower = 5907 };

        Character wompus = new Character { name = "Wompus With Gun", health = 999, attackPower = 55, defensePower = 3000 };

        enemies = new Character[] { fraawg, sassafrazzz, wompus };

        currentEnemy = enemies[currentEnemyIndex];
    }

    /// <summary>
    /// This function is called every time the game loops.
    /// </summary>
    public void Update()
    {
        DisplayCurrentScene();
    }

    /// <summary>
    /// This function is called before the applications closes
    /// </summary>
    public void End()
    {
        Console.WriteLine("Guh bah");
        Console.ReadKey(true);
    }

    /// <summary>
    /// Gets an input from the player based on some given decision
    /// </summary>
    /// <param name="description">The context for the input</param>
    /// <param name="option1">The first option the player can choose</param>
    /// <param name="option2">The second option the player can choose</param>
    /// <returns></returns>
    int GetInput(string description, string option1, string option2)
    {
        string input = "";
        int inputReceived = 0;

        while (inputReceived != 1 && inputReceived != 2)
        {//Print options
            Console.WriteLine(description);
            Console.WriteLine("1. " + option1);
            Console.WriteLine("2. " + option2);
            Console.Write("> ");

            //Get input from player
            input = Console.ReadLine();

            //If player selected the first option...
            if (input == "1" || input == option1)
            {
                //Set input received to be the first option
                inputReceived = 1;
            }
            //Otherwise if the player selected the second option...
            else if (input == "2" || input == option2)
            {
                //Set input received to be the second option
                inputReceived = 2;
            }
            //If neither are true...
            else
            {
                //...display error message
                Console.WriteLine("Invalid Input");
                Console.ReadKey();
            }

            Console.Clear();
        }
        return inputReceived;
    }

    /// <summary>
    /// Calls the appropriate function(s) based on the current scene index
    /// </summary>
    void DisplayCurrentScene()
    {
        switch (currentScene)
        {
            case 0:
                GetPlayerName();
                break;
            case 1:
                CharacterSelection();
                break;
            case 2:
                Battle();
                CheckBattleResults();
                break;
            case 3:
                DisplayMainMenu();
                break;

        }

    }

    /// <summary>
    /// Displays the menu that allows the player to start or quit the game
    /// </summary>
    void DisplayMainMenu()
    {
        int choice = GetInput("Play Again?", "Yes", "No");

        if (choice == 1)
        {
            currentScene = 0;
            currentEnemyIndex = 0;
            currentEnemy = enemies[currentEnemyIndex];
        }
        else if (choice == 2)
        {
            gameOver = true;
        }
    }

    /// <summary>
    /// Displays text asking for the players name. Doesn't transition to the next section
    /// until the player decides to keep the name.
    /// </summary>
    void GetPlayerName()
    {
        Console.WriteLine("Please enter your name.");
        Console.Write("> ");
        player.name = Console.ReadLine();

        Console.Clear();

        int choice = GetInput("You've entered " + player.name + ". Are you sure you want to keep this name?", "Yes", "No");

        if (choice == 1)
        {
            currentScene++;
        }
    }

    /// <summary>
    /// Gets the players choice of character. Updates player stats based on
    /// the character chosen.
    /// </summary>
    public void CharacterSelection()
    {
        int choice = GetInput("Nice meet you " + player.name + ". Please select a character.", "Wizard", "Knight");

        if (choice == 1)
        {
            player.health = 50;
            player.attackPower = 25000000;
            player.defensePower = 5000000;
            currentScene++;
        }
        else if (choice == 2)
        {
            player.health = 75;
            player.attackPower = 15;
            player.defensePower = 10;
            currentScene++;
        }
    }

    /// <summary>
    /// Prints a characters stats to the console
    /// </summary>
    /// <param name="character">The character that will have its stats shown</param>
    void DisplayStats(Character character)
    {
        Console.WriteLine("Name: " + character.name);
        Console.WriteLine("Health: " + character.health);
        Console.WriteLine("Attack Power: " + character.attackPower);
        Console.WriteLine("Defense Power: " + character.defensePower);
        Console.WriteLine();
    }

    /// <summary>
    /// Calculates the amount of damage that will be done to a character
    /// </summary>
    /// <param name="attackPower">The attacking character's attack power</param>
    /// <param name="defensePower">The defending character's defense power</param>
    /// <returns>The amount of damage done to the defender</returns>
    float CalculateDamage(float attackPower, float defensePower)
    {
        float damageTaken = attackPower - defensePower;

        if (damageTaken < 0)
        {
            damageTaken = 0;
        }

        return damageTaken;
    }

    /// <summary>
    /// Deals damage to a character based on an attacker's attack power
    /// </summary>
    /// <param name="attacker">The character that initiated the attack</param>
    /// <param name="defender">The character that is being attacked</param>
    /// <returns>The amount of damage done to the defender</returns>
    public float Attack(ref Character attacker, ref Character defender)
    {
        float damageTaken = CalculateDamage(attacker.attackPower, defender.defensePower);

        defender.health -= damageTaken;

        if (defender.health < 0)
        {
            defender.health = 0;
        }

        return damageTaken;
    }

    /// <summary>
    /// Simulates one turn in the current monster fight
    /// </summary>
    public void Battle()
    {
        float damageDealt = 0;

        DisplayStats(player);
        DisplayStats(currentEnemy);

        int choice = GetInput("A " + currentEnemy.name + " stands in front of you! What will you do?", "Attack", "Dodge");

        if (choice == 1)
        {
            damageDealt = Attack(ref player, ref currentEnemy);
            Console.WriteLine("You dealt " + damageDealt + " damage!");
        }
        else if (choice == 2)
        {
            Console.WriteLine("You dodged the enemy's attack!");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        damageDealt = Attack(ref currentEnemy, ref player);
        Console.WriteLine("The " + currentEnemy.name + " dealt " + damageDealt, " damage!");

        Console.ReadKey(true);
        Console.Clear();
    }

    /// <summary>
    /// Checks to see if either the player or the enemy has won the current battle.
    /// Updates the game based on who won the battle..
    /// </summary>
    void CheckBattleResults()
    {
        if (player.health <= 0)
        {
            Console.WriteLine("You were slain...");
            Console.ReadKey(true);
            Console.Clear();
            currentScene = 3;
        }
        else if (currentEnemy.health <= 0)
        {
            Console.WriteLine("You slayed the " + currentEnemy.name);
            Console.ReadKey();
            Console.Clear();
            currentEnemyIndex++;

            if (currentEnemyIndex >= enemies.Length)
            {
                currentScene = 3;
                Console.WriteLine("You've slain all the enemies! You are a true warrior.");
                return;
            }

            currentEnemy = enemies[currentEnemyIndex];
        }

    }

}
