using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Debt
{
    class Program
    {
        public static Dictionary<int, long> PayDic = new Dictionary<int, long>() { };
        public static void Main(string[] args)
        {
            PayDic[0] = 4000;
            PayDic[30] = 12000;
            PayDic[50] = 8000;
            long total = 0;
            double salary = 0;
            float cashPercentage = 0;
            float ratesPercentage = 0;
            double totalDebt = 0;
            int year = 0;
            double storage = 0;
            bool averageCapital = false;
            double perCapital = 0;
            double perAverage = 0;
            double perInterest = 0;
            //通货膨胀影响率
            double inflationEffectRates = 0.05;
            long pay = 0;
            int old = 0;
            Write("输入年龄 :");
            old = int.Parse(ReadLine());
            Write("输入房屋总价 :");
            total = long.Parse(ReadLine());
            Write("输入首付比例 :");
            cashPercentage = float.Parse(ReadLine());
            Write("输入每月收入 :");
            salary = double.Parse(ReadLine());
            Write("输入贷款利息 :");
            ratesPercentage = float.Parse(ReadLine());
            Write("输入贷款年限 :");
            year = int.Parse(ReadLine());
            Write("输入贷款方式 等额1 本金2 :");
            averageCapital = int.Parse(ReadLine()) == 2;
            totalDebt = total * (1 - cashPercentage);
            perCapital = totalDebt / (year * 12);
            if (!averageCapital)
            {
                perAverage = (totalDebt * ratesPercentage / 12 * Math.Pow((1 + ratesPercentage / 12), year * 12)) / (Math.Pow((1 + ratesPercentage / 12), year * 12) - 1);
            }
            pay = PayDic[0];
            for (int i = 1; i <= year * 12; i++)
            {
                if (i % 12 == 0)
                {
                    old += i / 12;
                    foreach (var item in PayDic.Keys)
                    {
                        if (old < item)
                        {
                            pay = PayDic[item];
                            break;
                        }
                    }
                    salary *= (double)(1 + inflationEffectRates);
                }
                WriteLine("第" + i + "个月：");
                perInterest = totalDebt * ratesPercentage / 12;
                if (!averageCapital)
                {
                    perCapital = perAverage - perInterest;
                }
                else
                {
                    perCapital = total * (1 - cashPercentage) / (year * 12);
                }

                if ((salary-pay) < perInterest + perCapital) { WriteLine("钱不够，贷个锤子款"); }
                else
                {
                    double rest = (salary-pay - perInterest - perCapital);
                    storage += rest;
                    if (storage >= totalDebt)
                    {
                        WriteLine("存够钱了，提前还了，结束！一次结清贷款 :" + totalDebt + "   结余:" + (storage - totalDebt));
                        break;
                    }
                    else
                    {
                        WriteLine("还款 : 本金" + perCapital + " 利息 ：" + perInterest);
                        WriteLine("结余存款 ：" + rest);
                        WriteLine("结余总存款 ：" + storage);
                        totalDebt -= perCapital;
                    }
                }
            }
            ReadKey();
        }
    }



}
