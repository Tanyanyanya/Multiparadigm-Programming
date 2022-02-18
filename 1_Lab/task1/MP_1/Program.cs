
using System;
using System.IO;

namespace MP_1
{

    /**
 * The {@code TermFrequency} display N  
 * of the most common words and the corresponding frequency of their repetition, in descending order.
 * Ignored stop words and normalized capitalization
 */
    class TermFrequency
    {
        /**
    * @param inPath path to the file from which the data will be read
    * @param outPath path to the file where the result will be written
    * @param word words in a file
    * @param amount word counter
    * @param length word length
    * @param wordAmount the number of words we want to display, entered through the console, 
    *                   in this case it is ignored and displays all the words
    * @param words sentence line
    */
        static void Main(string[] args)
        {
            string inPath = "input.txt"; 
            string outPath = "output.txt";
            string[] word = new string[0];
            int[] amount = new int[0];
            int length = 0, i;
            int wordAmount;
            wordAmount = int.Parse(Console.ReadLine());
            string words = "";

            //reading a file from a path
            StreamReader reader = new StreamReader(inPath);
        term_frequency:
            {
                if (reader.EndOfStream)
                    goto endReading;
                //normalizing the use of capital letters
                char symbol = (char)reader.Read();

                if ('Z' >= symbol && symbol >= 'A')
                {
                    words += ((char)(symbol + 32)).ToString();
                    if (!reader.EndOfStream)
                        goto term_frequency;
                }
                else if ('z' >= symbol && symbol >= 'a')
                {
                    words += symbol;
                    if (!reader.EndOfStream)
                        goto term_frequency;
                }

                //ignore stop words
                if (words != "" && symbol != '-' && symbol != '\'' && words != "the" && words != "for" && words != "of" && words != "an" && words != "on")
                {
                    i = 0;

                newWords: //check if it`s a new word
                    {
                        if (i == length)
                            goto uniqueWords;
                        if (words == word[i])
                        {
                            amount[i]++;
                            words = "";
                            if (reader.EndOfStream)
                                goto endReading;
                            goto term_frequency;
                        }
                        i++;
                        goto newWords;
                    }

                uniqueWords: //it`s new word
                    if (length == word.Length)
                    {

                        string[] newWord = new string[(length + 1) * 2];
                        int[] newAmount = new int[(length + 1) * 2];

                        i = 0;
                    copy:
                        {
                            if (i == length)
                            {
                                word = newWord;
                                amount = newAmount;
                                goto end;
                            }
                            newWord[i] = word[i];
                            newAmount[i] = amount[i];
                            i++;
                            goto copy;
                        }

                    }

                end:
                    word[length] = words;
                    amount[length] = 1;
                    words = "";
                    length++;
                }

                if (!reader.EndOfStream)
                    goto term_frequency;
            }

        endReading:
            reader.Close();
            int curr, j;
            i = 1;

        //sorting
        sort:
            {
                curr = amount[i];
                words = word[i];
                j = i - 1;
            sortLoop:
                {
                    if (j >= 0 && amount[j] < curr)
                    {
                        amount[j + 1] = amount[j];
                        word[j + 1] = word[j];
                        j--;
                        goto sortLoop;
                    }
                }

                amount[j + 1] = curr;
                word[j + 1] = words;
                i++;
                if (i < length)
                    goto sort;
            }

            //writing the result to a file
            StreamWriter writer = new StreamWriter(outPath);
            i = 0;
        write:
            {
                writer.WriteLine(word[i] + " - " + amount[i]);
                i++;
                if (i < length)
                    goto write;
            }

            writer.Close();
        }
    }
}
