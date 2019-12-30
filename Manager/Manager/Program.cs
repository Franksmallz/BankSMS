using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    class program
    {
        static void Main(string[] args)
        {
            string[] textMsg = new string[] { "access)))Credit Alert Amt: NGN 2000.00 Cr Acc: 073****630 Desc: MBAH DEREK ONYEDIKACHI/Online Time: 30/08/17 a02:33 PM Bal: NGN 2,000.00 #SaveToday",
                                               "Recharge your phone line with 1-Click airtime top up from GTBank. Simply dial *737*Amount # now. For N1000, dial *737*1000#",
                                                "access>>>Debit Alert Amt: NGN 50.00 Dr Acc: 073****630 Desc: MONTHLY CARD FEE ON:0020120685 Time: 30/08/17 a02:33 PM Bal: NGN 3,000.52 #SaveToday",
                                                "Recharge your phone line with 1-Click airtime top up from GTBank. Simply dial *737*Amount# now. For N1000, dial *737*1000#",
                                                "access)))Credit Alert Amt: NGN 2000.00 Cr Acc: 073****630 Desc: MBAH DEREK ONYEDIKACHI/Online Time: 30/08/17 a02:33 PM Bal: NGN 2,000.00 #SaveToday",
                                                "Recharge your phone line with 1-Click airtime top up from GTBank. Simply dial *737*Amount# now. For N1000, dial *737*1000#",
                                                "access>>>Debit Alert Amt: NGN 50.00 Dr Acc: 073****630 Desc: MONTHLY CARD FEE ON:0020120685 Time: 30/08/17 a02:33 PM Bal: NGN 3,000.52 #SaveToday",
                                                "Recharge your phone line with 1-Click airtime top up from GTBank. Simply dial *737*Amount# now. For N1000, dial *737*1000#",
                                                "access>>>Debit Alert Amt: NGN 50.00 Dr Acc: 073****630 Desc: MONTHLY CARD FEE ON:0020120685 Time: 30/08/17 a02:33 PM Bal: NGN 3,000.52 #SaveToday",
                                                "Recharge your phone line with 1-Click airtime top up from GTBank. Simply dial *737*Amount# now. For N1000, dial *737*1000#",
                                                "Recharge your phone line with 1-Click airtime top up from GTBank. Simply dial *737*Amount# now. For N1000, dial *737*1000#",
                                                "Recharge your phone line with 1-Click airtime top up from GTBank. Simply dial *737*Amount# now. For N1000, dial *737*1000#",
                                                "access>>>Debit Alert Amt: NGN 50.00 Dr Acc: 073****630 Desc: MONTHLY CARD FEE ON:0020120685 Time: 30/08/17 a02:33 PM Bal: NGN 3,000.52 #SaveToday",
                                                "Recharge your phone line with 1-Click airtime top up from GTBank. Simply dial *737*Amount# now. For N1000, dial *737*1000#",
                                                "access>>>Debit Alert Amt: NGN 50.00 Dr Acc: 073****630 Desc: MONTHLY CARD FEE ON:0020120685 Time: 30/08/17 a02:33 PM Bal: NGN 3,000.52 #SaveToday" };

            string[] msgDates = new string[] { "3434.34", "3434.34", "3434.34", "3434.34", "3434.34", "3434.34", "3434.34", "3434.34", "3434.34", "3434.34", "3434.34", "3434.34", "3434.34", "3434.34", "3434.34" };

            BankMessageToNumbers bm = new BankMessageToNumbers(textMsg, msgDates);

            var NoOfTrues = bm.NoOfTrues();
            var NoOfNos = bm.NoOfNoes();
            Console.WriteLine(NoOfTrues);
            Console.WriteLine(NoOfNos);

            var trAmounts = bm.TransactionAmounts;
            Console.WriteLine(trAmounts);
            var trDates = bm.TransactionDates;
            var trBalances = bm.BalanceAmounts;
            Console.WriteLine(trBalances);

            if (trAmounts.Length == trDates.Length)
            {

                foreach (double s in trAmounts)
                {
                    Console.WriteLine(s);
                }



                foreach (double s in trBalances)
                {
                    Console.WriteLine(s);
                }



                foreach (double s in trDates)
                {
                    Console.WriteLine(s);
                }
            }


            bm.checkMostReoccured();

        }
    }

}
