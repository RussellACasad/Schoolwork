import java.util.Scanner;

/*
 * Assignment2
 * Coded by Russell Casad
 * Last Edited: September 1, 2024
 * Version: 1.0
 */

public class DecimalFloor {
    public static void main(String[] args) 
    {
        boolean isRepeating; 
        try (Scanner scanner = new Scanner(System.in)) // Declare a new scanner object using try-with-resources, so the scanner is automatically closed when done.
        {
            System.out.println("========================================="); // prints this first so it's not repeated.
            System.out.println("=         Decimal Floor Program         =");
            System.out.println("=========================================");
            do
            {
                boolean validInput = false; // Declares a boolean to be checked as TRUE once the user inputs a valid number
                Double userInput = 0.0; // Declaring the user input so it's not in the scope of the DO/WHILE loop
                do{ // DO/WHILE exists to see if the user input is valid
                    System.out.print("Enter a Decimal Number: "); // Prompts the user for a decimal number
                    if(scanner.hasNextDouble()) // if the scanner detects a double
                    {
                        userInput = scanner.nextDouble(); // set userInput to the inputted double (2)
                        scanner.nextLine(); // nextDouble doesn't end the next line, this will. Prevents unexpected results when asking to repeat. 
                        validInput = true; // declares the input as valid
                    }
                    else // if the scanner does not detect a double
                    {
                        scanner.nextLine(); // scanner advances without specifying a data type
                        System.out.println(""); // skips a line for spacing
                        System.out.println("=========================================");
                        System.out.println("=         Invalid Data Entered!         ="); // tells the user that their data inputted is invalid
                        System.out.println("=========================================");
                        System.out.println(""); // skips a line for spacing
                    }
                } while(!validInput);
                int outputInt = userInput.intValue(); // converts the userInput double to an INT - dropping the decimal (3)
                double outputDec = outputInt; // Converts the int back into a double
                System.out.println(""); // skips a line for spacing
                if (userInput % 1.0 == 0) {
                    System.out.println("=========================================");
                    System.out.println("=         No Decimal to remove.         =");
                    System.out.println("=========================================");
                    System.out.println(String.format("= Input: %-30s =", userInput));
                    System.out.println("=========================================");
                } else // outputs user input and dropped decimal if there is a decimal
                {
                    System.out.println("=========================================");
                    System.out.println(String.format("= With Decimal: %-23s =", userInput)); // outputs the user input -- the Double (4)
                    System.out.println(String.format("= Without Decimal: %-20s =", outputDec)); // outputs the input converted to int then back to double (5)
                    System.out.println("=========================================");
                }
                
                System.out.println("Do again? [Y/n]: "); // asks if user wants to do another number
                String doAgain = scanner.nextLine(); // reads if user wants to do another
                isRepeating = "y".equals(doAgain.trim().toLowerCase()); // only loops if user says "y", otherwise ends the program
                System.out.println(""); // skips a line for spacing 
            } while(isRepeating);
        } // scanner CLOSED
        System.out.println("=========================================");
        System.out.println("=            Program  Closed            =");
        System.out.println("=========================================");
    }
}