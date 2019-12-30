using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class BankMessageToNumbers
    {

        private string[] BankMessageArray;
        private string[] BankMessageDateArray;

        //transaction amount and balance
        private double[] amount;
        private double[] balance;
        private double[] date;
        private double accountBalance = 0;

        private int cofTrues = 0;
        private int cofNoes = 0;


        public BankMessageToNumbers(string[] BankMessageArray, string[] BankMessageDateArray)
        {
           
            // TODO Auto-generated constructor stub
            this.BankMessageArray = BankMessageArray;
            this.BankMessageDateArray = BankMessageDateArray;

            checkRelevantData();
            initiateTransactArrays();
            saveTransactKeyValues();

            

        }

        //No of relevant bank transactions
        public virtual int NoOfTrues()
        {


            return cofTrues;

        }

        //No of irrelevant bank transactions
        public virtual int NoOfNoes()
        {


            return cofNoes;

        }

        public virtual double[] TransactionAmounts
        {
            get
            {
                double[] newAmount = new double[amount.Length];
                double count = 0.0;
                int counter = 0;

                foreach (double amt in amount)
                {

                    count = count + amt;
                    newAmount[counter] = count;
                    counter++;

                }

                return newAmount;

            }
        }


        public virtual double[] TransactionDates
        {
            get
            {
                return date;
            }
        }

        public virtual double[] BalanceAmounts
        {
            get
            {
                return balance;
            }
        }

        public virtual double TotalBalance
        {
            get
            {
                checkAccountBalance();
                return accountBalance;
            }
        }


        public void checkRelevantData()
        {
            bool[] relevant = new bool[BankMessageArray.Length];
            int count = 0;

            foreach (string s in BankMessageArray)
            {
                relevant[count] = checkBankTransact(s);
                count++;
            }

            foreach (bool r in relevant)
            {
                //check no of trues and no of false in the bool array
                //no of trues will be used to create an array to contain relevant data

                if (r)
                {
                    cofTrues++;
                }
                else
                {
                    cofNoes++;
                }
            }

            //date array  for relevant dates
            date = new double[cofTrues];
            int cont = 0;

            for (int x = 0; x < relevant.Length; x++)
            {

                if (relevant[x])
                {
                    date[cont] = Convert.ToDouble(BankMessageDateArray[x]);
                    cont++;
                }
            }

        }
        //checkRelevantData method must be called before this
        internal void initiateTransactArrays()
        {
            // amount = new double[];
            //balance = new double[cofTrues];
        }

        //store the relevant data in the amount and balance array
        public List<Transaction> saveTransactKeyValues()
        {
            var Transactions = new List<Transaction>();

            for (int i = 0; i < BankMessageArray.Length; i++)
            {
               
              var result =  BankTransact(BankMessageArray[i], i);
                Transactions.Add(new Transaction
                {
                    amounts = result.amounts,
                    balance = result.balance,
                    transactionDate = result.transactionDate,
                    transactionType = result.transactionType,
                    descrption = result.descrption

                });
                
            }
            return Transactions;
           
        }

        //check for relevant bank messages in each string inputed 	
        public Transaction BankTransact(string bankData, int BankMessageCount)
        {
            var transactionDetails = new Transaction();
            string newBankData = "";

            //change all letters to lower case 
            for (int i = 0; i < bankData.Length; i++)
            {
                char ch = bankData[i];
                if (char.IsLetter(ch) && char.IsUpper(ch))
                {
                    ch = char.ToLower(ch);
                }
                newBankData += ch;
            }

            Console.WriteLine(newBankData);

            //		check for relevant data in the message using the keyword
            //		search for amt, ngn or n(with a number beside), desc, debit or dr
            //		credit or cr, avail bal or bal
            //		if debit or dr or credit or cr and bal 
            //		and amt or time and acct or ac and desc is true then its bank sms
            //		else then its not a bank message
            string[] smsParam = new string[] { "ac", "cr", "dr", "by", "amt", "bal", "acct", "desc", "debit", "credit" };
            bool[] smsStatus = new bool[smsParam.Length];

            //loop through the sms param containing alll the strings we are searching for in the bank messgage 
            TheLoop:
            for (int i = 0; i < smsParam.Length; i++)
            {

                //loop through the lowercased bank meesage
                bigloop:
                for (int j = 0; j < newBankData.Length; j++)
                {
                    string compString = "";

                    //get the first character in the message
                    char ch1 = newBankData[j];

                    //change to a string
                    string st = "" + ch1;

                    //change the first character of the string in position i in the list to string eg 'amt' will result to 'a' as a string 
                    string letter = Convert.ToString(smsParam[i][0]);
                    //compare the first character to the searcher's first character
                    //if it matches then check all the characters\
                    //else move to the next word in the message


                    if (char.IsLetter(ch1) && st.Equals(letter))
                    {

                        //count/j is the position in the bigloop in which the search string matches a word in the bank message 
                        int count = j;

                        //loop through the search string exclusively since the first letters of the search string and the word in the message matches
                        for (int k = 0; k < smsParam[i].Length; k++)
                        {

                            char ch2 = 'a';

                            //protect the count index from becoming greater than the length of the bank message eg if the length of the bank message is 30 and count increments to 31 it will crash
                            // this crash signifies that we have reached the end of the bank message and there is no other string to be found so break of the big loop to move to the next search string in the smsparam list
                            try
                            {
                                //asssign the character of the message in position count
                                ch2 = newBankData[count];

                            }
                            catch (IndexOutOfRangeException)
                            {
                                //move to the next search string in the list
                                break;
                            }

                            //convert each character to string
                            string st1 = "" + ch2;
                            string let = Convert.ToString(smsParam[i][k]);

                            //compare the converted strings
                            if (char.IsLetter(ch2) && st1.Equals(let))
                            {
                                Console.WriteLine(let);
                                Console.WriteLine(st1);

                                //keep building the values of the matching string
                                //add the new matched string to the existing string eg am + t to make amt
                                compString = compString + let;

                                //increment the count for the next letter in the bank message
                                count = count + 1;

                                //keep checking if the builtup string matches the search string
                                //till it matches
                                //eg amt of compSt.. equals amt of the search string in the list
                                if (compString.Equals(smsParam[i]))
                                {
                                    Console.WriteLine("enter here");
                                    smsStatus[i] = true; //means that the search string(smsparam) at index i of the loop was found
                                    compString = ""; //empty the string
                                    break;
                                }
                            }
                            else
                            {
                                //the String does not match
                                //move to next search string
                                //System.out.println(let);
                                //System.out.println(st1);
                                Console.WriteLine("in here");
                                //empty compString
                                compString = "";
                                break; //skip building the string due to mismatch
                                       //and moves to the next letter/character on the text message
                            }

                        }

                    }
                }
            }

            foreach (bool s in smsStatus)
            {
                Console.WriteLine(s);
            }

            //next use the boolean data returned to know if the string is a bankSms
            //{"ac","cr","dr","amt","bal","acct","desc","by","debit","credit"};
            //the message to be a bank alert the following parameters must be true
            //(ac or acct = true) and (cr or credit = true) or (dr or debit = true)
            //desc = true && by = true("by" indicates description for first Bank)  
            //{"ac","cr","dr","by","amt","bal","acct","desc","debit","credit"};

            bool accountNo, desc, creditAlert, debitAlert;

            accountNo = (smsStatus[0] == true || smsStatus[6] == true);
            creditAlert = (smsStatus[1] == true || smsStatus[9] == true);
            debitAlert = (smsStatus[2] == true || smsStatus[8] == true);
            desc = (smsStatus[3] == true || smsStatus[7] == true);


            //for the message to be alert it must have accountNo as true and (credit or debit) true  
            if (accountNo && smsStatus[8] == true)
            {
                debitAlert = true;
                creditAlert = false;
            }
            else if (accountNo && smsStatus[9] == true)
            {
                creditAlert = true;
                debitAlert = false;
            }

            //Next extract relevant data if the message is a bank message
            //data like amount withrawn, available balance and description
            if (accountNo && creditAlert && !debitAlert && desc)
            {
                //System.out.println("Credit BankAlert!!!");
                relevantData(newBankData, smsStatus, BankMessageCount);
            }
            else if (accountNo && !creditAlert && debitAlert && desc)
            {
                //System.out.println("Debit BankAlert!!!");
                relevantData(newBankData, smsStatus, BankMessageCount);

            }
            else
            {
                return null;
                //System.out.println("Not BankAlert!!!");
            }

            return transactionDetails;
        }

        public void relevantData(string bankMsg, bool[] smsStatus, int BankMessageCount)
        {
            //Relevant Data (Description,Amount and Balance)
            //if you find string amt or bal 
            //then start looking for numbers if its not a number discard 
            //till stop seeing numbers
            //for desc, find keyword desc and pick every text until you reach a space
            //or the next search keyword

            //Keywords to search for

            string[] keyWords = new string[2];
            double[] bankVal = new double[2];

            if (smsStatus[3] == true && smsStatus[7] == false)
            {
                //if true it means the bank selected is first bank due to absense of balance
                keyWords[0] = "ngn";
                keyWords[1] = "acct";
            }
            else
            {
                //for other banks
                keyWords[0] = "amt";
                keyWords[1] = "bal";
            }

            for (int i = 0; i < keyWords.Length; i++)
            {
                for (int j = 0; j < bankMsg.Length; j++)
                {
                    string compString = "";
                    char ch1 = bankMsg[j];
                    string st = "" + ch1;
                    string letter = Convert.ToString(keyWords[i][0]);
                    //compare character to the searcher's first character
                    if (char.IsLetter(ch1) && st.Equals(letter))
                    {
                        int count = j;
                        for (int k = 0; k < keyWords[i].Length; k++)
                        {
                            char ch2 = bankMsg[count];
                            string st1 = "" + ch2;
                            string let = Convert.ToString(keyWords[i][k]);

                            if (char.IsLetter(ch2) && st1.Equals(let))
                            {
                                Console.WriteLine(let);
                                Console.WriteLine(st1);
                                compString = compString + let;
                                count = count + 1;
                                if (compString.Equals(keyWords[i]))
                                {
                                    //Start looking for the Numbers
                                    Console.WriteLine("enter here");
                                    Console.WriteLine(count);
                                    string Number = "";
                                    char lastChar = ' ';
                                    string fStop = ".";
                                    for (int p = count; p < bankMsg.Length; p++)
                                    {
                                        char Num = bankMsg[p];
                                        string NewChar = "" + Num;
                                        try
                                        {
                                            int newNum = int.Parse(NewChar);
                                            Number = Number + Convert.ToString(newNum);
                                            Console.WriteLine(Number);
                                        }
                                        catch (Exception)
                                        {
                                            Console.WriteLine(Num);
                                            //if the next string in the amount is a full stop then append the fullstop to the number
                                            if (NewChar.Equals(fStop, StringComparison.OrdinalIgnoreCase) && char.IsDigit(lastChar) && char.IsDigit(bankMsg[p + 1]))
                                            {
                                                Number += fStop;
                                            }
                                        }

                                        try
                                        {
                                            //if the last character is a digit and the present character is not a digit and pt1 is not a digit and number.length is greater than zero
                                            if (!char.IsDigit(lastChar) && !char.IsDigit(Num) && !char.IsDigit(bankMsg[p + 1]) && Number.Length > 0)
                                            {
                                                try
                                                {
                                                    bankVal[i] = double.Parse(Number);
                                                    Console.WriteLine("Parsed Number is " + Number + " at the " + i + "th value");



                                                }

                                                catch (Exception e)
                                                {
                                                    Console.WriteLine(e.ToString());
                                                    Console.Write(e.StackTrace);
                                                    bankVal[i] = 0.0;
                                                    Console.WriteLine("Double parse error");
                                                }
                                                Number = "";
                                                break;
                                            }
                                        }

                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.ToString());
                                            Console.Write(e.StackTrace);
                                        }

                                        lastChar = Num;
                                        if (p == (bankMsg.Length - 1))
                                        {
                                            try
                                            {
                                                bankVal[i] = double.Parse(Number);
                                                Console.WriteLine("Parsed Number is " + Number + " at the " + i + "th value");


                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine(e.ToString());
                                                Console.Write(e.StackTrace);
                                                bankVal[i] = 0.0;
                                                Console.WriteLine("Double parse error");
                                            }
                                            Number = "";
                                            break;
                                        }
                                    }

                                }
                            }
                            else
                            {
                                //System.out.println(let);
                                //System.out.println(st1);
                                Console.WriteLine("in here");
                                compString = "";
                            }

                        }
                    }

                }


            }

        }
        //check for relevant bank messages in eaach string inputed 	
        public virtual bool checkBankTransact(string bankData)
        {
            //String[] bankinfo = new String[10];

            string newBankData = "";

            //change all letters to lower case 
            for (int i = 0; i < bankData.Length; i++)
            {
                char ch = bankData[i];
                if (char.IsLetter(ch) && char.IsUpper(ch))
                {
                    ch = char.ToLower(ch);
                }
                newBankData += ch;
            }

            Console.WriteLine(newBankData);

            //check for relevant data in the message using the keyword
            //search for amt, ngn or n(with a number beside), desc, debit or dr
            //credit or cr, avail bal or bal
            //if debit or dr or credit or cr and bal 
            //and amt or time and acct or ac and desc is true then its bank sms
            //else then its not a bank message
            string[] smsParam = new string[] { "ac", "cr", "dr", "by", "amt", "bal", "acct", "desc", "debit", "credit" };
            bool[] smsStatus = new bool[smsParam.Length];

            //loop through the sms param containing alll the strings we are searching for in the bank messgage 
            TheLoop:
            for (int i = 0; i < smsParam.Length; i++)
            {

                //loop through the lowercased bank meesage
                bigloop:
                for (int j = 0; j < newBankData.Length; j++)
                {
                    string compString = "";

                    //get the first character in the message
                    char ch1 = newBankData[j];

                    //change to a string
                    string st = "" + ch1;

                    //change the first character of the string in position i in the list to string eg 'amt' will result to 'a' as a string 
                    string letter = Convert.ToString(smsParam[i][0]);
                    //compare the first character to the searcher's first character
                    //if it matches then check all the characters\
                    //else move to the next word in the message


                    if (char.IsLetter(ch1) && st.Equals(letter))
                    {

                        //count/j is the position in the bigloop in which the search string matches a word in the bank message 
                        int count = j;

                        //loop through the search string exclusively since the first letters of the search string and the word in the message matches
                        for (int k = 0; k < smsParam[i].Length; k++)
                        {

                            char ch2 = 'a';

                            //protect the count index from becoming greater than the length of the bank message eg if the length of the bank message is 30 and count increments to 31 it will crash
                            // this crash signifies that we have reached the end of the bank message and there is no other string to be found so break of the big loop to move to the next search string in the smsparam list
                            try
                            {
                                //asssign the character of the message in position count
                                ch2 = newBankData[count];

                            }
                            catch (IndexOutOfRangeException)
                            {
                                //move to the next search string in the list
                                break;
                            }

                            //convert each character to string
                            string st1 = "" + ch2;
                            string let = Convert.ToString(smsParam[i][k]);

                            //compare the converted strings
                            if (char.IsLetter(ch2) && st1.Equals(let))
                            {
                                Console.WriteLine(let);
                                Console.WriteLine(st1);

                                //keep building the values of the matching string
                                //add the new matched string to the existing string eg am + t to make amt
                                compString = compString + let;

                                //increment the count for the next letter in the bank message
                                count = count + 1;

                                //keep checking if the builtup string matches the search string
                                //till it matches
                                //eg amt of compSt.. equals amt of the search string in the list
                                if (compString.Equals(smsParam[i]))
                                {
                                    Console.WriteLine("enter here");
                                    smsStatus[i] = true; //means that the search string(smsparam) at index i of the loop was found
                                    compString = ""; //empty the string
                                    break;
                                }
                            }
                            else
                            {
                                //the String does not match
                                //move to next search string
                                //System.out.println(let);
                                //System.out.println(st1);
                                Console.WriteLine("in here");
                                //empty compString
                                compString = "";
                                break; //skip building the string due to mismatch
                                       //and moves to the next letter/character on the text message
                            }

                        }

                    }
                }
            }

            foreach (bool s in smsStatus)
            {
                Console.WriteLine(s);
            }
            //next use the boolean data returned to know if the string is a bankSms
            //{"ac","cr","dr","amt","bal","acct","desc","by","debit","credit"};
            //the message to be a bank alert the following parameters must be true
            //(ac or acct = true) and (cr or credit = true) or (dr or debit = true)
            //desc = true && by = true("by" indicates description for first Bank)  
            //{"ac","cr","dr","by","amt","bal","acct","desc","debit","credit"};

            bool accountNo, desc, creditAlert, debitAlert;

            accountNo = (smsStatus[0] == true || smsStatus[6] == true);
            creditAlert = (smsStatus[1] == true || smsStatus[9] == true);
            debitAlert = (smsStatus[2] == true || smsStatus[8] == true);
            desc = (smsStatus[3] == true || smsStatus[7] == true);

            //for the message to be alert it must have accountNo as true and (credit or debit) true  
            if (accountNo && smsStatus[8] == true)
            {
                debitAlert = true;
                creditAlert = false;
            }
            else if (accountNo && smsStatus[9] == true)
            {
                creditAlert = true;
                debitAlert = false;
            }

            //Next extract relevant data if the message is a bank message
            //data like amount withrawn, available balance and description
            if (accountNo && creditAlert && !debitAlert && desc)
            {
                Console.WriteLine("Credit BankAlert!!!");
                //relevantData(newBankData,smsStatus);
                return true;
            }
            else if (accountNo && !creditAlert && debitAlert && desc)
            {
                Console.WriteLine("Debit BankAlert!!!");
                //relevantData(newBankData,smsStatus);
                return true;
            }
            else
            {
                return false;
                //System.out.println("Not BankAlert!!!");
            }

        }

        public void checkAccountBalance()
        {
            //returns the calculated balance from the array
            //Through the array and sums all the transaction amounts
            for (int i = 0; i < balance.Length; i++)
            {
                accountBalance = accountBalance + balance[i];
            }
        }

        public void checkMostReoccured()
        {
            //create a new array to store the no of times a number reoccured
            //get the whole array and loop through the numbers
            //for each number search through array and count how many matches found
            //if found store the no of repetition
            //loop through the no of repitions
            //for each of repetitions look for other matching repetitions
            //if their amount matches set the new found amount to be (0.0) and 
            //its equivalent no of repetitions as (-1)

            int[] NofOccur = new int[amount.Length];
            int count = 0;

            for (int i = 0; i < amount.Length; i++)
            {
                for (int j = 0; j < amount.Length; j++)
                {

                    if (amount[i] == amount[j])
                    {
                        count++;
                    }


                    if (j == (amount.Length - 1))
                    {
                        NofOccur[i] = count;
                        count = 0;
                    }

                }

            }

            for (int k = 0; k < NofOccur.Length; k++)
            {
                int cont = 0;
                for (int l = 0; l < NofOccur.Length; l++)
                {
                    if (NofOccur[l] == -1 && amount[l] == 0.0)
                    {
                        break;
                    }
                    if (NofOccur[k] == NofOccur[l] && amount[k] == amount[l])
                    {
                        if (cont != 0)
                        {
                            NofOccur[l] = -1;
                            amount[l] = 0.0;
                        }
                        cont++;
                    }
                }
            }

        }
    }
}
