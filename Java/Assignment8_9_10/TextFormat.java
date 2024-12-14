public class TextFormat { 
    /*
     * Russell Casad
     * CPT-236-W44
     * Last Modified: October 30, 2024
     */
    
    /*
    Each part of this class works the same:
    A string input is taken, and ANSI Escape sequences are utilized to format text for the end user. 

    It returns the following: [ANSI Escape to format text] + [Inputted String] + [ANSI Escape code to end formatting]

    I have tested this in both the MacOS Terminal and Microsoft Powershell using Visual Studio Code, and it works fully.

    Colors can be used alongside other formatting. 
     */ 

    public static void clearScreen()
    {
        System.out.print("\033[H\033[2J");  
        System.out.flush();
    }

    public static String italics(String input)
    {
        return "\033[3m" + input + "\033[0m";
    } 

    public static String bold(String input)
    {
        return "\033[2m" + input + "\033[0m";
    } 

    public static String underline(String input)
    {
        return "\033[4m" + input + "\033[0m";
    } 

    public static String doubleUnderline(String input)
    {
        return "\033[21m" + input + "\033[0m";
    } 

    public static String color(String input, Color foreground, Color background)
    {
        String backgroundColor = switch(background)
        {
            case Color.Black -> "\u001B[40m";
            case Color.White -> "\u001B[47m";
            case Color.Red -> "\u001B[41m";
            case Color.Green -> "\u001B[42m";
            case Color.Yellow -> "\u001B[43m";
            case Color.Blue -> "\u001B[44m";
            case Color.Purple -> "\u001B[45m";
            case Color.Cyan -> "\u001B[46m";
            case Color.None -> "";
        };
        String foregroundColor = switch(foreground)
        {
            case Color.Black -> "\u001B[30m";
            case Color.White -> "\u001B[37m";
            case Color.Red -> "\u001B[31m";
            case Color.Green -> "\u001B[32m";
            case Color.Yellow -> "\u001B[33m";
            case Color.Blue -> "\u001B[34m";
            case Color.Purple -> "\u001B[35m";
            case Color.Cyan -> "\u001B[36m";
            case Color.None -> "";
        };
        return backgroundColor + foregroundColor + input + "\033[39m\033[49m";
    } 

    public static enum Color
    {
        Black, 
        White,
        Red, 
        Green, 
        Yellow, 
        Blue, 
        Purple,
        Cyan,
        None
    }
}
