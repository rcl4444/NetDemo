using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AEOPoco.Domain;
using System.Collections.ObjectModel;
using AEOPoco.Other;

namespace AEOService.Interface
{
    public interface ICustomerCompanyService : IBaseService<CustomerCompany>
    {
        bool ServiceInitialize(CustomerCompany company,XmlDocument obj , out string message);

        CustomerCompany GetCompany(string uniqueFlag);

        CustomerCompany GetCompany(int companyID);

        CustomerCompany GetDeparement(int companyID);
    }
}
