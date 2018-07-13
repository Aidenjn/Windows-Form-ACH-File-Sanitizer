// Title: CardFunctions.cs
// Author: Aiden Nelson
// Date: 7/12/2018
// Description: Handy functions for handling credit card numbers.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows_Forms_ACH_File_Sanitizer
{
    class CardFunctions
    {
        // Function: get_last_digit
        // Description: Returns the last digit of a given number.
        public static int get_last_digit(ulong num)
        {
            return Convert.ToInt32(num % 10);    // Get the last digit of the card number.
        }

        // Function: get_sanitized_number
        // Description: Returns sanitized version of card number.
        // Uses Functions: get_corrected_number
        // Parameters: Number to be sanitized.
        // Pre-Conditions: Valid card number.
        // Post-Conditions: Number retains first 5 digits then is all zeros, except for the check digit at the end that will pass the Luhn Algorithm.
        // Return: Sanitized number.
        public static ulong get_sanitized_number(ulong num)
        {
            ulong returnNum = 0;
            int checkDigit = CardFunctions.get_last_digit(num);
            int[] digits = num.ToString().Select(Convert.ToInt32).ToArray();    // Convert num into array of digits.

            for (int i = 0; i < 5; i++) // Retain first 5 digits of the number, and zero the rest.
            {
                digits[i] -= 48;
                returnNum += Convert.ToUInt64(digits[i]) * Convert.ToUInt64(Math.Pow(10, digits.Length - i - 1));
            }

            return get_corrected_number(returnNum); // Return number with the correct check digit at the end.
        }

        // Function: luhn_check
        // Description: Checks to see if the given number passes Luhn's Algorithm.
        // Uses Functions: get_last_digit, get_correct_digit
        // Parameters: Number being tested.
        // Pre-Conditions: Positive integer input.
        // Post-Conditions: Bool output.
        // Return: false - passes algorithm, true - does not pass algorithm.
        public static bool luhn_check(ulong num)
        {
            int checkDigit = get_last_digit(num);
            
            if (get_correct_check_digit(num) != checkDigit)
                return true;
            else
                return false;
        }

        // Function: get_corrected_number
        // Description: Returns the number with the proper check digit that allows it to pass Luhn's Algorithm.
        // Uses Functions: get_last_digit, get_correct_check_digit
        // Parameters: Number being corrected.
        // Pre-Conditions: Positive integer input.
        // Post-Conditions: Number passes Luhn's Algorithm.
        // Return: Version of number that passes Luhn's Algorithm.
        public static ulong get_corrected_number(ulong num)
        {
            int checkDigit = get_last_digit(num);
            return num - Convert.ToUInt32(checkDigit) + Convert.ToUInt32(get_correct_check_digit(num)); // Replace the last digit with the correct checkDigit.
        }

        // Function: get_correct_check_digit
        // Description: Returns what the number's check digit should be.
        // Uses Functions: get_last_digit
        // Parameters: Number being corrected.
        // Pre-Conditions: Positive integer input.
        // Post-Conditions: Number passes Luhn's algorithm.
        // Return: 
        public static int get_correct_check_digit(ulong num)
        {
            int sum = 0;
            int[] digits = num.ToString().Select(Convert.ToInt32).ToArray();    // Convert num into array of digits.
            int everyOtherNumber = 0;   // For tracking every other number.

            for (int i = digits.Length - 2; i >= 0; i--)    // Run through digits in reverse order, skipping the ending digit.
            {
                digits[i] -= 48;    // Convert char value to digit value.
                if (everyOtherNumber % 2 == 0)
                {
                    digits[i] *= 2;
                    if (digits[i] > 9)
                        digits[i] -= 9;
                }
                sum += digits[i];
                everyOtherNumber++;
            }

            return (sum * 9) % 10; // Return the calculated checkDigit.
        }

        // Function: is_corrrect_length
        // Description: Checks to see if the card number is the correct length.
        // Parameters: Number being tested.
        // Pre-Conditions: Positive integer input.
        // Post-Conditions: Output must be 0 or 1.
        // Return: 0 - Number is correct length, 1 - does not pass algorithm.
        public static bool is_correct_length(ulong num)
        {
            string numStr = num.ToString();

            if ((numStr[0] == '3') && (numStr.Length == 15))
                return true;
            else if (numStr.Length == 16)
                return true;
            else
                return false;
        }
    }
}
