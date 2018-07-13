// Title: ACHFileFunctions.cs
// Author: Aiden Nelson
// Date: 7/12/2018
// Description: Functions useful for the parsing of ACH files.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Windows_Forms_ACH_File_Sanitizer
{
    class ACHFileFunctions
    {
        // Random seed
        private static readonly Random rand = new Random();

        // Function: char_randomizer
        // Description: Returns a random lowercase or uppercase character.
        // Parameters: Character being input.
        // Pre-Conditions: None
        // Post-Conditions: Type 1 - Random lowercase, 2 - Random uppercase, 0 - No change.
        // Return: Processed character.
        private static char char_randomizer(char chr)
        {
            if (97 <= (int)chr && (int)chr <= 122)  // Return random lowercase letter.
            {
                return (char)random_return(97, 122);
            }
            else if (65 <= (int)chr && (int)chr <= 90)  // Return random lowercase letter.
            {
                return (char)random_return(65, 90);
            }
            return chr; // Return original character if it's not a letter.
        }


        // Function: random_return
        // Description: Returns a random number within a specified range.
        // Parameters: Lower bound, Upper bound.
        // Pre-Conditions: Parameters are integers.
        // Post-Conditions: Return integer is within range specified.
        // Return: Random number.
        private static int random_return(int lowerLimit, int upperLimit)
        {
            return rand.Next(lowerLimit, upperLimit);
        }

        // Function: sanitize_line
        // Description: Returns a sanitized version of the line passed in.
        // Uses Functions: 
        // Parameters: Line from an ACH file.
        // Pre-Conditions: string
        // Post-Conditions: string
        // Return: Sanitized version of line.
        public static string sanitize_line(string line)
        {
            StringBuilder sanitizedLine = new StringBuilder(line);   // Deal with line as a stringbuilder for convenience.

            if (line[0] == '6')  // Card numbers are only on lines that start with 6.
            {
                // Sanitize number.
                ulong number = UInt64.Parse(line.Substring(12, 16));
                sanitizedLine.Remove(12, 16);
                sanitizedLine.Insert(12, (CardFunctions.get_sanitized_number(number)).ToString());

                // Sanitize name.
                for (int i = 39; i < 78; i++)
                {
                    sanitizedLine[i] = char_randomizer(sanitizedLine[i]);
                }
            }
            else if (line[0] == '5')
            {
                // Sanitize name.
                for (int i = 53; i < 68; i++)
                {
                    sanitizedLine[i] = char_randomizer(sanitizedLine[i]);
                }
            }
            return sanitizedLine.ToString();
        }

        // Function: sanitize_file
        // Description: Creates a sanitized version of the file and places it in the same directory.
        // Uses Functions: sanitize_line
        // Parameters: Filepath of the file being sanitized.
        // Pre-Conditions: Valid file path.
        // Post-Conditions: Valid file path.
        // Return: Path to newly generated sanitized file.
        public static string sanitize_file(string fileName)
        {
            string line = "";

            // Create file object to be read from.
            StreamReader originalFile = new System.IO.StreamReader(@fileName);

            // Create file object to be output to.
            string newFilePath = Path.GetDirectoryName(fileName) + "\\sanitized_file.DAT";
            StreamWriter newFile = new StreamWriter(newFilePath);

            // Loop: Read in line as string, sanitize string, write string to output file.
            while ((line = originalFile.ReadLine()) != null)
            {
                newFile.WriteLine(sanitize_line(line));
            }

            // Close read and write files.
            newFile.Close();
            originalFile.Close();

            return newFilePath;
        }
    }
}
