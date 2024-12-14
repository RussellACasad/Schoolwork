/*
* Russell Casad
* CPT-236-W44
* Last Modified: November 16, 2024
* Assignment 10
*/

import java.util.Scanner;

final public class EncryptString extends AnyString
{

    public static void main(String[] args) {
        String input; 
        try(var scanner = new Scanner(System.in))
        {
            //You should input a string.
            System.out.print("Input a string: ");
            input = scanner.nextLine(); 
            //Create an object of the EncryptString class.
            var testObject = new EncryptString(input);
            //Call all the methods in the EncryptString class, verify that they work, and display the results.
            System.out.println("===      " + TextFormat.color("String Input", TextFormat.Color.Blue, TextFormat.Color.None) + "     ===");
            System.out.println("Encrypt String (Instance): " + TextFormat.color(testObject.encryptString(), TextFormat.Color.Purple, TextFormat.Color.None));
            var stringEncrypt = EncryptString.encryptString(testObject.getString());
            System.out.println("Encrypt String (Class):    " + TextFormat.color(stringEncrypt, TextFormat.Color.Green, TextFormat.Color.None));
            System.out.println("Decrypt String (Class):    " + TextFormat.color(EncryptString.decryptString(stringEncrypt), TextFormat.Color.Green, TextFormat.Color.None));
            //Create a character array and create an object of the EncryptString class.
            var testObjectArray = new EncryptString(input.toCharArray());
            //Call all the methods in the EncryptString class, verify that they work, and display the results.
            System.out.println("=== " + TextFormat.color("Character Array Input", TextFormat.Color.Blue, TextFormat.Color.None) + " ===");
            System.out.println("Encrypt String (Instance): " + TextFormat.color(testObjectArray.encryptString(), TextFormat.Color.Purple, TextFormat.Color.None));
            var stringEncryptArray = EncryptString.encryptString(testObjectArray.getString());
            System.out.println("Encrypt String (Class):    " + TextFormat.color(stringEncryptArray, TextFormat.Color.Green, TextFormat.Color.None));
            System.out.println("Decrypt String (Class):    " + TextFormat.color(EncryptString.decryptString(stringEncryptArray), TextFormat.Color.Green, TextFormat.Color.None));
        }
    }

    //Create two constructors for the EncryptString class. Each of the constructors should match one of the 
    // constructor methods in the AnyString class.
    public EncryptString(String text) 
    {
        super(text);
    }

    public EncryptString(char[] text)
    {
        super(text);
    }
    /* 
    Add a object/instance method named encryptString() that accepts no arguments and returns a String. 
    This method should encrypt the object/instance variable. This method will be a modified version of the method 
    created for the previous assignment.
    */
    public String encryptString() // Caeser Encode 
    {
        var output = "";
        text = text.toLowerCase(); 

        for (var x : text.toCharArray()) {
            if(x == ' ')
            {
                output += Character.toString(x);
            } else {
				int y;
				y = x + 1; // shifts the letter by the inputted number

                if(y > 'z') // if the shift extends past the alphabet, loops the encoding to ensure only alphabetic characters are used.
                {
                    y = (char)(('a' - 1) + (y - 'z'));
                }
                output += Character.toString(y);
            }
        }
        return output; // returns the shifted string
    }
    /* Add a class/static method named encryptString() that accepts a String argument and returns an encrypted 
    String object. This method can be found in a previous assignment.
    */
    public static String encryptString(String input) // Caeser Encode 
    {
        var output = "";
        input = input.toLowerCase(); 

        for (var x : input.toCharArray()) {
            if(x == ' ')
            {
                output += Character.toString(x);
            } else {
				int y;
				y = x + 1; // shifts the letter by the inputted number

                if(y > 'z') // if the shift extends past the alphabet, loops the encoding to ensure only alphabetic characters are used.
                {
                    y = (char)(('a' - 1) + (y - 'z'));
                }
                output += Character.toString(y);
            }
        }
        return output; // returns the shifted string
    }

    /*
    //Add a class/static method named decryptString() that accepts an encrypted String argument and returns 
    a decrypted String object. This methods can be found in a previous assignment.
    */
    public static String decryptString(String input) // Decode Caeser
    {
        var output = ""; 
        input = input.toLowerCase(); 

        for (var x : input.toCharArray()) {
            if(x == ' ') // spaces don't get shifted
            {
                output += x;
            } else {
				int y; // shifts the letter back to how it should be
				y = x - 1; 

                if(y < 'a') // loops the letter if needbe
                {
                    y = (char)(('z' + 1) - ('a' - y));
                }
                output += Character.toString(y);
            }
        }
        return output;
    }

    }