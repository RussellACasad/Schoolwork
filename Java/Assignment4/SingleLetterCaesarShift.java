import java.util.Scanner;

/**
 * SingleLetterCaesarShift
 */
public class SingleLetterCaesarShift {

    public static void main(String[] args) {
        boolean isRepeating; 
        String input; 
        try (Scanner Scanner = new Scanner(System.in))
        {
            do { 
                do { 
                    System.out.print(">> Enter a character to encrypt: ");
                    input = Scanner.nextLine();
                    if(input.length() == 1 && Character.isLetter(input.charAt(0)))
                    {
                        System.out.println("-" + "-".repeat(32) + "-");
                        System.out.println(String.format("- %-30s -", "Encryption: " + input + " -> " + cypher(input.charAt(0), 1)));
                        System.out.println("-" + "-".repeat(32) + "-\n\n");
                        break;  
                    }
                    else
                    {
                        System.out.println("-" + "-".repeat(32) + "-");
                        System.out.println(String.format("- %-30s -", "Invalid Input"));
                        System.out.println("-" + "-".repeat(32) + "-\n\n");
                    }
                } while (true);
                do { 
                    System.out.print(">> Enter a character to decrypt: ");
                    input = Scanner.nextLine();
                    if(input.length() == 1 && Character.isLetter(input.charAt(0)))
                    {
                        System.out.println("-" + "-".repeat(32) + "-");
                        System.out.println(String.format("- %-30s -", "Decryption: " + input + " -> " + cypher(input.charAt(0), -1)));
                        System.out.println("-" + "-".repeat(32) + "-\n\n");
                        break; 
                    }
                    else
                    {
                        System.out.println("-" + "-".repeat(32) + "-");
                        System.out.println(String.format("- %-30s -", "Invalid Input"));
                        System.out.println("-" + "-".repeat(32) + "-\n\n");
                    }
                } while (true);
                System.out.print(">> Again? [Y/n] ");
                var again = Scanner.nextLine(); 
                isRepeating = "y".equals(again.trim().toLowerCase());
            } while (isRepeating);
        }
    }

    private static char cypher(char letter, int modifier)
    {
        if(letter == 'a' && modifier == -1)
        {
            letter = 'z';
        }
        else if (letter == 'z' && modifier == 1)
        {
            letter = 'a';
        }
        else
        {
            letter += modifier;
        } 
        return letter;
    }
}