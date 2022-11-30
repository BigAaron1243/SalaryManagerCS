using System;
using System.Data;


namespace moneyclass;

public class salaryobj {
    private decimal grosspackage {get;set;}
    private int payfreq {get;set;}
    private decimal supercont {get;set;} = 0.095M;
    private decimal taxableincome {get;set;}


    //This will parse a file for dynamic deduction given an amount. File format is min~max:rule;min~max:rule. {am} will be replaced with the amount variable. rule is evaluated. Probably has a huge risk of abuse but im lazy.
    private decimal calc_dynamic(string filename, decimal amount) {
        try {
            string[] splitline = System.IO.File.ReadAllText(filename).Split(';');
            foreach (string element in splitline) {
                string[] storedvalue = element.Split(':');
                if (amount >= Int64.Parse(storedvalue[0].Split('~')[0]) && amount <= Int64.Parse(storedvalue[0].Split('~')[1])) {
                    return Math.Ceiling(Convert.ToDecimal(new DataTable().Compute(storedvalue[1].Replace("{am}", amount.ToString()), null)));
                }
            }
            return 0;
        } catch (Exception e) {
            Console.WriteLine($"File error in {filename} with exception {e}. sorry man, check your formatting");
            return 1;
        }
    }

    public string format(decimal amount) {
        string returnstr = Math.Round(amount, 2).ToString("#,##0.00");
        return "$" + returnstr;
    }

    public int set_payfreq(string input) {
        input = input.ToLower();
        if (input == "monthly" || input == "m") {
            payfreq = 12;
        } else if (input == "fortnightly" || input == "f") {
            payfreq = 26;
        } else if (input == "weekly" || input == "w") {
            payfreq = 52;
        } else {
            return 1;
        }
        return 0;
    }

    public int set_grosspackage(string input) {
        decimal value;
        if (Decimal.TryParse(input, out value)) {
            if (value < 0) {
                return 2;
            }
            grosspackage = value;
            taxableincome = grosspackage / (supercont + 1);
            return 0;
        } else {
            return 1;
        }
    }

    public decimal _get_grosspackage() {
        return grosspackage;
    }

    public decimal calc_supercont() {
        return grosspackage - taxableincome;
    }
    
    public decimal calc_taxableincome() {
        return taxableincome;
    }

    public decimal calc_medicarelevy() {
        return calc_dynamic(@".\medicare.txt", taxableincome);
    }

    public decimal calc_brl() {
        return calc_dynamic(@".\budget.txt", taxableincome);
    }

    public decimal calc_incometax() {
        return calc_dynamic(@".\incometax.txt", taxableincome);
    }

    public decimal calc_netincome() {
        return taxableincome - calc_medicarelevy() - calc_brl() - calc_incometax();
    }

    public decimal calc_paypacket() {
        return calc_netincome() / payfreq;
    }

}


