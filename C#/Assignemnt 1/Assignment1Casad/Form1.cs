namespace Assignment1Casad
{
    // Russell Casad
    // CPT-230-W01
    // Assignment 01
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            String letterA = " A \r\nA A\r\nAAA\r\nA A\r\nA A"; // assigns each letter a variable that can be printed 3x5
            String letterB = "BBB\r\nB B\r\nBBB\r\nB B\r\nBBB";
            String letterC = "CCC\r\nC  \r\nC  \r\nC  \r\nCCC";
            String letterD = "DD \r\nD D\r\nD D\r\nD D\r\nDD ";
            String letterE = "EEE\r\nE  \r\nEEE\r\nE  \r\nEEE";
            String letterF = "FFF\r\nF  \r\nFF \r\nF  \r\nF  ";
            String letterG = "GGG\r\nG  \r\nG G\r\nG G\r\nGGG";
            String letterH = "H H\r\nH H\r\nHHH\r\nH H\r\nH H";
            String letterI = "III\r\n I \r\n I \r\n I \r\nIII";
            String letterJ = "  J\r\n  J\r\n  J\r\nJ J\r\nJJJ";
            String letterK = "K K\r\nK K\r\nKK \r\nK K\r\nK K";
            String letterL = "L  \r\nL  \r\nL  \r\nL  \r\nLLL";
            String letterM = "M M\r\nMMM\r\nM M\r\nM M\r\nM M";
            String letterN = "NNN\r\nN N\r\nN N\r\nN N\r\nN N";
            String letterO = "OOO\r\nO O\r\nO O\r\nO O\r\nOOO";
            String letterP = "PPP\r\nP P\r\nPPP\r\nP  \r\nP  ";
            String letterQ = "QQQ\r\nQ Q\r\nQQQ\r\n  Q\r\n  Q";
            String letterR = "RRR\r\nR R\r\nRRR\r\nRR \r\nR R";
            String letterS = "SSS\r\nS  \r\nSSS\r\n  S\r\nSSS";
            String letterT = "TTT\r\n T \r\n T \r\n T \r\n T ";
            String letterU = "U U\r\nU U\r\nU U\r\nU U\r\nUUU";
            String letterV = "V V\r\nV V\r\nV V\r\nV V\r\n V ";
            String letterW = "W W\r\nW W\r\nW W\r\nWWW\r\nW W";
            String letterX = "X X\r\nX X\r\n X \r\nX X\r\nX X";
            String letterY = "Y Y\r\nY Y\r\nYYY\r\n Y \r\n Y ";
            String letterZ = "ZZZ\r\n  Z\r\n Z \r\nZ  \r\nZZZ";

            String userInput = txtInput.Text; // accepts a user input into a string variable for easy access
            String outputText = ""; // initialize the output text variable
            foreach (char c in userInput) // iterates through each character inputted, the original plan was to allow more than 2 but I got kinda tired and wanna go bed
            {
                switch (c) // uses a switch statement to see which character to print, then appends it to the outputText variable for printing, alongside 2 new lines to make it pretty
                {
                    case 'A':
                        outputText += letterA + "\r\n\r\n";
                        break;
                    case 'B':
                        outputText += letterB + "\r\n\r\n";
                        break;
                    case 'C':
                        outputText += letterC + "\r\n\r\n";
                        break;
                    case 'D':
                        outputText += letterD + "\r\n\r\n";
                        break;
                    case 'E':
                        outputText += letterE + "\r\n\r\n";
                        break;
                    case 'F':
                        outputText += letterF + "\r\n\r\n";
                        break;
                    case 'G':
                        outputText += letterG + "\r\n\r\n";
                        break;
                    case 'H':
                        outputText += letterH + "\r\n\r\n";
                        break;
                    case 'I':
                        outputText += letterI + "\r\n\r\n";
                        break;
                    case 'J':
                        outputText += letterJ + "\r\n\r\n";
                        break;
                    case 'K':
                        outputText += letterK + "\r\n\r\n";
                        break;
                    case 'L':
                        outputText += letterL + "\r\n\r\n";
                        break;
                    case 'M':
                        outputText += letterM + "\r\n\r\n";
                        break;
                    case 'N':
                        outputText += letterN + "\r\n\r\n";
                        break;
                    case 'O':
                        outputText += letterO + "\r\n\r\n";
                        break;
                    case 'P':
                        outputText += letterP + "\r\n\r\n";
                        break;
                    case 'Q':
                        outputText += letterQ + "\r\n\r\n";
                        break;
                    case 'R':
                        outputText += letterR + "\r\n\r\n";
                        break;
                    case 'S':
                        outputText += letterS + "\r\n\r\n";
                        break;
                    case 'T':
                        outputText += letterT + "\r\n\r\n";
                        break;
                    case 'U':
                        outputText += letterU + "\r\n\r\n";
                        break;
                    case 'V':
                        outputText += letterV + "\r\n\r\n";
                        break;
                    case 'W':
                        outputText += letterW + "\r\n\r\n";
                        break;
                    case 'X':
                        outputText += letterX + "\r\n\r\n";
                        break;
                    case 'Y':
                        outputText += letterY + "\r\n\r\n";
                        break;
                    case 'Z':
                        outputText += letterZ + "\r\n\r\n";
                        break;
                    default:
                        outputText += "Unsupported\r\nCharacter!\r\n\r\n\r\n\r\n\r\n"; // outputs "unsupported Character" for all characters not included, if your first / last name starts with a number or special character, I am sorry
                        break;
                }
            }
            txtOutput.Text = outputText; // outputs the characters
        }
    }
}