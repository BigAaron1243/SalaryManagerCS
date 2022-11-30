using System;
using moneyclass;


class program { 
	public static void Main() {
        salaryobj sman = new salaryobj();
        bool breakcon = false;
        while (!breakcon) {
            Console.Write("Enter your salary package amount: ");
            switch (sman.set_grosspackage(Console.ReadLine())) {
                case 0:
                    breakcon = true;
                    break;
                case 1:
                    Console.WriteLine("Please input a valid positive number.");
                    break;
                case 2:
                    Console.WriteLine("Please input a valid POSITIVE number.");
                    break;
            }
        }
        breakcon = false;

        while (!breakcon) {
            Console.Write("Enter your pay period (m for monthly, f for fortnightly, or w for weekly): ");
            switch (sman.set_payfreq(Console.ReadLine())) {
                case 0:
                    breakcon = true;
                    break;
                case 1:
                    Console.WriteLine("Please input m/monthly, f/fortnightly, or w/weekly.");
                    break;
            }
        }
        Console.WriteLine("\nCalculating salary details...");
        Console.WriteLine("\nGross package: " + sman.format(sman._get_grosspackage()));
        Console.WriteLine("Superannuation: " + sman.format(sman.calc_supercont()));
        Console.WriteLine("\nTaxable income: " + sman.format(sman.calc_taxableincome()));
        Console.WriteLine("\nDeductions:");
        Console.WriteLine("Medicare levy: " + sman.format(sman.calc_medicarelevy()));
        Console.WriteLine("Budget repair levy: " + sman.format(sman.calc_brl()));
        Console.WriteLine("Income tax: " + sman.format(sman.calc_incometax()));
        Console.WriteLine("\nNet income: " + sman.format(sman.calc_netincome()));
        Console.WriteLine("Pay packet: " + sman.format(sman.calc_paypacket()));
        Console.WriteLine("\nPress enter to end...");
        Console.ReadLine();
	}
}
