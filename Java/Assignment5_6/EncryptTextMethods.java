/*
 * 
 * CPT-236-W44
 * Assignment 6
 * Coded by Russell Casad
 * Last Edited: October 17, 2024
 * 
 */
import java.util.Scanner;


class EncryptTextMethods
{

    public enum CipherType 
    { // enum of the type of cipher to ease in consistency
        Caeser,
        Viginere, 
        Bacon, 
        Atbash, 
        Multi, 
        RailFence; 
    }

    public static void main(String[] args) 
    {
        String option, message, newMessage; // Declare variables
        CipherType cipher = CipherType.Caeser; // Stores the type of cipher being used

        try(var scanner = new Scanner(System.in)) 
        {// Declare a scaner with try to close when done
            while(true)
            {
                option = "";  // clears the option selection for repeat uses
                while (!option.equals("1") && !option.equals("2"))
                {
                    var cipherString = cipher + (String.valueOf(cipher).length() % 2 == 0 ? "  " : " ") + "Cipher"; // assembles the title string
                    var padding = (30 - cipherString.length()) / 2; // gets the spacing for padding
                    System.out.println("\n");
                    System.out.println("==================================");
                    System.out.println("= " + " ".repeat(padding) + cipherString + " ".repeat(padding) + " ="); // prints the title 
                    System.out.println("=================================="); // Present the user options
                    if(cipher == CipherType.Atbash)
                    {
                        System.out.println("=  1. Encode/Decode              =");
                    }
                    else
                    {
                        System.out.println("=  1. Encode                     =");
                        System.out.println("=  2. Decode                     =");
                    }
                    System.out.println("=  0. Change Cipher Type         =");
                    System.out.println("==================================");
                    System.out.println("= **. Quit                       =");
                    System.out.println("==================================");
                    System.out.print("Selection: ");
                    option = scanner.nextLine().trim(); // user inputs whether to encode or decode
                    switch(option)
                    {
                        case "1" -> {}
                        case "2" -> { if(cipher == CipherType.Atbash) printInvalid(); continue;} 
                        case "0" -> cipher = changeCipher(scanner); 
                        case "**" -> System.exit(0);
                        default -> printInvalid(); 
                    }
                }

                message = getInput(scanner, cipher, option);

                newMessage = switch (cipher) { // runs the correct cipher algorithm based on the selected cipher and options
                    case Viginere -> option.equals("1") ? PasswordEncode(message, scanner) : PasswordDecode(message, scanner);
                    case Bacon -> option.equals("1") ? BaconEncode(message, scanner) : BaconDecode(message, scanner); 
                    case Caeser -> option.equals("1") ? CaeserEncode(message, scanner) : CaeserDecode(message, scanner);
                    default -> AtbashDecodeEncode(message);
                };

                Output(message, newMessage, scanner); // prints the coded message
            }
        }
    }

    public static String getInput(Scanner scanner, CipherType cipher, String option) // takes the message input to encode/decode
    {
        String output, note; 
        
        note = "= NOTE: Numbers and special characters will be removed. ="; // Prompts the user for the message to encode / decode
        System.out.println("\n");
        System.out.println("=".repeat(note.length()));
        System.out.println(note);
        System.out.println("=".repeat(note.length()));
        System.out.print("Message to " + (option.equals("1") ? "Encode" : "Decode")  + ": ");
        output = scanner.nextLine().trim().replace("[^a-zA-Z\s]]", "");

        if(!(cipher == CipherType.Bacon && option.equals("2")))
        {
            output = output.toLowerCase();
        }

        return output; 
    }

    public static CipherType changeCipher(Scanner scanner) // Outputs a list of all ciphers and returns the one the user selects
    {
        CipherType output = null; 
        while(output == null)
            {
                System.out.println("\n");
                System.out.println("==================================");
                System.out.println("=         Cipher  Select         =");
                System.out.println("==================================");
                System.out.println("= 1. Caeser                      ="); 
                System.out.println("= 2. Viginere                    =");
                System.out.println("= 3. Bacon                       =");
                System.out.println("= 4. Atbash                      =");
                System.out.println("==================================");
                System.out.print("Selection: ");
                var in = scanner.nextLine().trim(); 
                switch (in) // takes the user input and outputs the selection 
                {
                    case "1" -> output = CipherType.Caeser; 
                    case "2" -> output = CipherType.Viginere; 
                    case "3" -> output = CipherType.Bacon;
                    case "4" -> output = CipherType.Atbash; 
                    default -> printInvalid(); 
                }
            }
        return output; 
    }

    public static void Output(String message, String coded, Scanner scanner) // Outputs the input and output
    {
        System.out.println("\n");
        System.out.println("==- Input -=======================");
        System.out.println(longTextFormatter(message));
        System.out.println("==- Output -======================");
        System.out.println(longTextFormatter(coded));
        System.out.println("==================================");
        System.out.print("Press enter to continue..."); 
        scanner.nextLine(); 
    }

    public static void printInvalid()
    {
        System.out.println("\n");
        System.out.println("==================================");
        System.out.println("=         Invalid Input.         =");
        System.out.println("==================================");
    } 

    public static String longTextFormatter(String input) // formats my output strings
    {
        if (input.length() < 30) // if inputted string is less that 30 chars, format and return 
        {
            return String.format("= %-30s =", input);
        }
        else // if greater than 30 chars, split among multuple lines
        {
            var count = 0;
            var array = input.toCharArray(); 
            var output = "= "; 
            for(var x : array) // for each character in the string, append it to the output
            {
                output += String.valueOf(x);
                count++; 
                if(count == 30) // if we hit char 30, add a new line with some fancy formatting, then reset the count for another line
                {
                    count = 0; 
                    output += " =\n= ";
                }
            }
            var outputArray = output.split("\n"); // converts final amount to an array, using new lines to split them. This is to format the last line
            output += " ".repeat(32 - outputArray[outputArray.length - 1].length()) + " ="; // I know this is inefficient but i can't figure it out better ;w;
            return output; 
        }
    }
    
    /* === ENCODE === */

    public static String CaeserEncode(String input, Scanner scanner) // Caeser Encode 
    {
        var number = 0; 
        var output = "";
         
        while(true)
        {
            System.out.println("");
            System.out.print("Shift (1-25): "); // prompts the user for the shift of the message
            if (scanner.hasNextInt()) // ensures the input is a number
            {
                number = scanner.nextInt(); // if input is number, set the number var
                scanner.nextLine();
            }
            else {
                printInvalid();
                scanner.next();
                continue;
            }
            if((number < 1 || number > 25)) // ensures number is between 1 and 25
            {
                printInvalid();
            }
            else break;
        }

        for (var x : input.toCharArray()) {
            if(x == ' ')
            {
                output += Character.toString(x);
            } else {
				int y;
				y = x + number; // shifts the letter by the inputted number

                if(y > 'z') // if the shift extends past the alphabet, loops the encoding to ensure only alphabetic characters are used.
                {
                    y = (char)(('a' - 1) + (y - 'z'));
                }
                output += Character.toString(y);
            }
        }
        return output; // returns the shifted string
    }

    public static String PasswordEncode(String input, Scanner scanner) // Password Encode
    {
		var passwordIndex = 0; 
        int number; 
        var output = ""; 
        var password = ""; 

        while (true) 
        {
            System.out.println("");
            System.out.println("=========================================================");
            System.out.println("= NOTE: Numbers and special characters will be removed. =");
            System.out.println("=========================================================");
            System.out.print("Password: ");
            password = scanner.nextLine().trim().toLowerCase().replaceAll("[^a-zA-Z]" /* filter all except a-z */ , "");
            if (password.equals("")) //Checks the password to ensure it exists
            {
                printInvalid();
            }
            else
            {
                break;
            }
        }

		var passwordArray = password.toCharArray(); 
        
        for (var x : input.toCharArray()) {
            if(x == ' ')
            {
                output += Character.toString(x);
            } else {

                number = passwordArray[passwordIndex] - 'a'; // sets the number to be the password char minus 'a' to get the correct offset
                if (passwordIndex == passwordArray.length - 1) // loops the password if it's on the last letter
                {
                    passwordIndex = 0;
                }
                else
                {
                    passwordIndex++;
                }

				int y;
				y = x + number; // shifts the letter by the inputted number

                if(y > 'z') // if the shift extends past the alphabet, loops the encoding to ensure only alphabetic characters are used.
                {
                    y = (char)(('a' - 1) + (y - 'z'));
                }
                output += Character.toString(y);
            }
        }
        return output; // returns the shifted string
    }

    public static String BaconEncode(String input, Scanner scanner) // Bacon encode
    {
        var output = "";
        var letterOne = "";
        var letterZero = "";

        while (true) 
        { 
            while(true) // prompt for letter 1
            {
                System.out.println("");
                System.out.println("===============================================================");
                System.out.println("= NOTE: Only 1 character, numbers and whitespace not allowed. =");
                System.out.println("===============================================================");
                System.out.print("Letter 1: ");
                letterZero = scanner.nextLine().trim().replaceAll("[0-9\s]" , ""); 
                if(letterZero.length() != 1)
                {
                    printInvalid();
                } 
                else 
                {
                    break;
                }
            }
            while(true) // prompt for letter 2
            {
                System.out.println("");
                System.out.println("==================================================================================");
                System.out.println("= NOTE: Must be different from the first character. Different cases are allowed. =");
                System.out.println("==================================================================================");
                System.out.print("Letter 2: ");
                letterOne = scanner.nextLine().trim().replaceAll("[\s]" , ""); 
                if(letterOne.length() != 1)
                {
                    printInvalid();
                } 
                else 
                {
                    break;
                }
            }
            if(letterOne.toCharArray()[0] == letterZero.toCharArray()[0]) // data validation to ensure the letters 
            {
                printInvalid();
                letterOne = "";
                letterZero = "";
            }
            else
            {
                break; 
            }
        }

        // for each letter in input
        for (var x : input.toCharArray())
        {
            String z; 
            if (x == ' ') 
            {
                z = "11111";
            }
            else 
            {
                var y = x - 'a'; // converts char to meet requirements of cipher, a = 0, b = 1, etc...
                z = String.format("%5s", Integer.toBinaryString(y)).replace(" ", "0"); // formats the string into 5 char binary. This can hold up to 31 chars, 26 needed
            }
            z = z.replace("0", letterZero); // replace 0 with letterZero
            z = z.replace("1", letterOne); // replace 1 with letterOne 
            z += " "; // append a space at the end of the string. Each character is separated by a space. this is why all spaces will be removed.
            output += z; 
        }
        return output;
    }

    /* === DECODE === */

    public static String CaeserDecode(String input, Scanner scanner) // Decode Caeser
    {
        var output = ""; 
        var number = 0; 

        while(true)
        {
            System.out.println("");
            System.out.print("Shift (1-25): "); // prompts the user for the shift of the message
            if (scanner.hasNextInt()) // ensures the input is a number
            {
                number = scanner.nextInt(); // if input is number, set the number var
                scanner.nextLine();
            }
            else {
                printInvalid();
                scanner.next();
                continue;
            }
            if((number < 1 || number > 25)) // ensures number is between 1 and 25
            {
                printInvalid();
            }
            else break;
        }

        for (var x : input.toCharArray()) {
            if(x == ' ') // spaces don't get shifted
            {
                output += x;
            } else {
				int y; // shifts the letter back to how it should be
				y = x - number; 

                if(y < 'a') // loops the letter if needbe
                {
                    y = (char)(('z' + 1) - ('a' - y));
                }
                output += Character.toString(y);
            }
        }
        return output;
    }

    public static String PasswordDecode(String input, Scanner scanner) // Decode Password
    {
        var passwordIndex = 0; 
        var output = ""; 
        var number = 0; 
        var password = ""; 

        while (true) 
        {
            System.out.println("");
            System.out.println("=========================================================");
            System.out.println("= NOTE: Numbers and special characters will be removed. =");
            System.out.println("=========================================================");
            System.out.print("Password: ");
            password = scanner.nextLine().trim().toLowerCase().replaceAll("[^a-zA-Z]" /* filter all except a-z */ , "");
            if (password.equals("")) //Checks the password to ensure it exists
            {
                printInvalid();
            }
            else
            {
                break;
            }
        }
        
		var passwordArray = password.toCharArray(); 
        
        for (var x : input.toCharArray()) {
            if(x == ' ')
            {
                output += x;
            } else {
				int y; 
                if (!password.equals(""))
				{
					number = passwordArray[passwordIndex] - 'a'; // loops through the password
					if (passwordIndex == passwordArray.length - 1)
					{
						passwordIndex = 0;
					}
					else
					{
						passwordIndex++;
					}
				}
				y = x - number; 

                if(y < 'a')
                {
                    y = (char)(('z' + 1) - ('a' - y));
                }
                output += Character.toString(y);
            }
        }
        return output;
    }

    public static String BaconDecode(String input, Scanner scanner) // Decode Bacon
    {
        var inputArray = input.split(" "); // splits the input at spaces
        var output = ""; 
        var letterOne = ""; 
        var letterZero = "";

        while (true) 
        { 
            while(true) // prompt for letter 1
            {
                System.out.println("");
                System.out.println("===============================================================");
                System.out.println("= NOTE: Only 1 character, numbers and whitespace not allowed. =");
                System.out.println("===============================================================");
                System.out.print("Letter 1: ");
                letterZero = scanner.nextLine().trim().replaceAll("[0-9\s]" , ""); 
                if(letterZero.length() != 1)
                {
                    printInvalid();
                } 
                else 
                {
                    break;
                }
            }
            while(true) // prompt for letter 2
            {
                System.out.println("");
                System.out.println("==================================================================================");
                System.out.println("= NOTE: Must be different from the first character. Different cases are allowed. =");
                System.out.println("==================================================================================");
                System.out.print("Letter 2: ");
                letterOne = scanner.nextLine().trim().replaceAll("[\s]" , ""); 
                if(letterOne.length() != 1)
                {
                    printInvalid();
                } 
                else 
                {
                    break;
                }
            }
            if(letterOne.toCharArray()[0] == letterZero.toCharArray()[0])
            {
                printInvalid();
                letterOne = "";
                letterZero = "";
            }
            else
            {
                break; 
            }
        }

        for (var x : inputArray)
        {
            char y;
            x = x.replace(letterZero, "0"); // converts each input "cluster" into binary
            x = x.replace(letterOne, "1");

            if (x.equals("11111")) {
                y = ' ';
            }
            else
            {
                try 
                {
                    y = (char)Integer.parseInt(x, 2); // attempts to parse the binary into a char agan, if not the given passcode was invalid. 
                } 
                catch (NumberFormatException e) 
                {
                    return "Invalid Passcode";
                }
                y += 'a'; // adds the value of 'a' back to the input char to go back to a readable message
            }
            output += String.valueOf(y);
        }
        return output; 
    }

    public static String AtbashDecodeEncode(String input) // ATBASH cipher works bidirectional
    {
        var output = ""; 
        for (var x : input.toCharArray()) { // swaps A -> Z, B -> Y, etc...
            output += switch(x) 
            {
                case 'a' -> 'z';
                case 'b' -> 'y';
                case 'c' -> 'x';
                case 'd' -> 'w';
                case 'e' -> 'v';
                case 'f' -> 'u';
                case 'g' -> 't';
                case 'h' -> 's';
                case 'i' -> 'r';
                case 'j' -> 'q';
                case 'k' -> 'p';
                case 'l' -> 'o';
                case 'm' -> 'n';
                case 'n' -> 'm';
                case 'o' -> 'l';
                case 'p' -> 'k';
                case 'q' -> 'j';
                case 'r' -> 'i';
                case 's' -> 'h';
                case 't' -> 'g';
                case 'u' -> 'f';
                case 'v' -> 'e';
                case 'w' -> 'd';
                case 'x' -> 'c';
                case 'y' -> 'b';
                case 'z' -> 'a';
                default -> ' ';
            };
        }
        return output; 
    }
}