/*
 * Russell Casad
 * CPT-236-W44
 * Assignment 7
 * Last Edited: October 20, 2024
 */

import java.util.Random;

public  class RandomDistributionCheck
{
    @SuppressWarnings("unused")
    public static void main(String[] args) 
    {
        final int numberOfIterations = 10000000; // Number of times a random number will be picked. Lower numbers show more variation in percent. (Default: 10000000)
        final int numberOfOptions = 20; // Amount of numbers that can be picked. Includes 0. (20 = 0-19) (Default: 20)
        final int numberOfRows = 25; // number of rows shown in the graph. The higher the number, the more precise the graph will be. (Default: 25)
        final String leftLabel = "Number of Occurances"; // lavel for Y axis (Default: "Number of Occurances")
        final String bottomLabel = "Number"; // label for X axis (Default: "Number")

        if (numberOfIterations < 1)
        {
            System.out.println("Number of Iterations must be 1.");
        }
        else if (numberOfOptions < 1)
        {
            System.out.println("Number of Options must be 1.");
        }
        else if (numberOfRows < 1)
        {
            System.out.println("Number of Rows must be 1.");
        }

        int[] numbers = new int[numberOfOptions]; // declares an array of numbers
        Random random = new Random(); // declares a random var

        for(var x = 0; x < numberOfIterations; x++)
        {
            var y = random.nextInt(numberOfOptions); // declares a random int between 0 and numberOfOptions - 1 
            numbers[y]++; // adds one to the number array for the random number to tally how many times that number has been called
        }

        var label = 0;  // declares a number to label the output
        System.out.println("┌────────────────────────────────┐");
        System.out.println("│     Random Number Counter      │");
        System.out.println("├────────────────────────────────┤");
        System.out.println(String.format("│  Sample size: %-16s │", numberOfIterations));
        System.out.println("├─────┬────────────────┬─────────┤");
        System.out.println("│ Num │     Amount     │ Percent │");
        System.out.println("├─────┼────────────────┼─────────┤");
        for (var number : numbers) 
        {
            double percent = ((double)number * 100) / (double)numberOfIterations;
            System.out.println(String.format("│ %-3s │ %-14s │ %6.2f%% │", label, number, percent)); // prints the number, then the number of occurences
            label++; 
        }
        System.out.println("└─────┴────────────────┴─────────┘");
        graphPrinter(numbers, numberOfRows, leftLabel, bottomLabel);
    }

    public static void graphPrinter(int[] input, int numberOfRows, String leftLabel, String bottomLabel)
    {
        int largest = 0; 
        for (var i : input) // finds the largest number in the array
        {
            if (i >= largest)
            {
                largest = i; 
            }
        }

        var largestNumLength = String.valueOf(largest).length(); // gets the length of the largest num as a string for display formatting
        largestNumLength = largestNumLength  == 1 ? 2 : largestNumLength;


        /* Graph Prepping */

        var topBar = "┌───┬─" + "─".repeat(largestNumLength) + "─┬" + "───┬".repeat(input.length - 1) + "───┐"; // Makes the top bar
        var roundModifier = largestNumLength * 10; // automatically deciphers how to round up the numbers for a clean graph, all numbers end in 0
        var largestRounded = ((largest + roundModifier - 1) / roundModifier) * roundModifier; // rounds largest number to the nearest number that ends in all 0's 
        var interval = (largestRounded > numberOfRows) ? (largestRounded / numberOfRows) : 1; // gets the interval for the selected number of rows
        var leftLabelTop = numberOfRows / 2 + leftLabel.length() / 2;  // finds the top letter index for the label
        var leftLabelBot = leftLabelTop - leftLabel.length() + 1;  // finds the bottom letter index for the label
        var labelArray = leftLabel.toCharArray(); 
        
        /* Graph Printing */

        System.out.println("");
        System.out.println(topBar);
        for(var x = numberOfRows; x >= 0; x--) // for each row...
        {
            var num = x * interval; // ... get the interval we're on... 
            var line = String.format("│ " + (x >= leftLabelBot && x <= leftLabelTop ? labelArray[leftLabelTop - x] : " ") +" │ %" + largestNumLength + "s ", num); // ...then makes a string with the interval...
            for (int i : input) {//... and for each input number...
                line += (i >= num) ? (i >= num + interval) ? "┼ █ " : "┼ ▄ " : "┼   "; // ... adds to the string a part of the bar if the number was picked interval amount of times or less
            }
            line += "│";// Adds a final bar onto the graph
            System.out.println(line); // prints the line
        }
        var line = "│" + " ".repeat(largestNumLength + 5) + " │"; // starts the last graph line
        var divider = "├───┴─" + "─".repeat(largestNumLength) + "─┼"; // starts the divider to go above the bottom line
        for(var x = 0; x < input.length; x++) // for each number in input again, this time just to label the bottom numbers
        {
            line += String.format("%3s│", x); // adds each bottom number with the space of 3 chars
             // adds a divider segment per number
        }
        divider += "───┼".repeat(input.length - 1) + "───┤";
        System.out.println(divider); // outputs the divider
        System.out.println(line); // outputs the labels
        var bottomTextSpacer = " ".repeat(((line.length() / 2) - (bottomLabel.length() / 2)) - 1); // creates a spacer to center the bottom label
        var bottomTextLine = "│" + bottomTextSpacer + bottomLabel + bottomTextSpacer + (bottomLabel.length() % 2 == 0 && largestNumLength % 2 == 1? " │" : "│");
        System.out.println("├─────" + "─".repeat(largestNumLength) + "─┴" + "───┴".repeat(input.length - 1) + "───┤"); // prints the bottom label // 
        System.out.println(bottomTextLine);
        System.out.println("└" + "─".repeat(bottomTextLine.length() - 2) + "┘");

    }
}