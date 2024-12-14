import java.util.Scanner;

/**
 * ValueSwap
 * CPT-236-W44
 * Coded by Russell Casad
 * Last Edited: September 9, 2024
 * Version 1.0
 */

public class ValueSwap {
    public static void main(String[] args) {

        int min = 0;
        int max = 99;
        int range = max - min + 1;
        int numA = 0; 
        int numB;
        boolean invalidAnswer = true; 

        try (Scanner scanner = new Scanner(System.in)) { // Declare new scanner with try, so it auto closes when finished.
            System.out.println("========================================");
            System.out.println("=              Value Swap              ="); // 38 whitespace
            System.out.println("========================================");
            while (invalidAnswer) { 
                System.out.print("Enter an integer between 0 and 99: "); // Prompts for an integer
                if(scanner.hasNextInt()){
                    numA = scanner.nextInt(); // Reads the integer
                    if (numA >= min && numA <= max) {
                        invalidAnswer = false; // only ends loop if user inputs a number AND is within limitations of the random number generator
                    }
                }
                if(invalidAnswer) {
                    System.out.println("========================================");
                    System.out.println("=            Invalid  Input            =");
                    System.out.println("========================================");
                }
                scanner.nextLine(); // pushes the scanner to the next line
            }

            numB = (int)(Math.random() * range) + min; // declares a random int
            System.out.println("========================================");
            System.out.println(String.format("= Inputted Number: %-20s", numA) + "="); // prints the inputted number
            System.out.println(String.format("= Random Number: %-22s", numB) + "="); // prints the randomly generated number
            System.out.println("========================================");
            if(numA > numB) { // swaps number A and B if A is larger than B
                int x = numA;
                numA = numB; 
                numB = x; 
            }
            System.out.println(String.format("=    %-2s is less than or equal to %-2s    =", numA, numB)); // Prints results
            System.out.println("========================================");
        }
    }
}