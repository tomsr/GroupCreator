using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Item> items = new List<Item>();
            Item item;
            DataTable datatable = new DataTable();
            StreamReader streamreader = new StreamReader("File/a.txt");
            char[] delimiter = new char[] { '\t' };
            string[] columnheaders = streamreader.ReadLine().Split(delimiter);
            foreach (string columnheader in columnheaders)
            {
                datatable.Columns.Add(columnheader); // I've added the column headers here.
            }

            while (streamreader.Peek() > 0)
            {
                DataRow datarow = datatable.NewRow();
                datarow.ItemArray = streamreader.ReadLine().Split(delimiter);
                datatable.Rows.Add(datarow);
            }

            foreach (DataRow row in datatable.Rows)
            {
                string id = row.ItemArray[4].ToString();
                string name = row.ItemArray[5].ToString();

                string dailySoldTxt = row.ItemArray[16].ToString();
                //dailySoldTxt = dailySoldTxt.Replace('.', ',');

                string ahPriceTxt = row.ItemArray[8].ToString();
                //ahPriceTxt = ahPriceTxt.Replace('.', ',');

                double dailySold;
                double ahPrice;

                if (Double.TryParse(dailySoldTxt, out dailySold))
                {
                    if (dailySold > 200)
                    {
                        item = new Item();
                        item.Id = id;
                        item.Name = name;
                        item.SoldInDayAvg = dailySold;

                        if (Double.TryParse(ahPriceTxt, out ahPrice))
                        {
                            item.AHPrice = ahPrice;
                            if (ahPrice > 2)
                            {
                                items.Add(item);

                            }
                        }

                    }
                }
            }


            List<Item> SortedList = items.OrderByDescending(o => o.SoldInDayAvg).ToList();

            printList(SortedList);

            Console.ReadLine();
        }
        public static void printList(List<Item> items)
        {
            int counter = 1;
            string tsmTXT = "";
            foreach (Item item in items)
            {

                string text = String.Format("{0,3} | Item ID: {1,10} | Sold: {2, 10} | AHPrice: {3, 10} | Name: {4, 30}",
                                    counter,
                                    item.Id,
                                    item.SoldInDayAvg,
                                    item.AHPrice,
                                    item.Name);
                tsmTXT += item.Id + ",";
                Console.WriteLine(text);
                counter++;
            }

            Console.WriteLine();
            tsmTXT = tsmTXT.Substring(0, tsmTXT.Length - 1);
            Console.WriteLine(tsmTXT);
        }

    }
}
