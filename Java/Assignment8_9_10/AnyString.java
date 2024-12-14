/*
* Russell Casad
* CPT-236-W44
* Last Modified: November 16, 2024
* Assignment 10
*/

import java.util.Scanner;

public class AnyString 
{
    public static void main(String[] args) 
    {
        String text, textToArray, equalsString; 
        char[] charArray; 
        try(var scanner = new Scanner(System.in))
        {
            // Prompts for the needed strings
            System.out.print("Enter text for " + TextFormat.color("String", TextFormat.Color.Purple, TextFormat.Color.None) + " test: ");
            text = scanner.nextLine();
            System.out.print("Enter text for " + TextFormat.color("Array", TextFormat.Color.Purple, TextFormat.Color.None) + " test: ");
            textToArray = scanner.nextLine(); 
            System.out.print("Enter text for " + TextFormat.color("Equality", TextFormat.Color.Purple, TextFormat.Color.None) + " tests: ");
            equalsString = scanner.nextLine(); 
            charArray = textToArray.toCharArray();

            // Creates the anystring variables
            var stringTest = new AnyString(text); 
            var arrayTest = new AnyString(charArray); 
            var equalsTest = new AnyString(equalsString);

            // test string
            System.out.println("\n" + TextFormat.color("String Tests:", TextFormat.Color.Purple, TextFormat.Color.None));
            System.out.println(">> To String:  " + TextFormat.color(stringTest.toString(), TextFormat.Color.Green, TextFormat.Color.None));
            System.out.println(">> Equals:     " + TextFormat.color(String.valueOf(stringTest.equals(equalsTest)), TextFormat.Color.Green, TextFormat.Color.None) + TextFormat.italics(" (" + stringTest.getString() + (stringTest.equals(equalsTest) ? " = " : " ≠ ") + equalsTest.getString() + ")"));
            System.out.println(">> Is Letters: " + TextFormat.color(String.valueOf(stringTest.isLetters()), TextFormat.Color.Green, TextFormat.Color.None));
            System.out.println(">> Is Numeric: " + TextFormat.color(String.valueOf(stringTest.isNumeric()), TextFormat.Color.Green, TextFormat.Color.None));
            System.out.println(">> Get String: " + TextFormat.color(stringTest.getString(), TextFormat.Color.Green, TextFormat.Color.None));
            System.out.println(">> Lowercase:  " + TextFormat.color(stringTest.lowercase(), TextFormat.Color.Green, TextFormat.Color.None));
            System.out.println(">> Uppercase:  " + TextFormat.color(stringTest.uppercase(), TextFormat.Color.Green, TextFormat.Color.None));
            System.out.println(">> Get Length: " + TextFormat.color(String.valueOf(stringTest.getLength()), TextFormat.Color.Green, TextFormat.Color.None));

            // test array
            System.out.println("\n" + TextFormat.color("Array Tests:", TextFormat.Color.Purple, TextFormat.Color.None));
            System.out.println(">> To String:  " + TextFormat.color(arrayTest.toString(), TextFormat.Color.Green, TextFormat.Color.None));
            System.out.println(">> Equals:     " + TextFormat.color(String.valueOf(arrayTest.equals(equalsTest)), TextFormat.Color.Green, TextFormat.Color.None) + TextFormat.italics(" (" + arrayTest.getString() + (arrayTest.equals(equalsTest) ? " = " : " ≠ ") + equalsTest.getString() + ")"));
            System.out.println(">> Is Letters: " + TextFormat.color(String.valueOf(arrayTest.isLetters()), TextFormat.Color.Green, TextFormat.Color.None));
            System.out.println(">> Is Numeric: " + TextFormat.color(String.valueOf(arrayTest.isNumeric()), TextFormat.Color.Green, TextFormat.Color.None));
            System.out.println(">> Get String: " + TextFormat.color(arrayTest.getString(), TextFormat.Color.Green, TextFormat.Color.None));
            System.out.println(">> Lowercase:  " + TextFormat.color(arrayTest.lowercase(), TextFormat.Color.Green, TextFormat.Color.None));
            System.out.println(">> Uppercase:  " + TextFormat.color(arrayTest.uppercase(), TextFormat.Color.Green, TextFormat.Color.None));
            System.out.println(">> Get Length: " + TextFormat.color(String.valueOf(arrayTest.getLength()), TextFormat.Color.Green, TextFormat.Color.None));
        }
    }

    //An single object/instance variable of type String.
    protected String text;

    //A constructor method that will accept a single String value and uses it to set the object/instance variable.
    public AnyString(String text) 
    {
        this.text = text; 
    }

    // A constructor method that will accept a character array argument. The character array argument should be used to set the object/instance variable.
    public AnyString(char[] text)
    {
        this.text = new String(text);
    }

    // An object/instance method toString() that accepts no arguments and returns the object/instance variable.
    @Override public String toString()
    {
        return text; 
    }

    // An object/instance method equals() that accepts an AnyString object and returns true if the argument contains a object/instance variable value that is equal to the object/instance variable value of the object calling the method or false if not equal.
    public boolean equals(AnyString input)
    {
        return this.text.equals(input.text); 
    }

    // An object/instance method isLetters() that accepts no arguments and returns true is the object/instance variable is all letters or false if not all letters.
    public boolean isLetters()
    {
        return !text.matches(".*[0-9].*");
    }

    // An object/instance method isNumeric() that accepts no arguments and returns true is the object/instance variable is all numbers or false if not all numbers.
    public boolean isNumeric()
    {
        try
        {
            Float.valueOf(text);
            return true;
        }
        catch (NumberFormatException e)
        {
            return false; 
        }
    }

    //An object/instance method getString() that accepts no argument and returns the object/instance variable value.
    public String getString()
    {
        return text; 
    }
    //An object/instance method lowercase() that accepts no arguments and returns a String value. The method will return the lowercase value of the object/instance variable.
    public String lowercase()
    {
        return text.toLowerCase(); 
    }
    //An object/instance method uppercase() that accepts no arguments and returns the uppercase value of the object/instance variable.
    public String uppercase()
    {
        return text.toUpperCase(); 
    }
    //An object/instance method getLength() that accepts no arguments and returns the length of the object/instance variable
    public int getLength()
    {
        return text.length(); 
    }
    // A method to change the string when desired
    public void changeString(String text)
    {
        this.text = text; 
    }

    
}
