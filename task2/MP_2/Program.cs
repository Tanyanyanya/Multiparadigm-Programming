using System.IO;

namespace MP_2
{
    class Program
    {
        /**
   * @param inPath path to the file from which the data will be read
   * @param outPath path to the file where the result will be written
   * @param lines the number of lines that will separate pages
   * @param words words in a file
   * @param pages array of pages separated by 45 @lines
   * @param amount word counter
   * @param length word length
   * @param this_page current page
   * @param word sentence line
   */
        static void Main(string[] args)
        {
            string inPath = "input.txt";
            string outPath = "output.txt";
            int lines = 45;
            int length = 0, i, string_ = 0;
            int this_page = 1;
            string word = "";
            string[] words = new string[0];
            int[] amount = new int[0];
            int[][] pages = new int[0][];

            //reading a file from a path
            StreamReader reader = new StreamReader(inPath);
        read:
            {
                if (reader.EndOfStream)
                    goto end;

                //split pages
                string str = reader.ReadLine();
                if (string_ == lines)
                {
                    this_page++;
                    string_ = 0;
                }
                string_++;

                int j = 0;
            loop:
                {
                    if (j == str.Length)
                        goto endLoop;
                    //normalizing the use of capital letters
                    char symbol = str[j];
                    if (symbol >= 'A' &&  'Z' >= symbol )
                    {
                        word += ((char)(symbol + 32));
                        if (j + 1 < str.Length)
                            goto endLoop;
                    }
                    else if (symbol >= 'a' && 'z' >= symbol)
                    {
                        word += symbol;
                        if (j + 1 < str.Length)
                            goto endLoop;
                    }

                    if (word != "" && symbol != '-' && symbol != '\'' && word != "the" && word != "for" && word != "of" && word != "an" && word != "on")
                    {
                        i = 0;
                    newWords: //check if it's new word
                        {
                            if (i == length)
                                goto uniqueWords;
                            if (word == words[i]) //the word occurs not for the first time
                            {
                                word = "";
                                if (amount[i] > 100) //the word is ignored if it occurs more than 100 times in the text
                                {
                                    goto endLoop;
                                }
                                amount[i]++;
                                if (amount[i] <= pages[i].Length) //move to another page?
                                {
                                    pages[i][amount[i] - 1] = this_page;
                                }
                                else
                                {
                                    //increase the number of pages
                                    int[] pagesTmp = new int[amount[i] * 2];
                                    int p = 0;
                                copy: //save past pages
                                    {
                                        pagesTmp[p] = pages[i][p];
                                        p++;
                                        if (p < amount[i] - 1)
                                            goto copy;
                                    }
                                    pages[i] = pagesTmp;
                                    pages[i][amount[i] - 1] = this_page;
                                }
                                goto endLoop;
                            }
                            i++;
                            goto newWords;
                        }

                    uniqueWords: //a new word appears
                        if (length == words.Length)
                        {
                            string[] newWords = new string[(length + 1) * 2];
                            int[] newCounts = new int[(length + 1) * 2];
                            int[][] newPages = new int[(length + 1) * 2][];

                            i = 0;
                        copyLoop:
                            {
                                if (i == length)
                                {
                                    words = newWords;
                                    amount = newCounts;
                                    pages = newPages;
                                    goto endCopyLoop;
                                }
                                newWords[i] = words[i];
                                newCounts[i] = amount[i];
                                newPages[i] = pages[i];
                                i++;
                                goto copyLoop;
                            }
                        }

                    endCopyLoop:
                        words[length] = word;
                        amount[length] = 1;
                        pages[length] = new int[] { this_page };
                        length++;
                        word = "";
                    }

                endLoop:
                    j++;
                    if (j < str.Length)
                        goto loop;
                }
                if (!reader.EndOfStream)
                    goto read;
            }

        end:
            reader.Close();
            //sorting
            int l;
            int k;
            int[] page;
            i = 1;
        sort:
            {
                l = amount[i];
                word = words[i];
                page = pages[i];
                k = i - 1;
            whileSort:
                {
                    if (k >= 0)
                    {
                        int symb = 0;
                    compare: //compare words
                        {
                            if (symb == words[k].Length || words[k][symb] < word[symb])
                                goto endWhile;

                            if (symb + 1 < word.Length && words[k][symb] == word[symb])
                            {
                                symb++;
                                goto compare;
                            }
                        }

                        amount[k + 1] = amount[k];
                        words[k + 1] = words[k];
                        pages[k + 1] = pages[k];
                        k--;
                        goto whileSort;
                    }
                }
            endWhile:
                amount[k + 1] = l;
                words[k + 1] = word;
                pages[k + 1] = page;
                i++;
                if (i < length)
                    goto sort;
            }

            //output
            StreamWriter writer = new StreamWriter(outPath);
            i = 0;
        write:
            {
                if (amount[i] <= 100)
                { //if there are less than 100 words
                    writer.Write(words[i] + " - " + pages[i][0]);
                    int j = 1;
                pagesLoop:
                    {
                        if (j == amount[i])
                            goto endPagesLoop;
                        if (pages[i][j] != pages[i][j - 1]) // split pages
                            writer.Write(", " + pages[i][j]);
                        j++;
                        goto pagesLoop;
                    }
                endPagesLoop:
                    writer.WriteLine();
                }
                i++;
                if (i < length)
                    goto write;
            }

            writer.Close();
        
    }
    }
}
